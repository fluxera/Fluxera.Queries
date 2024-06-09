namespace Fluxera.Queries.AspNetCore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Fluxera.Guards;
	using Fluxera.Queries.Model;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;

	/// <summary>
	///		The options for the data queries.
	/// </summary>
	[PublicAPI]
	public sealed class DataQueriesOptions
	{
		private readonly IDictionary<Type, EntitySetOptions> entitySetsByType = new Dictionary<Type, EntitySetOptions>();
		private readonly IDictionary<string, EntitySetOptions> entitySetsByName = new Dictionary<string, EntitySetOptions>();

		///  <summary>
		/// 		Configures an entity set for the given type and name.
		///  </summary>
		///  <typeparam name="T">The entity type.</typeparam>
		///  <param name="name">The entity set name.</param>
		///  <param name="configure">An action to configure the options for the entity set.</param>
		///  <exception cref="NotImplementedException"></exception>
		public void EntitySet<T>(string name, Action<EntitySetOptions> configure = null)
			where T : class
		{
			Guard.Against.NullOrWhiteSpace(name);

			EntitySetOptions entitySet = new EntitySetOptions();
			configure?.Invoke(entitySet);
			entitySet.Name = name;
			entitySet.EntityType = typeof(T);
			entitySet.IdentifierType = GetIdentifierType(typeof(T));

			if(!this.entitySetsByName.TryAdd(entitySet.Name, entitySet))
			{
				throw new InvalidOperationException($"An entity set with the name '{entitySet.Name}' was already configured.");
			}

			if(!this.entitySetsByType.TryAdd(entitySet.EntityType, entitySet))
			{
				throw new InvalidOperationException($"An entity set for the type '{entitySet.EntityType}' was already configured.");
			}
		}

		internal IReadOnlyCollection<EntitySetOptions> EntitySetOptions => this.entitySetsByType.Values.AsReadOnly();

		/// <summary>
		///		Gets the configured entity set for the given entity type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public EntitySet GetByType<T>() where T : class
		{
			return GetByType(typeof(T));
		}

		/// <summary>
		///		Gets the configured entity set for the given entity type.
		/// </summary>
		/// <param name="entityType"></param>
		/// <returns></returns>
		public EntitySet GetByType(Type entityType)
		{
			return this.entitySetsByType.TryGetValue(entityType, out EntitySetOptions options) 
				? options.EntitySet 
				: null;
		}

		private Type GetIdentifierType(Type entityType)
		{
			PropertyInfo propertyInfo = entityType.GetProperties().FirstOrDefault(x => x.Name.Equals("ID", StringComparison.OrdinalIgnoreCase));

			if(propertyInfo is null)
			{
				throw new InvalidOperationException($"The entity {entityType.Name} doesn't define an ID property.");
			}

			return propertyInfo.PropertyType;
		}
	}
}
