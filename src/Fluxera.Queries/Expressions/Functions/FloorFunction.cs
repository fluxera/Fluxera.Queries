namespace Fluxera.Queries.Expressions.Functions
{
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class FloorFunction : QueryableFunction
	{
		public override string FunctionName => "floor";

		public override Expression CreateExpression(IList<Expression> arguments)
		{
			this.ValidateParameterCount(arguments, 1);

			Expression floorArg = arguments[0].Type == TypeUtilities.DoubleType
				? arguments[0]
				: Expression.Convert(arguments[0], TypeUtilities.DoubleType);

			return Expression.Call(null, Methods.MathFloor, floorArg);
		}
	}
}