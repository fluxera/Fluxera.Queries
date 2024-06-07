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

			foreach(string propertyName in tokenValue.Split('/'))
			{
				if(edmComplexType is null)
				{
					throw new QueryException($"Property {propertyName} not found.");
				}

				try
				{
					EdmProperty currentProperty = edmComplexType.GetProperty(propertyName);
					properties.Add(currentProperty);
					edmComplexType = currentProperty.PropertyType as EdmComplexType;
				}
				catch(ArgumentException ex)
				{
					throw new QueryException(ex.Message);
				}
			}

			return properties;
		}
	}
}
