namespace Fluxera.Queries.Parsers
{
	using System;
	using System.Collections.Generic;
	using Fluxera.Queries.Model;

	internal static class PropertyParseHelper
	{
		public static IEnumerable<EdmProperty> ParseNestedProperties(string tokenValue, EdmComplexType edmComplexType)
		{
			List<EdmProperty> properties = new List<EdmProperty>();

			foreach(string propertyName in RewriteProperties(tokenValue/*, edmComplexType*/))
			{
				if(edmComplexType is null)
				{
					throw new QueryException($"Property {propertyName} not found.");
				}

				try
				{
					EdmProperty currentProperty = edmComplexType.GetProperty(propertyName);
					properties.Add(currentProperty);

					//// Special handling for the last property of enumeration.
					//if(currentProperty.PropertyType is not EdmComplexType)
					//{
					//	EdmProperty lastProperty = currentProperty;
					//	if(lastProperty.PropertyType.ClrType.IsEnumeration())
					//	{
					//		List<EdmProperty> enumerationProperties = new List<EdmProperty>();
					//		EdmComplexType complexType = new EdmComplexType(lastProperty.PropertyType.ClrType, enumerationProperties);
					//		currentProperty = new EdmProperty("Name", EdmPrimitiveType.String, complexType);

					//		enumerationProperties.Add(currentProperty);
					//		properties.Add(currentProperty);

					//		break;
					//	}
					//}

					edmComplexType = currentProperty.PropertyType as EdmComplexType;
				}
				catch(Exception ex)
				{
					throw new QueryException(ex.Message);
				}
			}

			return properties;
		}

		private static string[] RewriteProperties(string tokenValue/*, EdmComplexType edmComplexType*/)
		{
			return tokenValue.Split('/', StringSplitOptions.TrimEntries);

			//EdmProperty lastProperty = null;

			//foreach(string propertyName in properties)
			//{
			//	if(edmComplexType is null)
			//	{
			//		throw new QueryException($"Property {propertyName} not found.");
			//	}

			//	try
			//	{
			//		EdmProperty currentProperty = edmComplexType.GetProperty(propertyName);
			//		lastProperty = currentProperty;
			//		edmComplexType = currentProperty.PropertyType as EdmComplexType;
			//	}
			//	catch(Exception ex)
			//	{
			//		throw new QueryException(ex.Message);
			//	}
			//}

			//// Rewrite the properties for primitive value objects and strongly-typed IDs
			//// when the last property type one of those special types.
			//if(lastProperty is not null && 
			//	(lastProperty.PropertyType.ClrType.IsPrimitiveValueObject() || 
			//	 lastProperty.PropertyType.ClrType.IsStronglyTypedId()))
			//{
			//	// We add the property name 'Value' to the end.
			//	properties.Add("Value");
			//}

			//// Rewrite the properties for enumeration when the last property type is of
			//// this special type.
			//if(lastProperty is not null && lastProperty.PropertyType.ClrType.IsEnumeration())
			//{
			//	// We add the property name 'Value' to the end.
			//	properties.Add("Name");
			//}

			//return properties.ToArray();
		}
	}
}
