namespace Fluxera.Queries.Parsers
{
	using System;
	using System.Globalization;
	using Fluxera.Queries.Model;
	using Fluxera.Queries.Nodes;

	internal static class ConstantNodeParser
	{
		private const string DateFormat = "yyyy-MM-dd";

		public static ConstantNode ParseConstantNode(Token token, IEdmTypeProvider typeProvider)
		{
			return token.TokenType switch
			{
				TokenType.Date           => ParseDate(token.Value),
				TokenType.DateTimeOffset => ParseDateTimeOffset(token.Value),
				TokenType.Duration       => ParseDuration(token.Value),
				TokenType.TimeOfDay      => ParseTimeOfDay(token.Value),
				TokenType.Integer        => ParseInteger(token.Value),
				TokenType.Decimal        => ParseDecimal(token.Value),
				TokenType.Single         => ParseSingle(token.Value),
				TokenType.Double         => ParseDouble(token.Value),
				TokenType.Enum           => ParseEnum(token.Value, typeProvider),
				TokenType.Guid           => ParseGuid(token.Value),
				TokenType.String         => ParseString(token.Value),
				TokenType.True           => ConstantNode.True,
				TokenType.False          => ConstantNode.False,
				TokenType.Null           => ConstantNode.Null,
				_                        => throw new NotSupportedException(token.TokenType.ToString())
			};
		}

		private static ConstantNode ParseDate(string value)
		{
			DateOnly dateOnlyValue = DateOnly.ParseExact(value, DateFormat, CultureInfo.InvariantCulture);
			return ConstantNode.Date(value, dateOnlyValue);
		}

		private static ConstantNode ParseDateTimeOffset(string value)
		{
			DateTimeOffset dateTimeOffsetValue = DateTimeOffset.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
			return ConstantNode.DateTimeOffset(value, dateTimeOffsetValue);
		}

		private static ConstantNode ParseDuration(string value)
		{
			string durationText = value.Substring(9, value.Length - 10)
									   .Replace("P", string.Empty)
									   .Replace("DT", ".")
									   .Replace("H", ":")
									   .Replace("M", ":")
									   .Replace("S", string.Empty);
			TimeSpan durationTimeSpanValue = TimeSpan.Parse(durationText, CultureInfo.InvariantCulture);
			return ConstantNode.Duration(value, durationTimeSpanValue);
		}

		private static ConstantNode ParseTimeOfDay(string value)
		{
			TimeSpan timeOfDayValue = TimeSpan.Parse(value, CultureInfo.InvariantCulture);
			return ConstantNode.Time(value, timeOfDayValue);
		}

		private static ConstantNode ParseInteger(string value)
		{
			switch(value)
			{
				case "0":
					return ConstantNode.Int32Zero;
				case "0l" or "0L":
					return ConstantNode.Int64Zero;
			}

			bool is64BitSuffix = value.EndsWith("l", StringComparison.OrdinalIgnoreCase);

			if(!is64BitSuffix && int.TryParse(value, out int int32Value))
			{
				return ConstantNode.Int32(value, int32Value);
			}

			string int64Text = !is64BitSuffix ? value : value.Substring(0, value.Length - 1);
			long int64Value = long.Parse(int64Text, CultureInfo.InvariantCulture);
			return ConstantNode.Int64(value, int64Value);
		}

		private static ConstantNode ParseDecimal(string value)
		{
			string decimalText = value.Substring(0, value.Length - 1);
			decimal decimalValue = decimal.Parse(decimalText, CultureInfo.InvariantCulture);
			return ConstantNode.Decimal(value, decimalValue);
		}

		private static ConstantNode ParseSingle(string value)
		{
			string singleText = value.Substring(0, value.Length - 1);
			float singleValue = float.Parse(singleText, CultureInfo.InvariantCulture);
			return ConstantNode.Single(value, singleValue);
		}

		private static ConstantNode ParseDouble(string value)
		{
			string doubleText = value.EndsWith("d", StringComparison.OrdinalIgnoreCase)
				? value.Substring(0, value.Length - 1)
				: value;
			double doubleValue = double.Parse(doubleText, CultureInfo.InvariantCulture);
			return ConstantNode.Double(value, doubleValue);
		}

		private static ConstantNode ParseEnum(string value, IEdmTypeProvider typeProvider)
		{
			int firstQuote = value.IndexOf('\'');
			string edmEnumTypeName = value.Substring(0, firstQuote);
			EdmEnumType edmEnumType = (EdmEnumType)typeProvider.GetByName(edmEnumTypeName);
			string edmEnumMemberName = value.Substring(firstQuote + 1, value.Length - firstQuote - 2);
			object enumValue = edmEnumType.GetClrValue(edmEnumMemberName);

			return new ConstantNode(edmEnumType, value, enumValue);
		}

		private static ConstantNode ParseGuid(string value)
		{
			Guid guidValue = Guid.ParseExact(value, "D");
			return ConstantNode.Guid(value, guidValue);
		}

		private static ConstantNode ParseString(string value)
		{
			string stringText = UnescapeStringText(value);
			return ConstantNode.String(value, stringText);
		}

		private static string UnescapeStringText(string tokenValue)
		{
			return tokenValue
				   .Trim('\'')
				   .Replace("''", "'")
				   .Replace("%2B", "+")
				   .Replace("%2F", "/")
				   .Replace("%3F", "?")
				   .Replace("%25", "%")
				   .Replace("%23", "#")
				   .Replace("%26", "&");
		}
	}
}
