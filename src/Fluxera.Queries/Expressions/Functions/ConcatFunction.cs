namespace Fluxera.Queries.Expressions.Functions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class ConcatFunction : QueryableFunction
	{
		public override string FunctionName => "concat";

		public override Expression CreateExpression(IList<Expression> arguments)
		{
			if(arguments.Count < 1)
			{
				throw new InvalidOperationException($"{this.FunctionName} needs at least 1 parameter.");
			}

			if(ReflectionHelper.IsEnumerable(arguments[0].Type, out Type itemType))
			{
				Expression result = arguments[0];
				MethodInfo concatMethod = Methods.EnumerableConcat.MakeGenericMethod(itemType);

				foreach(Expression arg in arguments.Skip(1))
				{
					result = Expression.Call(null, concatMethod, result, arg);
				}

				return result;
			}

			if(arguments[0].Type == TypeUtilities.StringType)
			{
				return Expression.Call(null, Methods.StringConcat, Expression.NewArrayInit(typeof(object), arguments));
			}

			return this.InvalidParameterTypes("strings, enumerables");
		}
	}
}
