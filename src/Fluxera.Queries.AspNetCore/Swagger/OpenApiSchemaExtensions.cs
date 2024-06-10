namespace Fluxera.Queries.AspNetCore.Swagger
{
	using System;
	using Fluxera.Enumeration;
	using Fluxera.Queries.Model;
	using Fluxera.StronglyTypedId;
	using Fluxera.ValueObject;
	using Microsoft.OpenApi.Any;
	using Microsoft.OpenApi.Interfaces;
	using Microsoft.OpenApi.Models;

	internal static class OpenApiSchemaExtensions
	{
		public static void SetStronglyTypedId(this OpenApiSchema schema, Type type)
		{
			schema.Extensions["x-edm-stronglytypedid"] = new OpenApiBoolean(type.IsStronglyTypedId());
		}

		public static void SetPrimitiveValueObject(this OpenApiSchema schema, Type type)
		{
			schema.Extensions["x-edm-primitivevalueobject"] = new OpenApiBoolean(type.IsPrimitiveValueObject());
		}

		public static void SetEnumeration(this OpenApiSchema schema, Type type)
		{
			schema.Extensions["x-edm-enumeration"] = new OpenApiBoolean(type.IsEnumeration());
		}

		public static void SetComplexType(this OpenApiSchema schema, Type type, IEdmTypeProvider typeProvider)
		{
			EdmType edmType = typeProvider.GetByType(type);
			schema.Extensions["x-edm-complextype"] = new OpenApiBoolean(edmType is EdmComplexType);
		}

		public static void SetEdmTypeName(this OpenApiSchema schema, Type type, IEdmTypeProvider typeProvider)
		{
			EdmType edmType = typeProvider.GetByType(type);
			schema.Extensions["x-edm-edmtype"] = new OpenApiString(edmType.FullName);
		}

		public static bool IsStronglyTypedId(this OpenApiSchema schema)
		{
			if(!schema.Extensions.TryGetValue("x-edm-stronglytypedid", out IOpenApiExtension extension))
			{
				return false;
			}

			OpenApiBoolean isStronglyTypedId = (OpenApiBoolean)extension;
			return isStronglyTypedId.Value;
		}

		public static bool IsPrimitiveValueObject(this OpenApiSchema schema)
		{
			if(!schema.Extensions.TryGetValue("x-edm-primitivevalueobject", out IOpenApiExtension extension))
			{
				return false;
			}

			OpenApiBoolean isPrimitiveValueObject = (OpenApiBoolean)extension;
			return isPrimitiveValueObject.Value;
		}

		public static bool IsEnumeration(this OpenApiSchema schema)
		{
			if(!schema.Extensions.TryGetValue("x-edm-enumeration", out IOpenApiExtension extension))
			{
				return false;
			}

			OpenApiBoolean isEnumeration = (OpenApiBoolean)extension;
			return isEnumeration.Value;
		}

		public static bool IsComplexType(this OpenApiSchema schema)
		{
			if(!schema.Extensions.TryGetValue("x-edm-complextype", out IOpenApiExtension extension))
			{
				return false;
			}

			OpenApiBoolean isComplexType = (OpenApiBoolean)extension;
			return isComplexType.Value;
		}

		public static string GetEdmTypeName(this OpenApiSchema schema)
		{
			OpenApiString isComplexType = (OpenApiString)schema.Extensions["x-edm-edmtype"];
			return isComplexType.Value;
		}
	}
}
