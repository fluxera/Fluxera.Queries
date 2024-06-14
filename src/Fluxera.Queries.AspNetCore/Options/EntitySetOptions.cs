namespace Fluxera.Queries.AspNetCore.Options
{
	using System;
	using Fluxera.Queries.Model;

	/// <summary>
	///		The options for an entity set.
	/// </summary>
	internal sealed class EntitySetOptions
	{
		/// <summary>
		///		Gets the entity set name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///		Gets the ID type.
		/// </summary>
		public Type KeyType { get; set; }

		/// <summary>
		///		Gets the entity set.
		/// </summary>
		public EntitySet EntitySet { get; set; }

		/// <summary>
		///		Gets the complex type options of the entity.
		/// </summary>
		public ComplexTypeOptions ComplexTypeOptions { get; } = new ComplexTypeOptions();

		/// <summary>
		///		Flag, indicating if the @odata.count value is always written in
		///		the response, overriding the value provided in the query string.
		/// </summary>
		public bool AlwaysIncludeCount { get; set; }
	}
}
