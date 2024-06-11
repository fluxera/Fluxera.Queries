namespace Fluxera.Queries.AspNetCore.Options
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Fluxera.Guards;
	using Fluxera.Queries.Model;
	using Fluxera.StronglyTypedId;
	using Fluxera.Utilities.Extensions;
	using Fluxera.ValueObject;

	/// <summary>
	///		The options for the data queries.
	/// </summary>
	internal sealed class DataQueriesOptions
	{
		private readonly IDictionary<Type, EntitySetOptions> entitySetsByType = new Dictionary<Type, EntitySetOptions>();
		private readonly IDictionary<string, EntitySetOptions> entitySetsByName = new Dictionary<string, EntitySetOptions>();
		private readonly IDictionary<Type, ComplexTypeOptions> complexTypesByType = new Dictionary<Type, ComplexTypeOptions>();

		/// <summary>
		///		Configures a complex type the given type and complex type name.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="complexTypeName"></param>
		/// <param name="configure"></param>
		public void ComplexType<T>(string complexTypeName, Action<IComplexTypeOptionsBuilder<T>> configure = null)
			where T : class
		{
			Guard.Against.NullOrWhiteSpace(complexTypeName);

			if(typeof(T).IsEnumerable() || typeof(T).IsStronglyTypedId() || typeof(T).IsPrimitiveValueObject())
			{
				throw new ArgumentException($"The type {typeof(T).Name} is not a complex type.");
			}

			ComplexTypeOptions complexType = this.ComplexType(configure);

			complexType.TypeName = complexTypeName;
		}

		/// <summary>
		///		Configures a complex type the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="configure"></param>
		public ComplexTypeOptions ComplexType<T>(Action<IComplexTypeOptionsBuilder<T>> configure = null)
			where T : class
		{
			if(typeof(T).IsEnumerable() || typeof(T).IsStronglyTypedId() || typeof(T).IsPrimitiveValueObject())
			{
				throw new ArgumentException($"The type {typeof(T).Name} is not a complex type.");
			}

			ComplexTypeOptions complexType = new ComplexTypeOptions
			{
				ClrType = typeof(T)
			};

			if(!this.complexTypesByType.TryAdd(complexType.ClrType, complexType))
			{
				throw new InvalidOperationException($"A complex type for the type '{complexType.ClrType}' was already configured.");
			}

			ComplexTypeOptionsBuilder<T> builder = new ComplexTypeOptionsBuilder<T>(complexType);
			configure?.Invoke(builder);

			return complexType;
		}

		/// <summary>
		///		Configures an entity set for the given type, name and entity type name.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <param name="entityTypeName"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public void EntitySet<T>(string name, string entityTypeName, Action<IEntityTypeOptionsBuilder<T>> configure = null)
			where T : class
		{
			Guard.Against.NullOrWhiteSpace(entityTypeName);

			if(typeof(T).IsEnumerable() || typeof(T).IsStronglyTypedId() || typeof(T).IsPrimitiveValueObject())
			{
				throw new ArgumentException($"The type {typeof(T).Name} is not a complex type.");
			}

			EntitySetOptions entitySet = this.EntitySet(name, configure);

			entitySet.ComplexTypeOptions.TypeName = entityTypeName;
		}

		///  <summary>
		/// 	Configures an entity set for the given type and name.
		///  </summary>
		///  <typeparam name="T">The entity type.</typeparam>
		///  <param name="name">The entity set name.</param>
		///  <param name="configure">An action to configure the entity set.</param>
		///  <exception cref="NotImplementedException"></exception>
		public EntitySetOptions EntitySet<T>(string name, Action<IEntityTypeOptionsBuilder<T>> configure = null)
			where T : class
		{
			Guard.Against.NullOrWhiteSpace(name);

			if(typeof(T).IsEnumerable() || typeof(T).IsStronglyTypedId() || typeof(T).IsPrimitiveValueObject())
			{
				throw new ArgumentException($"The type {typeof(T).Name} is not a complex type.");
			}

			EntitySetOptions entitySet = new EntitySetOptions
			{
				Name = name,
				ComplexTypeOptions =
				{
					ClrType = typeof(T)
				}
			};

			if(!this.entitySetsByName.TryAdd(entitySet.Name, entitySet))
			{
				throw new InvalidOperationException($"An entity set with the name '{entitySet.Name}' was already configured.");
			}

			if(!this.entitySetsByType.TryAdd(entitySet.ComplexTypeOptions.ClrType, entitySet))
			{
				throw new InvalidOperationException($"An entity set for the type '{entitySet.ComplexTypeOptions.ClrType}' was already configured.");
			}

			EntityTypeOptionsBuilder<T> builder = new EntityTypeOptionsBuilder<T>(entitySet);
			configure?.Invoke(builder);

			entitySet.KeyType ??= GetIdentifierType(typeof(T));

			return entitySet;
		}

		internal IReadOnlyDictionary<Type, EntitySetOptions> EntitySetOptions => this.entitySetsByType.AsReadOnly();

		internal IReadOnlyDictionary<Type, ComplexTypeOptions> ComplexTypeOptions => this.complexTypesByType.AsReadOnly();

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

		private static Type GetIdentifierType(Type entityType)
		{
			PropertyInfo propertyInfo = entityType.GetProperties().FirstOrDefault(x => x.Name.Equals("ID", StringComparison.OrdinalIgnoreCase));

			return propertyInfo is null
				? throw new InvalidOperationException($"The entity {entityType.Name} doesn't define a key property named ID or Id.")
				: propertyInfo.PropertyType;
		}
	}
}
