namespace Fluxera.Queries.Options
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using Fluxera.Queries.Model;
	using Fluxera.Queries.Parsers;
	using JetBrains.Annotations;

	/// <summary>
	///     A class containing de-serialized values from the '$orderby' query option.
	/// </summary>
	[PublicAPI]
	public sealed class OrderByQueryOption : QueryOption
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="OrderByQueryOption" /> type.
		/// </summary>
		internal OrderByQueryOption(string expression, EdmComplexType edmType)
			: base(expression)
		{
			OrderByProperty[] properties = OrderByExpressionParser.Parse(expression, edmType);
			this.Properties = new ReadOnlyCollection<OrderByProperty>(properties);
		}

		/// <summary>
		///     Gets the properties the query should be ordered by.
		/// </summary>
		public IReadOnlyList<OrderByProperty> Properties { get; }

		/// <inheritdoc />
		public override string ToString()
		{
			if(this.Properties.Count == 0)
			{
				return "OrderBy=<none>";
			}

			string props = string.Join(",", this.Properties);

			return $"OrderBy={props}";
		}
	}
}
