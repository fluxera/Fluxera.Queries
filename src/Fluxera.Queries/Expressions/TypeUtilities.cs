namespace Fluxera.Queries.Expressions
{
	using System;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[PublicAPI]
	internal static class TypeUtilities
	{
		public static readonly Type StringType = typeof(string);
		public static readonly Type DoubleType = typeof(double);
		public static readonly Type DateTimeOffsetType = typeof(DateTimeOffset);
		public static readonly Type DateTimeType = typeof(DateTime);
		public static readonly Type DateOnlyType = typeof(DateOnly);
		public static readonly Type TimeSpanType = typeof(TimeSpan);

		public static bool IsEnumOrNullableEnum(Type itemType, out Type enumType)
		{
			enumType = null;

			if(itemType.IsEnum)
			{
				enumType = itemType;
				return true;
			}

			Type underlyingType = Nullable.GetUnderlyingType(itemType);

			if(underlyingType?.IsEnum == true)
			{
				enumType = underlyingType;
				return true;
			}

			return false;
		}

		public static Type ParseTargetType(Expression argument)
		{
			if(argument is not ConstantExpression constantExpression)
			{
				return null;
			}

			if(constantExpression.Value is Type targetType)
			{
				return targetType;
			}

			string typeName = constantExpression.Value?.ToString();

			return GetBuiltInTypeByName(typeName);
		}

		private static Type GetBuiltInTypeByName(string typeName)
		{
			// primitiveTypeName
			if(typeName.StartsWith("Edm."))
			{
				typeName = typeName.Substring(4);
			}

			return typeName switch
			{
				"Boolean"        => typeof(bool),
				"Byte"           => typeof(byte),
				"Date"           => DateOnlyType,
				"DateTimeOffset" => DateTimeOffsetType,
				"Decimal"        => typeof(decimal),
				"Double"         => DoubleType,
				"Duration"       => TimeSpanType,
				"Guid"           => typeof(Guid),
				"Int16"          => typeof(short),
				"Int32"          => typeof(int),
				"Int64"          => typeof(long),
				"SByte"          => typeof(sbyte),
				"Single"         => typeof(float),
				"String"         => StringType,
				"TimeOfDay"      => typeof(TimeOnly),
				_                => null
			};
		}
	}
}
