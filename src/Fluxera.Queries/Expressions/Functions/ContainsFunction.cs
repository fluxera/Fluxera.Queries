namespace Fluxera.Queries.Expressions.Functions
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class ContainsFunction : QueryableFunction
	{
		public override string FunctionName => "contains";

		public override Expression CreateExpression(IList<Expression> arguments)
		{
			this.ValidateParameterCount(arguments, 2);

			if(arguments[0].Type == TypeUtilities.StringType)
			{
				return Expression.Call(arguments[0], Methods.StringContains, arguments[1]);
			}

			if(!ReflectionHelper.IsEnumerable(arguments[0].Type, out Type itemType))
			{
				return this.InvalidParameterTypes("strings, enumerables");
			}

			Expression sourceArg = arguments[0];
			Expression elementArg = arguments[1];

			bool itemIsEnum = TypeUtilities.IsEnumOrNullableEnum(itemType, out Type itemEnumType);
			bool argIsEnum = TypeUtilities.IsEnumOrNullableEnum(elementArg.Type, out Type argEnumType);

			if(itemType == elementArg.Type || (!itemIsEnum && !argIsEnum))
			{
				return Expression.Call(null, Methods.EnumerableContains.MakeGenericMethod(itemType), sourceArg, elementArg);
			}

			// enum case: we need to convert/promote expression to enum types

			Type enumType = itemIsEnum ? itemEnumType : argEnumType;
			Type arrayType = itemIsEnum ? itemType : elementArg.Type;

			if(sourceArg is ConstantExpression sourceConstantExpression)
			{
				object[] sourceEnumerable = ((IEnumerable)sourceConstantExpression.Value).Cast<object>()
																						 .ToArray();
				Array enumArray = Array.CreateInstance(arrayType, sourceEnumerable.Length);

				for(int index = 0; index < sourceEnumerable.Length; index++)
				{
					object item = sourceEnumerable[index];
					enumArray.SetValue(ExpressionHelper.ToEnumValue(enumType, item), index);
				}

				sourceArg = Expression.Constant(enumArray, enumArray.GetType());

				itemType = arrayType;
			}
			else if(elementArg is ConstantExpression elementConstantExpression)
			{
				elementArg = Expression.Constant(ExpressionHelper.ToEnumValue(enumType, elementConstantExpression.Value));
			}

			return Expression.Call(null, Methods.EnumerableContains.MakeGenericMethod(itemType),
				sourceArg,
				elementArg);
		}
	}
}
