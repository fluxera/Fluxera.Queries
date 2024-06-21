namespace Fluxera.Queries.Expressions.Functions
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class CaseFunction : QueryableFunction
	{
		/// <inheritdoc />
		public override string FunctionName => "case";

		/// <inheritdoc />
		public override Expression CreateExpression(IList<Expression> arguments)
		{
			throw new NotSupportedException($"The function {this.FunctionName} is not supported.");
		}
	}
}
