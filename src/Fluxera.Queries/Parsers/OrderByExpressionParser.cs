namespace Fluxera.Queries.Parsers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Fluxera.Queries.Model;
	using Fluxera.Queries.Options;

	internal static class OrderByExpressionParser
	{
		public static OrderByProperty[] Parse(string oderByValue, EdmComplexType model)
		{
			OrderByProperty[] properties;

			if(oderByValue.Contains(','))
			{
				properties = oderByValue.Split(SplitCharacter.Comma, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
										.Select(propertyString => ParseOrderByProperty(propertyString, model))
										.ToArray();
			}
			else
			{
				properties =
				[
					ParseOrderByProperty(oderByValue, model)
				];
			}

			return properties;
		}

		private static OrderByProperty ParseOrderByProperty(string propertyString, EdmComplexType model)
		{
			Guard.ThrowIfNull(propertyString);
			Guard.ThrowIfNull(model);

			string[] parts = propertyString.Split(SplitCharacter.Space, StringSplitOptions.RemoveEmptyEntries);

			OrderByDirection direction = OrderByDirection.Ascending;

			string propertyName = parts[0];

			IEnumerable<EdmProperty> properties = PropertyParseHelper.ParseNestedProperties(propertyName, model);

			if(parts.Length != 1)
			{
				direction = parts[1] switch
				{
					"asc"  => OrderByDirection.Ascending,
					"desc" => OrderByDirection.Descending,
					_      => throw new QueryParserException(Messages.OrderByPropertyRawValueInvalid)
				};
			}

			return new OrderByProperty(properties, direction);
		}
	}
}
