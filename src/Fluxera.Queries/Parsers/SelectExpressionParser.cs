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

			IEnumerable<EdmProperty> properties = PropertyParseHelper.ParseNestedProperties(propertyName, model);

			return new SelectProperty(properties);
		}
	}
}
