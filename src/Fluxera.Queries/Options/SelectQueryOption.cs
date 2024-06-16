namespace Fluxera.Queries.Options
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using Fluxera.Queries.Model;
	using Fluxera.Queries.Parsers;
	using JetBrains.Annotations;

	/// <summary>
	///     A class containing de-serialized values from the $select query option.
	/// </summary>
	[PublicAPI]
	public sealed class SelectQueryOption : QueryOption
	{
		/// <inheritdoc />
		public SelectQueryOption(string expression, EdmComplexType edmType)
			: base(expression)
		{
			SelectProperty[] properties = SelectExpressionParser.Parse(expression, edmType);
			this.Properties = new ReadOnlyCollection<SelectProperty>(properties);
		}

		/// <summary>
		///     Gets the properties the query should select.
		/// </summary>
		public IReadOnlyList<SelectProperty> Properties { get; }

		/// <inheritdoc />
		public override string ToString()
		{
			if(this.Properties.Count == 0)
			{
				return "Select=*";
			}

			string properties = string.Join(",", this.Properties);

			return $"Select={properties}";
		}
	}
}
