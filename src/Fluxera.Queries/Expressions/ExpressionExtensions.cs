namespace Fluxera.Queries.Expressions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using Fluxera.Guards;
	using Fluxera.Queries.Expressions.Functions;
	using Fluxera.Queries.Model;
	using Fluxera.Queries.Nodes;
	using Fluxera.Queries.Options;

	/// <summary>
	///		Extension methods for the query option types.
	/// </summary>
	internal static class ExpressionExtensions
	{
		private static readonly IDictionary<string, QueryableFunction> AvailableFunctions = typeof(QueryableFunction)
			.Assembly
			.GetTypes()
			.Where(type => typeof(QueryableFunction).IsAssignableFrom(type) && !type.IsAbstract)
			.Select(Activator.CreateInstance)
			.Cast<QueryableFunction>()
			.ToDictionary(function => function.FunctionName, function => function, StringComparer.OrdinalIgnoreCase);

		/// <summary>
		///		Creates a filter predicate expression.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="filterQueryOption"></param>
		/// <returns></returns>
		public static Expression<Func<T, bool>> ToPredicateExpression<T>(this FilterQueryOption filterQueryOption)
			where T : class
		{
			Guard.Against.Null(filterQueryOption);
			Guard.Against.Null(filterQueryOption.Expression, nameof(filterQueryOption.Expression));

			ParameterExpression parameter = Expression.Parameter(typeof(T), "x");

			Expression filterClause = CreateFilterExpression(filterQueryOption.Expression, parameter);

			Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(filterClause, parameter);

			return lambda;
		}

		///  <summary>
		/// 	Creates a search predicate expression.
		///  </summary>
		///  <typeparam name="T"></typeparam>
		///  <param name="searchQueryOption"></param>
		///  <param name="searchPredicate"></param>
		///  <returns></returns>
		public static Expression<Func<T, bool>> ToPredicateExpression<T>(this SearchQueryOption searchQueryOption, Expression<Func<T, string, bool>> searchPredicate)
			where T : class
		{
			Guard.Against.Null(searchQueryOption);
			Guard.Against.Null(searchQueryOption.Expression, nameof(searchQueryOption.Expression));
			Guard.Against.Null(searchPredicate);

			ParameterExpression parameter = searchPredicate.Parameters[0];

			Expression searchClause = CreateSearchExpression(searchQueryOption.Expression, searchPredicate);

			Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(searchClause, parameter);

			return lambda;
		}

		/// <summary>
		///		Creates a typed selector expression.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="selectQueryOption"></param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public static Expression<Func<T, T>> ToSelectorExpression<T>(this SelectQueryOption selectQueryOption)
			where T : class
		{
			Guard.Against.Null(selectQueryOption);
			
			ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
			Expression expression = parameter;

			if(selectQueryOption.Properties.Count > 0)
			{
				expression = CreateSelectorExpression(parameter, parameter.Type, selectQueryOption.Properties);
			}

			Expression<Func<T, T>> lambda = Expression.Lambda<Func<T, T>>(expression, parameter);

			return lambda;
		}

		/// <summary>	
		///		Creates OrderBy/ThenBy property expressions including the order direction.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="orderByQueryOption"></param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public static IReadOnlyCollection<OrderByExpression<T>> ToOrderByExpressions<T>(this OrderByQueryOption orderByQueryOption)
			where T : class
		{
			Guard.Against.Null(orderByQueryOption);

			if(orderByQueryOption.Properties.Count == 0)
			{
				return [];
			}

			IList<OrderByExpression<T>> expressions = new List<OrderByExpression<T>>();

			ParameterExpression parameter = Expression.Parameter(typeof(T), "x");

			foreach(OrderByProperty clause in orderByQueryOption.Properties)
			{
				// x.PropertyName
				Expression propertyExpression = parameter;

				foreach(EdmProperty edmProperty in clause.Properties)
				{
					PropertyInfo property = propertyExpression.Type.GetProperty(edmProperty.Name);

					if(property is null)
					{
						throw new InvalidOperationException($"Invalid property name {edmProperty.Name}.");
					}

					propertyExpression = Expression.MakeMemberAccess(propertyExpression, property);
				}

				// x => (object)x.PropertyName
				UnaryExpression convertExpression = Expression.Convert(propertyExpression, typeof(object));
				Expression<Func<T, object>> selectorExpression = Expression.Lambda<Func<T, object>>(convertExpression, parameter);

				expressions.Add(new OrderByExpression<T>(selectorExpression, clause.Direction));
			}

			return expressions.AsReadOnly();
		}

		private static Expression CreateSelectorExpression(Expression expression, Type type, IReadOnlyList<SelectProperty> properties)
		{
			IEnumerable<IGrouping<string, SelectProperty>> clauseGroups = properties
				.OrderByDescending(x => x.Properties.Count)
				.GroupBy(x => x.Properties[0].Name);

			IList<MemberBinding> bindings = new List<MemberBinding>();

			Expression parameter = expression;

			foreach(IGrouping<string, SelectProperty> clauseGroup in clauseGroups)
			{
				string groupName = clauseGroup.Key;
				PropertyInfo property = type.GetProperty(groupName);

				if(property is null)
				{
					throw new InvalidOperationException($"Invalid property name {groupName}.");
				}

				if(clauseGroup.Any(x => x.Properties.Count == 1))
				{
					MemberExpression memberAccess = Expression.MakeMemberAccess(parameter, property);
					MemberAssignment binding = Expression.Bind(property, memberAccess);
					bindings.Add(binding);
				}

				if(clauseGroup.Any(x => x.Properties.Count > 1))
				{
					IReadOnlyList<SelectProperty> clauses = clauseGroup
						.Select(x => new SelectProperty(x.Properties.ToArray()[1..]))
						.ToList()
						.AsReadOnly();

					Type propertyType = property.PropertyType;
					expression = Expression.MakeMemberAccess(parameter, property);

					expression = CreateSelectorExpression(expression, propertyType, clauses);
					MemberAssignment binding = Expression.Bind(property, expression);
					bindings.Add(binding);
				}
			}

			NewExpression newExpression = Expression.New(type);
			expression = Expression.MemberInit(newExpression, bindings);

			return expression;
		}

		private static Expression CreateSearchExpression<T>(QueryNode queryNode, Expression<Func<T, string, bool>> searchPredicate) 
			where T : class
		{
			return queryNode switch
			{
				ConstantNode constant =>
					CreateConstantSearchExpression(constant, searchPredicate),

				BinaryOperatorNode binaryOperator =>
					CreateBinaryOperatorSearchExpression(binaryOperator, searchPredicate),

				UnaryOperatorNode { OperatorKind: UnaryOperatorKind.Not } unaryOperator =>
					Expression.Not(CreateSearchExpression(unaryOperator.Operand, searchPredicate)),

				_ => throw new InvalidOperationException($"Invalid query {queryNode.Kind}.")
			};
		}

		private static Expression CreateConstantSearchExpression<T>(ConstantNode constant, Expression<Func<T, string, bool>> searchPredicate)
			where T : class
		{
			// Transforming a generic expression with two arguments into a single argument expression
			// that uses a constant value instead of a parameter.
			// 
			// Original:
			//				  (element, searchTerm) => element.MyValue.Contains(searchTerm)
			// 
			// Transformed:
			//                string searchTerm = "my current search text";
			//                element => element.MyValue.Contains(searchTerm);

			Expression argumentBoundSearchPredicate = ExpressionHelper.BindSecondArgument(searchPredicate, constant.Value.ToString());
			return argumentBoundSearchPredicate;
		}

		private static Expression CreateBinaryOperatorSearchExpression<T>(BinaryOperatorNode binaryOperatorNode, Expression<Func<T, string, bool>> searchPredicate)
			where T : class
		{
			Expression left = CreateSearchExpression(binaryOperatorNode.Left, searchPredicate);
			Expression right = CreateSearchExpression(binaryOperatorNode.Right, searchPredicate);
			BinaryOperatorKind kind = binaryOperatorNode.OperatorKind;

			// Boolean operations
			if(kind == BinaryOperatorKind.Or)
			{
				return Expression.OrElse(left, right);
			}

			if(kind == BinaryOperatorKind.And)
			{
				return Expression.AndAlso(left, right);
			}

			throw new InvalidOperationException("Unknown binary operator for $select.");
		}

		private static Expression CreateFilterExpression(QueryNode queryNode, Expression baseExpression)
		{
			return queryNode switch
			{
				ValueNode value =>
					CreateValueExpression(value, baseExpression),

				FunctionCallNode functionCall =>
					CreateFunctionCallExpression(functionCall.Name, functionCall.Parameters.CreateExpressions(baseExpression)),

				UnaryOperatorNode { OperatorKind: UnaryOperatorKind.Not } unaryOperator =>
					Expression.Not(CreateFilterExpression(unaryOperator.Operand, baseExpression)),

				BinaryOperatorNode binaryOperator =>
					CreateBinaryOperatorExpression(binaryOperator, baseExpression),

				_ => throw new InvalidOperationException($"Invalid query {queryNode.Kind}.")
			};
		}

		private static IList<Expression> CreateExpressions(this IEnumerable<QueryNode> nodes, Expression baseExpression)
		{
			return nodes.Select(n => CreateFilterExpression(n, baseExpression)).ToArray();
		}

		private static Expression CreateBinaryOperatorExpression(BinaryOperatorNode binaryOperator, Expression baseExpression)
		{
			Expression left = CreateFilterExpression(binaryOperator.Left, baseExpression);
			Expression right = CreateFilterExpression(binaryOperator.Right, baseExpression);
			BinaryOperatorKind kind = binaryOperator.OperatorKind;

			// Boolean operations
			if(kind == BinaryOperatorKind.Or)
			{
				return Expression.OrElse(left, right);
			}

			if(kind == BinaryOperatorKind.And)
			{
				return Expression.AndAlso(left, right);
			}

			// enum.HasFlag()
			if(kind == BinaryOperatorKind.Has)
			{
				return Expression.Call(null, Methods.HasFlag, left, Expression.Convert(right, typeof(Enum)));
			}

			// Collection contains
			if(kind == BinaryOperatorKind.In)
			{
				return CreateFunctionCallExpression("contains", [right, left]);
			}

			ExpressionType expressionType = kind switch
			{
				BinaryOperatorKind.Equal              => ExpressionType.Equal,
				BinaryOperatorKind.NotEqual           => ExpressionType.NotEqual,
				BinaryOperatorKind.GreaterThan        => ExpressionType.GreaterThan,
				BinaryOperatorKind.GreaterThanOrEqual => ExpressionType.GreaterThanOrEqual,
				BinaryOperatorKind.LessThan           => ExpressionType.LessThan,
				BinaryOperatorKind.LessThanOrEqual    => ExpressionType.LessThanOrEqual,
				BinaryOperatorKind.Add                => ExpressionType.Add,
				BinaryOperatorKind.Subtract           => ExpressionType.Subtract,
				BinaryOperatorKind.Multiply           => ExpressionType.Multiply,
				BinaryOperatorKind.Divide             => ExpressionType.Divide,
				BinaryOperatorKind.Modulo             => ExpressionType.Modulo,
				_                                     => throw new InvalidOperationException($"Invalid operator {kind}.")
			};

			// Let's "promote" expressions, so we have two expressions of a compatible
			// type in both sides of the binary expression
			Expression promotedLeftExpression = ExpressionHelper.Promote(left, right);
			Expression promotedRightExpression = ExpressionHelper.Promote(right, left);

			return Expression.MakeBinary(expressionType, promotedLeftExpression, promotedRightExpression);
		}

		private static Expression CreateValueExpression(ValueNode valueNode, Expression parameterExpression)
		{
			switch(valueNode)
			{
				case ConstantNode { Value: null }:
					return Expression.Constant(null);

				case ConstantNode { Value: not null } constant:
					return Expression.Constant(constant.Value, constant.EdmType.ClrType);

				case ArrayNode { Value: not null } array:
					return Expression.Constant(array.Value, array.ArrayClrType);

				case PropertyAccessNode propertyAccess:
					Expression expression = parameterExpression;

					foreach(EdmProperty edmProperty in propertyAccess.Properties)
					{
						PropertyInfo property = expression.Type.GetProperty(edmProperty.Name);

						if(property is null)
						{
							throw new InvalidOperationException($"Invalid property name {edmProperty.Name}.");
						}

						expression = Expression.MakeMemberAccess(expression, property);
					}

					return expression;
				default:
					throw new InvalidOperationException($"Invalid query value {valueNode.Kind}.");
			}
		}

		private static Expression CreateFunctionCallExpression(string functionName, IList<Expression> arguments)
		{
			if(!AvailableFunctions.TryGetValue(functionName, out QueryableFunction function))
			{
				throw new InvalidOperationException($"Could not find the function '{functionName}'.");
			}

			return function.CreateExpression(arguments);
		}
	}
}
