namespace Fluxera.Queries.Expressions.Functions
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class HasSubsetFunction : QueryableFunction
	{
		/// <inheritdoc />
		public override string FunctionName => "hassubset";

		/// <inheritdoc />
		public override Expression CreateExpression(IList<Expression> arguments)
		{
			throw new NotSupportedException($"The function {this.FunctionName} is not supported.");
		}
	}
}
