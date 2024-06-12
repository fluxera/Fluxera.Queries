namespace Fluxera.Queries.Expressions
{
	using System;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.Serialization;
	using Fluxera.Enumeration;
	using Fluxera.StronglyTypedId;
	using Fluxera.Utilities.Extensions;
	using Fluxera.ValueObject;

	internal static class ExpressionHelper
	{
		/// <summary>
		///		Rewrites an expression by substituting the argument in the expression with a constant value.
		/// </summary>
		/// <param name="source">Source expression</param>
		/// <param name="argument">Constant value to bind</param>
		public static Expression<Func<T1, TResult>> BindSecondArgument<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> source, T2 argument)
		{
			ConstantExpression arg2 = Expression.Constant(argument, typeof(T2));
			Rewriter rewriter = new Rewriter(source.Parameters[1], arg2);
			Expression newBody = rewriter.Visit(source.Body);
			return Expression.Lambda<Func<T1, TResult>>(newBody!, source.Parameters[0]);
		}

		private class Rewriter : ExpressionVisitor
		{
			private readonly Expression candidate;
			private readonly Expression replacement;

			public Rewriter(Expression candidate, Expression replacement)
			{
				this.candidate = candidate ?? throw new ArgumentNullException(nameof(candidate));
				this.replacement = replacement ?? throw new ArgumentNullException(nameof(replacement));
			}

			public override Expression Visit(Expression node)
			{
				return node == this.candidate ? this.replacement : base.Visit(node);
			}
		}

		/// <summary>
		///		Promotes two expressions into a compatible type (e.g. float, double => double, double).
		/// </summary>
		/// <param name="toPromote">Expression to promote</param>
		/// <param name="other">Other side of expression</param>
		/// <returns></returns>
		public static Expression Promote(Expression toPromote, Expression other)
		{
			Type otherExpressionType = other.Type;

			if(toPromote.Type == otherExpressionType)
			{
				return toPromote;
			}

			(Type toPromotePrimitiveType, Type toPromoteNullableType) = GetUnderlyingType(toPromote.Type);
			(Type otherPrimitiveType, Type otherNullableType) = GetUnderlyingType(otherExpressionType);

			bool toPromoteIsNullable = toPromoteNullableType is not null;

			if(toPromote.Type.IsValueType && otherExpressionType == typeof(object) && toPromoteIsNullable)
			{
				return Expression.Convert(toPromote, otherExpressionType);
			}

			// Promote 
			if(otherPrimitiveType.IsPrimitiveValueObject())
			{
				Type valueType = otherPrimitiveType.GetPrimitiveValueObjectValueType();
				if(valueType == toPromotePrimitiveType)
				{
					// We can promote only ConstantsExpressions from the primitive value.
					if(toPromote is ConstantExpression constant)
					{
						if(constant.Value is null)
						{
							return constant;
						}

						object constantValue = constant.Value;

						MethodInfo methodInfo = otherPrimitiveType.GetMethod("Create", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
						constantValue = methodInfo?.Invoke(null, [constantValue]) ?? constantValue;

						ConstantExpression constantExpression = Expression.Constant(constantValue, otherPrimitiveType);
						return constantExpression;
					}
				}
			}

			if(otherPrimitiveType.IsEnumeration())
			{
				Type valueType = typeof(string);
				if(valueType == toPromotePrimitiveType)
				{
					// We can promote only ConstantsExpressions from the enumeration name.
					if(toPromote is ConstantExpression constant)
					{
						if(constant.Value is null)
						{
							return constant;
						}

						object constantValue = constant.Value;

						MethodInfo methodInfo = otherPrimitiveType.GetMethod("ParseName", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
						constantValue = methodInfo?.Invoke(null, [constantValue, false]) ?? constantValue;

						ConstantExpression constantExpression = Expression.Constant(constantValue, otherPrimitiveType);
						return constantExpression;
					}
				}
			}

			if(otherPrimitiveType.IsStronglyTypedId())
			{
				Type valueType = otherPrimitiveType.GetStronglyTypedIdValueType();
				if(valueType == toPromotePrimitiveType)
				{
					// We can promote only ConstantsExpressions from the ID value.
					if(toPromote is ConstantExpression constant)
					{
						if(constant.Value is null)
						{
							return constant;
						}

						object constantValue = constant.Value;

						MethodInfo methodInfo = otherPrimitiveType.GetMethod("Create", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
						constantValue = methodInfo?.Invoke(null, [constantValue]) ?? constantValue;

						ConstantExpression constantExpression = Expression.Constant(constantValue, otherPrimitiveType);
						return constantExpression;
					}
				}
			}

			if(toPromotePrimitiveType == typeof(string) && otherPrimitiveType.IsEnum)
			{
				// We can promote only ConstantsExpressions from string.
				if(toPromote is ConstantExpression constant)
				{
					if(constant.Value is null)
					{
						return constant;
					}

					string stringValue = (string)constant.Value;

					object enumValue = ParseEnum(otherPrimitiveType, stringValue);

					return Expression.Constant(enumValue, otherNullableType ?? otherPrimitiveType);
				}
			}

			if(toPromotePrimitiveType == TypeUtilities.DateOnlyType &&
				(otherPrimitiveType == TypeUtilities.DateTimeType || otherPrimitiveType == TypeUtilities.DateTimeOffsetType))
			{
				// We support constant values directly, for other kind of mappings we need to convert DateTime to DateOnly below.
				if(toPromote is ConstantExpression constant)
				{
					if(constant.Value is null)
					{
						return constant;
					}

					DateOnly dateOnlyValue = (DateOnly)constant.Value;

					DateTime dateTimeValue = dateOnlyValue.ToDateTime(new TimeOnly(), DateTimeKind.Local);

					if(otherPrimitiveType == TypeUtilities.DateTimeOffsetType)
					{
						return Expression.Constant(new DateTimeOffset(dateTimeValue), otherNullableType ?? otherPrimitiveType);
					}

					return Expression.Constant(dateTimeValue, otherNullableType ?? otherPrimitiveType);
				}
			}

			// We can support only conversion to DateOnly from DateTime.
			if(toPromotePrimitiveType == TypeUtilities.DateTimeType && otherPrimitiveType == TypeUtilities.DateOnlyType)
			{
				if(other is ConstantExpression)
				{
					// The other side is a constant expression, it will be converted by this method if possible the above if block.
					return toPromote;
				}

				// We can short-circuit a constant value.
				if(toPromote is ConstantExpression constant)
				{
					if(constant.Value is null)
					{
						return constant;
					}

					DateTime dateTimeValue = (DateTime)constant.Value;

					DateOnly dateOnlyValue = DateOnly.FromDateTime(dateTimeValue);

					return Expression.Constant(dateOnlyValue, otherNullableType ?? otherPrimitiveType);
				}

				if(!toPromoteIsNullable)
				{
					return ConvertToDateOnlyExpression(toPromote, otherNullableType != null);
				}

				Type targetType = otherNullableType ?? typeof(Nullable<>).MakeGenericType(otherPrimitiveType);

				MemberExpression testExpression = Expression.MakeMemberAccess(toPromote, Methods.NullableDateTimeHasValue);
				MemberExpression valueAccess = Expression.MakeMemberAccess(toPromote, Methods.NullableDateTimeValue);
				Expression convertedValueAccess = ConvertToDateOnlyExpression(valueAccess, true);
				ConstantExpression nullConstant = Expression.Constant(null, targetType);

				return Expression.Condition(
					testExpression,
					convertedValueAccess,
					nullConstant,
					targetType
				);
			}

			if(toPromotePrimitiveType == TypeUtilities.DateTimeOffsetType && otherPrimitiveType == TypeUtilities.DateOnlyType)
			{
				if(other is ConstantExpression)
				{
					// The other side is a constant expression, it will be converted by this method if possibly on the above if block.
					return toPromote;
				}

				// we can short-circuit a constant value
				if(toPromote is ConstantExpression constant)
				{
					if(constant.Value is null)
					{
						return constant;
					}

					DateTimeOffset dateTimeValue = (DateTimeOffset)constant.Value;

					DateOnly dateOnlyValue = DateOnly.FromDateTime(dateTimeValue.LocalDateTime);

					return Expression.Constant(dateOnlyValue, otherNullableType ?? otherPrimitiveType);
				}

				// WARNING: this is a workaround because there is no real option in LINQ to convert a DateOnly into a DateTimeOffset
				// and vice-versa. This double cast will work with EF Core SQL Server (and maybe other providers)
				// but not on other IQueryable providers (including InMemory)
				UnaryExpression castToObject = Expression.Convert(toPromote, typeof(object));
				return ConvertExpression(castToObject, toPromoteIsNullable, otherPrimitiveType, otherNullableType);
			}

			if(toPromotePrimitiveType == TypeUtilities.DateTimeType && otherPrimitiveType == TypeUtilities.DateTimeOffsetType)
			{
				return ConvertExpression(toPromote, toPromoteIsNullable, otherPrimitiveType, otherNullableType);
			}

			if(toPromotePrimitiveType == typeof(sbyte))
			{
				if(otherPrimitiveType == typeof(short) ||
					otherPrimitiveType == typeof(int) ||
					otherPrimitiveType == typeof(long) ||
					otherPrimitiveType == typeof(float) ||
					otherPrimitiveType == typeof(double) ||
					otherPrimitiveType == typeof(decimal))
				{
					return ConvertExpression(toPromote, toPromoteIsNullable, otherPrimitiveType, otherNullableType);
				}
			}

			if(toPromotePrimitiveType == typeof(byte))
			{
				if(otherPrimitiveType == typeof(short) ||
					otherPrimitiveType == typeof(ushort) ||
					otherPrimitiveType == typeof(int) ||
					otherPrimitiveType == typeof(uint) ||
					otherPrimitiveType == typeof(long) ||
					otherPrimitiveType == typeof(ulong) ||
					otherPrimitiveType == typeof(float) ||
					otherPrimitiveType == typeof(double) ||
					otherPrimitiveType == typeof(decimal))
				{
					return ConvertExpression(toPromote, toPromoteIsNullable, otherPrimitiveType, otherNullableType);
				}
			}

			if(toPromotePrimitiveType == typeof(short))
			{
				if(otherPrimitiveType == typeof(int) ||
					otherPrimitiveType == typeof(long) ||
					otherPrimitiveType == typeof(float) ||
					otherPrimitiveType == typeof(double) ||
					otherPrimitiveType == typeof(decimal))
				{
					return ConvertExpression(toPromote, toPromoteIsNullable, otherPrimitiveType, otherNullableType);
				}
			}

			if(toPromotePrimitiveType == typeof(ushort))
			{
				if(otherPrimitiveType == typeof(int) ||
					otherPrimitiveType == typeof(uint) ||
					otherPrimitiveType == typeof(long) ||
					otherPrimitiveType == typeof(ulong) ||
					otherPrimitiveType == typeof(float) ||
					otherPrimitiveType == typeof(double) ||
					otherPrimitiveType == typeof(decimal))
				{
					return ConvertExpression(toPromote, toPromoteIsNullable, otherPrimitiveType, otherNullableType);
				}
			}

			if(toPromotePrimitiveType == typeof(int))
			{
				if(otherPrimitiveType == typeof(long) ||
					otherPrimitiveType == typeof(float) ||
					otherPrimitiveType == typeof(double) ||
					otherPrimitiveType == typeof(decimal) ||
					otherPrimitiveType.IsEnum)
				{
					return ConvertExpression(toPromote, toPromoteIsNullable, otherPrimitiveType, otherNullableType);
				}
			}

			if(toPromotePrimitiveType == typeof(uint))
			{
				if(otherPrimitiveType == typeof(long) ||
					otherPrimitiveType == typeof(ulong) ||
					otherPrimitiveType == typeof(float) ||
					otherPrimitiveType == typeof(double) ||
					otherPrimitiveType == typeof(decimal))
				{
					return ConvertExpression(toPromote, toPromoteIsNullable, otherPrimitiveType, otherNullableType);
				}
			}

			if(toPromotePrimitiveType == typeof(long))
			{
				if(otherPrimitiveType == typeof(float) ||
					otherPrimitiveType == typeof(double) ||
					otherPrimitiveType == typeof(decimal))
				{
					return ConvertExpression(toPromote, toPromoteIsNullable, otherPrimitiveType, otherNullableType);
				}
			}

			if(toPromotePrimitiveType == typeof(ulong))
			{
				if(otherPrimitiveType == typeof(float) ||
					otherPrimitiveType == typeof(double) ||
					otherPrimitiveType == typeof(decimal))
				{
					return ConvertExpression(toPromote, toPromoteIsNullable, otherPrimitiveType, otherNullableType);
				}
			}

			if(toPromotePrimitiveType == typeof(float))
			{
				if(otherPrimitiveType == typeof(double) ||
					otherPrimitiveType == typeof(decimal))
				{
					return ConvertExpression(toPromote, toPromoteIsNullable, otherPrimitiveType, otherNullableType);
				}
			}

			if(toPromotePrimitiveType == typeof(double))
			{
				if(otherPrimitiveType == typeof(decimal))
				{
					return ConvertExpression(toPromote, toPromoteIsNullable, otherPrimitiveType, otherNullableType);
				}
			}

			if(otherNullableType is not null && !otherPrimitiveType.IsEnum && toPromotePrimitiveType != typeof(object))
			{
				// This will promote nullables to the best matching type of nullable (in this case is the same type of the "toPromote" type).

				toPromoteNullableType ??= typeof(Nullable<>).MakeGenericType(toPromotePrimitiveType);
				return ConvertExpression(toPromote, toPromoteIsNullable, toPromotePrimitiveType, toPromoteNullableType);
			}

			return toPromote;
		}

		private static Expression ConvertExpression(Expression toPromote, bool expressionTypeIsNullable, Type otherType, Type otherNullableType)
		{
			if(!expressionTypeIsNullable && otherNullableType is null) return Expression.Convert(toPromote, otherType);

			if(otherNullableType is not null) return Expression.Convert(toPromote, otherNullableType);

			Type nullableType = typeof(Nullable<>).MakeGenericType(otherType);

			return Expression.Convert(toPromote, nullableType);
		}

		private static (Type primitiveType, Type nullableType) GetUnderlyingType(Type type)
		{
			Type nullableUnderlyingType = Nullable.GetUnderlyingType(type);

			if(nullableUnderlyingType is not null)
			{
				return (nullableUnderlyingType, type);
			}

			return (type, null);
		}

		public static object ToEnumValue(Type enumType, object value)
		{
			if(value is int) return Enum.ToObject(enumType, value);

			if(value is string stringValue) return ParseEnum(enumType, stringValue);

			return Enum.ToObject(enumType, 0);
		}

		public static object ParseEnum(Type enumType, string value)
		{
			if(value == null) throw new ArgumentNullException(nameof(value));

			string[] names = Enum.GetNames(enumType);
			Array values = Enum.GetValues(enumType);

			for(int index = 0; index < Enum.GetNames(enumType).Length; index++)
			{
				string name = names[index];

				if(string.Equals(name, value, StringComparison.OrdinalIgnoreCase))
				{
					return values.GetValue(index);
				}

				EnumMemberAttribute enumMemberAttribute = enumType.GetField(name).GetCustomAttribute<EnumMemberAttribute>();

				if(enumMemberAttribute is not null && string.Equals(value, enumMemberAttribute.Value,
					StringComparison.OrdinalIgnoreCase))
				{
					return values.GetValue(index);
				}
			}

			throw new InvalidOperationException($"Invalid value '{values}' for enum type {enumType.Name}.");
		}

		public static Expression CreateCastExpression(Expression argument, Type targetType)
		{
			if(targetType == TypeUtilities.StringType)
			{
				return Expression.Call(argument, Methods.ObjectToString);
			}

			return Expression.Convert(argument, targetType);
		}

		private static Expression ConvertToDateOnlyExpression(Expression argument, bool isNullable)
		{
			MethodCallExpression fromDateTimeExpression = Expression.Call(
				argument,
				Methods.DateOnlyFromDateTime
			);

			if(isNullable)
			{
				return ConvertExpression(
					fromDateTimeExpression,
					false,
					TypeUtilities.DateOnlyType,
					typeof(DateOnly?)
				);
			}

			return fromDateTimeExpression;
		}
	}
}
