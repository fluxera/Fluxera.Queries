namespace Fluxera.Queries.Expressions
{
	using System;
	using System.Diagnostics;
	using Fluxera.Queries.Model;
	using JetBrains.Annotations;

	/// <summary>
	///     A <see cref="QueryNode" /> which represents a constant value.
	/// </summary>
	[PublicAPI]
	[DebuggerDisplay("{LiteralText}")]
	public sealed class ConstantNode : ValueNode
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="ConstantNode" /> type.
		/// </summary>
		/// <param name="edmType">The <see cref="EdmType" /> of the value.</param>
		/// <param name="literalText">The literal text.</param>
		/// <param name="value">The value.</param>
		public ConstantNode(EdmType edmType, string literalText, object value)
		{
			this.EdmType = edmType;
			this.LiteralText = literalText;
			this.Value = value;
		}

		/// <summary>
		///     Gets the <see cref="EdmType" /> of the value.
		/// </summary>
		public EdmType EdmType { get; }

		/// <summary>
		///     Gets the <see cref="EdmType" /> of the value.
		/// </summary>
		public override EdmType EdmValueType => this.EdmType;

		/// <summary>
		///     Gets the kind of query node.
		/// </summary>
		public override QueryNodeKind Kind => QueryNodeKind.Constant;

		/// <summary>
		///     Gets the literal text if the constant value.
		/// </summary>
		public string LiteralText { get; }

		/// <summary>
		///     Gets the constant value as an object.
		/// </summary>
		public object Value { get; }

		/// <summary>
		///     Gets the <see cref="ConstantNode"/> which represents the  value <c>true</c>.
		/// </summary>
		public static ConstantNode True { get; } = new ConstantNode(EdmPrimitiveType.Boolean, "true", true);

		/// <summary>
		///     Gets the <see cref="ConstantNode"/> which represents the value <c>false</c>.
		/// </summary>
		public static ConstantNode False { get; } = new ConstantNode(EdmPrimitiveType.Boolean, "false", false);

		/// <summary>
		///     Gets the <see cref="ConstantNode"/> which represents the 32-bit integer value <c>0</c>.
		/// </summary>
		public static ConstantNode Int32Zero { get; } = new ConstantNode(EdmPrimitiveType.Int32, "0", 0);

		/// <summary>
		///     Gets the <see cref="ConstantNode"/> which represents the 64-bit integer value <c>0</c>.
		/// </summary>
		public static ConstantNode Int64Zero { get; } = new ConstantNode(EdmPrimitiveType.Int64, "0L", 0L);

		/// <summary>
		///     Gets the <see cref="ConstantNode"/> which represents the value <c>null</c>.
		/// </summary>
		public static ConstantNode Null { get; } = new ConstantNode(null, "null", null);

		/// <summary>
		///     Gets a <see cref="ConstantNode"/> which represents a <see cref="DateOnly"/> value.
		/// </summary>
		/// <param name="literalText">The literal text.</param>
		/// <param name="value">The value.</param>
		/// <returns>A <see cref="ConstantNode"/> representing a <see cref="DateOnly"/> value.</returns>
		public static ConstantNode Date(string literalText, DateOnly value)
		{
			return new ConstantNode(EdmPrimitiveType.Date, literalText, value);
		}

		/// <summary>
		///     Gets a <see cref="ConstantNode"/> which represents a <see cref="System.DateTimeOffset"/> value.
		/// </summary>
		/// <param name="literalText">The literal text.</param>
		/// <param name="value">The value.</param>
		/// <returns>A <see cref="ConstantNode"/> representing a <see cref="System.DateTimeOffset"/> value.</returns>
		public static ConstantNode DateTimeOffset(string literalText, DateTimeOffset value)
		{
			return new ConstantNode(EdmPrimitiveType.DateTimeOffset, literalText, value);
		}

		/// <summary>
		///     Gets a <see cref="ConstantNode"/> which represents a <see cref="TimeSpan"/> value.
		/// </summary>
		/// <param name="literalText">The literal text.</param>
		/// <param name="value">The value.</param>
		/// <returns>A <see cref="ConstantNode"/> representing a <see cref="TimeSpan"/> value.</returns>
		public static ConstantNode Duration(string literalText, TimeSpan value)
		{
			return new ConstantNode(EdmPrimitiveType.Duration, literalText, value);
		}

		/// <summary>
		///     Gets a <see cref="ConstantNode"/> which represents a <see cref="System.Guid"/> value.
		/// </summary>
		/// <param name="literalText">The literal text.</param>
		/// <param name="value">The value.</param>
		/// <returns>A <see cref="ConstantNode"/> representing a <see cref="System.Guid"/> value.</returns>
		public static ConstantNode Guid(string literalText, Guid value)
		{
			return new ConstantNode(EdmPrimitiveType.Guid, literalText, value);
		}

		/// <summary>
		///     Gets a <see cref="ConstantNode"/> which represents a <see cref="int"/> value.
		/// </summary>
		/// <param name="literalText">The literal text.</param>
		/// <param name="value">The value.</param>
		/// <returns>A <see cref="ConstantNode"/> representing a <see cref="int"/> value.</returns>
		public static ConstantNode Int32(string literalText, int value)
		{
			return new ConstantNode(EdmPrimitiveType.Int32, literalText, value);
		}

		/// <summary>
		///     Gets a <see cref="ConstantNode"/> which represents a <see cref="long"/> value.
		/// </summary>
		/// <param name="literalText">The literal text.</param>
		/// <param name="value">The value.</param>
		/// <returns>A <see cref="ConstantNode"/> representing a <see cref="long"/> value.</returns>
		public static ConstantNode Int64(string literalText, long value)
		{
			return new ConstantNode(EdmPrimitiveType.Int64, literalText, value);
		}

		/// <summary>
		///     Gets a <see cref="ConstantNode"/> which represents a <see cref="float"/> value.
		/// </summary>
		/// <param name="literalText">The literal text.</param>
		/// <param name="value">The value.</param>
		/// <returns>A <see cref="ConstantNode"/> representing a <see cref="float"/> value.</returns>
		public static ConstantNode Single(string literalText, float value)
		{
			return new ConstantNode(EdmPrimitiveType.Single, literalText, value);
		}

		/// <summary>
		///     Gets a <see cref="ConstantNode"/> which represents a <see cref="decimal"/> value.
		/// </summary>
		/// <param name="literalText">The literal text.</param>
		/// <param name="value">The value.</param>
		/// <returns>A <see cref="ConstantNode"/> representing a <see cref="decimal"/> value.</returns>
		public static ConstantNode Decimal(string literalText, decimal value)
		{
			return new ConstantNode(EdmPrimitiveType.Decimal, literalText, value);
		}

		/// <summary>
		///     Gets a <see cref="ConstantNode"/> which represents a <see cref="double"/> value.
		/// </summary>
		/// <param name="literalText">The literal text.</param>
		/// <param name="value">The value.</param>
		/// <returns>A <see cref="ConstantNode"/> representing a <see cref="double"/> value.</returns>
		public static ConstantNode Double(string literalText, double value)
		{
			return new ConstantNode(EdmPrimitiveType.Double, literalText, value);
		}

		/// <summary>
		///     Gets a <see cref="ConstantNode"/> which represents a <see cref="string"/> value.
		/// </summary>
		/// <param name="literalText">The literal text.</param>
		/// <param name="value">The value.</param>
		/// <returns>A <see cref="ConstantNode"/> representing a <see cref="string"/> value.</returns>
		public static ConstantNode String(string literalText, string value)
		{
			return new ConstantNode(EdmPrimitiveType.String, literalText, value);
		}

		/// <summary>
		///     Gets a <see cref="ConstantNode"/> which represents a <see cref="TimeSpan"/> value.
		/// </summary>
		/// <param name="literalText">The literal text.</param>
		/// <param name="value">The value.</param>
		/// <returns>A <see cref="ConstantNode"/> representing a <see cref="TimeSpan"/> value.</returns>
		public static ConstantNode Time(string literalText, TimeSpan value)
		{
			return new ConstantNode(EdmPrimitiveType.TimeOfDay, literalText, value);
		}

		/// <summary>
		///     Gets a <see cref="ConstantNode"/> which represents an <see cref="Array"/> value.
		/// </summary>
		/// <param name="elements"></param>
		/// <returns>A <see cref="ConstantNode"/> representing an <see cref="Array"/> value.</returns>
		public static ArrayNode Array(params ConstantNode[] elements)
		{
			return new ArrayNode(elements);
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return this.LiteralText;
		}
	}
}
