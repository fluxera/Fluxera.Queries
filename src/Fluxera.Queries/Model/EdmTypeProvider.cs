namespace Fluxera.Queries.Model
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Fluxera.Enumeration;
	using Fluxera.StronglyTypedId;
	using Fluxera.ValueObject;
	using JetBrains.Annotations;

	/// <summary>
	///		Creates EDM types or provides access to cached types.
	/// </summary>
	[PublicAPI]
	internal sealed class EdmTypeProvider : IEdmTypeProvider
	{
		private static readonly Dictionary<Type, EdmType> PrimitiveTypes = new Dictionary<Type, EdmType>
		{
			[typeof(byte[])] = EdmPrimitiveType.Binary,
			[typeof(bool)] = EdmPrimitiveType.Boolean,
			[typeof(bool?)] = EdmPrimitiveType.Boolean,
			[typeof(byte)] = EdmPrimitiveType.Byte,
			[typeof(byte?)] = EdmPrimitiveType.Byte,
			[typeof(DateOnly)] = EdmPrimitiveType.Date,
			[typeof(DateOnly?)] = EdmPrimitiveType.Date,
			[typeof(DateTime)] = EdmPrimitiveType.DateTimeOffset,
			[typeof(DateTime?)] = EdmPrimitiveType.DateTimeOffset,
			[typeof(DateTimeOffset)] = EdmPrimitiveType.DateTimeOffset,
			[typeof(DateTimeOffset?)] = EdmPrimitiveType.DateTimeOffset,
			[typeof(decimal)] = EdmPrimitiveType.Decimal,
			[typeof(decimal?)] = EdmPrimitiveType.Decimal,
			[typeof(double)] = EdmPrimitiveType.Double,
			[typeof(double?)] = EdmPrimitiveType.Double,
			[typeof(TimeSpan)] = EdmPrimitiveType.Duration,
			[typeof(TimeSpan?)] = EdmPrimitiveType.Duration,
			[typeof(TimeOnly)] = EdmPrimitiveType.TimeOfDay,
			[typeof(TimeOnly?)] = EdmPrimitiveType.TimeOfDay,
			[typeof(Guid)] = EdmPrimitiveType.Guid,
			[typeof(Guid?)] = EdmPrimitiveType.Guid,
			[typeof(short)] = EdmPrimitiveType.Int16,
			[typeof(short?)] = EdmPrimitiveType.Int16,
			[typeof(int)] = EdmPrimitiveType.Int32,
			[typeof(int?)] = EdmPrimitiveType.Int32,
			[typeof(long)] = EdmPrimitiveType.Int64,
			[typeof(long?)] = EdmPrimitiveType.Int64,
			[typeof(sbyte)] = EdmPrimitiveType.SByte,
			[typeof(sbyte?)] = EdmPrimitiveType.SByte,
			[typeof(float)] = EdmPrimitiveType.Single,
			[typeof(float?)] = EdmPrimitiveType.Single,
			[typeof(char)] = EdmPrimitiveType.String,
			[typeof(char?)] = EdmPrimitiveType.String,
			[typeof(string)] = EdmPrimitiveType.String
		};

		private readonly ConcurrentDictionary<Type, EdmType> MapByType = new ConcurrentDictionary<Type, EdmType>(PrimitiveTypes);

		/// <inheritdoc />
		public EdmType GetByType(Type clrType)
		{
			EdmType edmType = this.MapByType.GetOrAdd(clrType, this.ResolveEdmType);

			return edmType;
		}

		/// <inheritdoc />
		public EdmType GetByName(string edmTypeName)
		{
			return this.MapByType.Values.FirstOrDefault(x => x.FullName == edmTypeName);
		}

		private EdmType ResolveEdmType(Type clrType)
		{
			return this.ResolveEdmType(clrType, new Dictionary<Type, EdmType>());
		}

		private EdmType ResolveEdmType(Type clrType, IDictionary<Type, EdmType> visitedTypes)
		{
			// Try to get the cached edm type.
			if(visitedTypes.TryGetValue(clrType, out EdmType visitedEdmType))
			{
				return visitedEdmType;
			}

			// Handle enums and enumerations.
			if(clrType.IsEnum || clrType.IsEnumeration())
			{
				return CreateEnumType(clrType);
			}

			// Handle strongly-typed IDs.
			if(clrType.IsStronglyTypedId())
			{
				return CreateStronglyTypedIdType(clrType, visitedTypes);
			}

			// Handle primitive value objects.
			if(clrType.IsPrimitiveValueObject())
			{
				return CreatePrimitiveValueObjectType(clrType, visitedTypes);
			}

			// Handle enumerables.
			if(clrType.IsGenericType)
			{
				Type innerType = clrType.GetGenericArguments()[0];
				if(typeof(IEnumerable<>).MakeGenericType(innerType).IsAssignableFrom(clrType))
				{
					EdmType containedType = this.MapByType.GetOrAdd(innerType, t => this.ResolveEdmType(t, visitedTypes));
					return this.MapByType.GetOrAdd(clrType, x => new EdmCollectionType(x, containedType));
				}
			}

			// Handle complex types.
			return this.CreateComplexType(clrType, visitedTypes);
		}

		private static EdmType CreateEnumType(Type clrType)
		{
			List<EdmEnumMember> members = new List<EdmEnumMember>();

			if(clrType.IsEnum)
			{
				foreach(object enumMember in Enum.GetValues(clrType))
				{
					members.Add(new EdmEnumMember(enumMember.ToString(), (int)enumMember));
				}
			}

			if(clrType.IsEnumeration())
			{
				foreach(IEnumeration enumMember in Enumeration.All(clrType))
				{
					members.Add(new EdmEnumMember(enumMember.Name, (int)enumMember.Value));
				}
			}

			EdmEnumType edmEnumType = new EdmEnumType(clrType, members.AsReadOnly());

			return edmEnumType;
		}

		private EdmType CreateComplexType(Type clrType, IDictionary<Type, EdmType> visitedTypes)
		{
			EdmType baseType = clrType.BaseType != typeof(object)
				? this.ResolveEdmType(clrType.BaseType, visitedTypes)
				: null;

			IOrderedEnumerable<PropertyInfo> clrTypeProperties =
				clrType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					   .Where(x => x.CanRead && x.CanWrite)
					   .OrderBy(x => x.Name);

			List<EdmProperty> edmProperties = new List<EdmProperty>();
			EdmComplexType edmComplexType = new EdmComplexType(clrType, baseType, edmProperties);

			visitedTypes[clrType] = edmComplexType;

			IEnumerable<EdmProperty> properties = clrTypeProperties.Select(
				property => new EdmProperty(property.Name,
					this.MapByType.GetOrAdd(property.PropertyType,
						type => this.ResolveEdmType(type, visitedTypes)), edmComplexType));

			edmProperties.AddRange(properties);

			return edmComplexType;
		}

		private EdmType CreateStronglyTypedIdType(Type clrType, IDictionary<Type, EdmType> visitedTypes)
		{
			EdmType baseType = clrType.BaseType != typeof(object)
				? this.ResolveEdmType(clrType.BaseType, visitedTypes)
				: null;

			IOrderedEnumerable<PropertyInfo> clrTypeProperties =
				clrType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					   .Where(x => x.CanRead)
					   .OrderBy(x => x.Name);

			List<EdmProperty> edmProperties = new List<EdmProperty>();
			EdmComplexType edmComplexType = new EdmComplexType(clrType, baseType, edmProperties);

			visitedTypes[clrType] = edmComplexType;

			IEnumerable<EdmProperty> properties = clrTypeProperties.Select(
				property => new EdmProperty(property.Name,
					this.MapByType.GetOrAdd(property.PropertyType,
						type => this.ResolveEdmType(type, visitedTypes)), edmComplexType));

			edmProperties.AddRange(properties);

			return edmComplexType;
		}

		private EdmType CreatePrimitiveValueObjectType(Type clrType, IDictionary<Type, EdmType> visitedTypes)
		{
			EdmType baseType = clrType.BaseType != typeof(object)
				? this.ResolveEdmType(clrType.BaseType, visitedTypes)
				: null;

			IOrderedEnumerable<PropertyInfo> clrTypeProperties =
				clrType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					   .Where(x => x.CanRead)
					   .OrderBy(x => x.Name);

			List<EdmProperty> edmProperties = new List<EdmProperty>();
			EdmComplexType edmComplexType = new EdmComplexType(clrType, baseType, edmProperties);

			visitedTypes[clrType] = edmComplexType;

			IEnumerable<EdmProperty> properties = clrTypeProperties.Select(
				property => new EdmProperty(property.Name,
					this.MapByType.GetOrAdd(property.PropertyType,
						type => this.ResolveEdmType(type, visitedTypes)), edmComplexType));

			edmProperties.AddRange(properties);

			return edmComplexType;
		}
	}
}
