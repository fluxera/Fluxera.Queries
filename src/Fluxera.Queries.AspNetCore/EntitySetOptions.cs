namespace Fluxera.Queries.AspNetCore
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using Fluxera.Queries.Model;
	using JetBrains.Annotations;

	/// <summary>
	///		The options for an entity set.
	/// </summary>
	[PublicAPI]
	public sealed class EntitySetOptions
	{
		/// <summary>
		///		Gets the entity set name.
		/// </summary>
		internal string Name { get; set; }

		/// <summary>
		///		Gets the ID type.
		/// </summary>
		internal Type KeyType { get; set; }

		/// <summary>
		///		Gets the entity set.
		/// </summary>
		internal EntitySet EntitySet { get; set; }

		/// <summary>
		///		Gets the complex type options of the entity.
		/// </summary>
		internal ComplexTypeOptions ComplexTypeOptions { get; } = new ComplexTypeOptions();

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
