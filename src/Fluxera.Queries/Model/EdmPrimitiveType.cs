namespace Fluxera.Queries.Model
{
	using System;
	using System.Diagnostics;
	using JetBrains.Annotations;

	/// <summary>
	///     The EDM representation of a primitive type.
	/// </summary>
	/// <seealso cref="EdmType" />
	[PublicAPI]
	[DebuggerDisplay("{FullName}: {ClrType}")]
	public sealed class EdmPrimitiveType : EdmType
	{
		/// <inheritdoc />
		private EdmPrimitiveType(string name, string fullName, Type clrType)
			: base(name, fullName, clrType)
		{
		}

		/// <summary>
		///     Gets the <see cref="EdmType"/> which represents binary data.
		/// </summary>
		public static EdmType Binary { get; } = new EdmPrimitiveType("Binary", "Edm.Binary", typeof(byte[]));

		/// <summary>
		///     Gets the <see cref="EdmType"/> which represents a boolean.
		/// </summary>
		public static EdmType Boolean { get; } = new EdmPrimitiveType("Boolean", "Edm.Boolean", typeof(bool));

		/// <summary>
		///     Gets the <see cref="EdmType"/> which represents an unsigned 8-bit integer value.
		/// </summary>
		public static EdmType Byte { get; } = new EdmPrimitiveType("Byte", "Edm.Byte", typeof(byte));

		/// <summary>
		///     Gets the <see cref="EdmType"/> which represents numeric values with fixed precision and scale.
		/// </summary>
		public static EdmType Decimal { get; } = new EdmPrimitiveType("Decimal", "Edm.Decimal", typeof(decimal));

		/// <summary>
		///     Gets the <see cref="EdmType"/> which represents a unique identifier.
		/// </summary>
		public static EdmType Guid { get; } = new EdmPrimitiveType("Guid", "Edm.Guid", typeof(Guid));

		/// <summary>
		///     Gets the <see cref="EdmType"/> which represents a signed 16-bit integer value.
		/// </summary>
		public static EdmType Int16 { get; } = new EdmPrimitiveType("Int16", "Edm.Int16", typeof(short));

		/// <summary>
		///     Gets the <see cref="EdmType"/> which represents a signed 32-bit integer value.
		/// </summary>
		public static EdmType Int32 { get; } = new EdmPrimitiveType("Int32", "Edm.Int32", typeof(int));

		/// <summary>
		///     Gets the <see cref="EdmType"/> which represents a signed 64-bit integer value.
		/// </summary>
		public static EdmType Int64 { get; } = new EdmPrimitiveType("Int64", "Edm.Int64", typeof(long));

		/// <summary>
		///     Gets the <see cref="EdmType"/> which represents a signed 8-bit integer value.
		/// </summary>
		public static EdmType SByte { get; } = new EdmPrimitiveType("SByte", "Edm.SByte", typeof(sbyte));

		/// <summary>
		///     Gets the <see cref="EdmType"/> which represents a single floating point number.
		/// </summary>
		public static EdmType Single { get; } = new EdmPrimitiveType("Single", "Edm.Single", typeof(float));

		/// <summary>
		///     Gets the <see cref="EdmType"/> which represents a double floating point number.
		/// </summary>
		public static EdmType Double { get; } = new EdmPrimitiveType("Double", "Edm.Double", typeof(double));

		/// <summary>
		///     Gets the <see cref="EdmType"/> which represents a string.
		/// </summary>
		public static EdmType String { get; } = new EdmPrimitiveType("String", "Edm.String", typeof(string));

		/// <summary>
		///     Gets the <see cref="EdmType"/> which represents the time of day.
		/// </summary>
		public static EdmType TimeOfDay { get; } = new EdmPrimitiveType("TimeOfDay", "Edm.TimeOfDay", typeof(TimeOnly));

		/// <summary>
		///     Gets the <see cref="EdmType"/> which represents date values.
		/// </summary>
		public static EdmType Date { get; } = new EdmPrimitiveType("Date", "Edm.Date", typeof(DateOnly));

		/// <summary>
		///     Gets the <see cref="EdmType"/> which represents date and time value with an offset in minutes from GMT.
		/// </summary>
		public static EdmType DateTimeOffset { get; } = new EdmPrimitiveType("DateTimeOffset", "Edm.DateTimeOffset", typeof(DateTimeOffset));

		/// <summary>
		///     Gets the <see cref="EdmType"/> which represents a timespan.
		/// </summary>
		public static EdmType Duration { get; } = new EdmPrimitiveType("Duration", "Edm.Duration", typeof(TimeSpan));

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}
	}
}
