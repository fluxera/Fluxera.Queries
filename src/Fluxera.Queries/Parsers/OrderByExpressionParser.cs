namespace Fluxera.Queries.Parsers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Fluxera.Guards;
	using Fluxera.Queries.Model;
	using Fluxera.Queries.Options;

	internal static class SelectExpressionParser
	{
		public static SelectProperty[] Parse(string expression, EdmComplexType model)
		{
			SelectProperty[] properties;

			if(expression.Contains(','))
			{
				properties = expression.Split(SplitCharacter.Comma, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
									   .Select(propertyString => ParseSelectProperty(propertyString, model))
									   .ToArray();
			}
			else
			{
				properties =
				[
					ParseSelectProperty(expression.Trim(), model)
				];
			}

			return properties;
		}

		private static SelectProperty ParseSelectProperty(string propertyName, EdmComplexType model)
		{
			Guard.Against.Null(propertyName);
			Guard.Against.Null(model);

			EdmProperty property = model.GetProperty(propertyName);

			return new SelectProperty(property);
		}
	}

	internal static class OrderByExpressionParser
	{
		public static OrderByProperty[] Parse(string expression, EdmComplexType model)
		{
			OrderByProperty[] properties;

			if(expression.Contains(','))
			{
				properties = expression.Split(SplitCharacter.Comma, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
										.Select(propertyString => ParseOrderByProperty(propertyString, model))
										.ToArray();
			}
			else
			{
				properties =
				[
					ParseOrderByProperty(expression, model)
				];
			}

			return properties;
		}

		private static OrderByProperty ParseOrderByProperty(string propertyString, EdmComplexType model)
		{
			Guard.Against.Null(propertyString);
			Guard.Against.Null(model);

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
					_      => throw new QueryException(Messages.OrderByPropertyRawValueInvalid)
				};
			}

			return new OrderByProperty(properties, direction);
		}
	}
}
