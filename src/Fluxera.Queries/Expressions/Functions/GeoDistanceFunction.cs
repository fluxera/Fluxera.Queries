namespace Fluxera.Queries.Expressions.Functions
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	// https://devblogs.microsoft.com/odata/customizing-filter-for-spatial-data-in-asp-net-core-odata-8/
	[UsedImplicitly]
	internal sealed class GeoDistanceFunction : QueryableFunction
	{
		/// <inheritdoc />
		public override string FunctionName => "geo.distance";

		/// <inheritdoc />
		public override Expression CreateExpression(IList<Expression> arguments)
		{
			this.ValidateParameterCount(arguments, 2);

			throw new NotSupportedException($"The function {this.FunctionName} is not supported.");
		}
	}
}