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

		///// <summary>
		/////		Gets or sets the (optional) default $top value when no value
		/////		is provided in the query string.
		///// </summary>
		//internal int? DefaultTop { get; set; }

		///// <summary>
		/////		Gets or sets the (optional) maximum $top value even if a higher
		/////		values is provided in the query string.
		///// </summary>
		//internal int? MaxTop { get; set; }

		///// <summary>
		/////		Flag, indicating if the @odata.count value is always written in
		/////		the response, overriding the value provided oin the query string.
		///// </summary>
		//internal bool AlwaysIncludeCount { get; set; }
	}
}
