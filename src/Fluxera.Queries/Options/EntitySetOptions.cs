namespace Fluxera.Queries.Options
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
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
		public string Name { get; set; }

		/// <summary>
		///		Gets the entity set.
		/// </summary>
		public EntitySet EntitySet { get; set; }

		/// <summary>
		///		Gets the ID type.
		/// </summary>
		internal Type KeyType { get; set; }

		/// <summary>
		///		Gets the complex type options of the entity.
		/// </summary>
		internal ComplexTypeOptions ComplexTypeOptions { get; } = new ComplexTypeOptions();

		/// <summary>
		///		Gets the registered metadata for the entity set.
		/// </summary>
		internal IDictionary<string, object> Metadata { get; private set; } = new Dictionary<string, object>();

		/// <summary>
		///		Gets the search predicate expression.
		/// </summary>
		internal LambdaExpression SearchPredicate { get; set; }

		/// <summary>
		///		Flag, indicating if the @odata.count value is always written in
		///		the response, overriding the value provided in the query string.
		/// </summary>
		internal bool AlwaysIncludeCount { get; set; }

		/// <summary>
		///		Gets the max top value.
		/// </summary>
		internal int? MaxTop { get; set; }

		/// <summary>
		///		Gets the default top value.
		/// </summary>
		internal int? DefaultTop { get; set; }

		/// <summary>
		///		Gets a metadata entry.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public object GetMetadata(string key)
		{
			this.Metadata.TryGetValue(key, out object entry);
			return entry;
		}
	}
}
