namespace Fluxera.Queries.Nodes
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Diagnostics;
	using System.Linq;
	using Fluxera.Queries.Model;
	using JetBrains.Annotations;

	/// <summary>
	///     A <see cref="QueryNode" /> which represents an array of constant values.
	/// </summary>
	[PublicAPI]
	[DebuggerDisplay("{LiteralText}")]
	public sealed class ArrayNode : ValueNode
	{
		private readonly List<ConstantNode> elements;

		/// <summary>
		///     Initializes a new instance of the <see cref="ArrayNode" /> type.
		/// </summary>
		/// <param name="elements">The array elements.</param>
		/// <exception cref="InvalidOperationException"></exception>
		public ArrayNode(IEnumerable<ConstantNode> elements)
		{
			this.elements = elements.ToList();

			if(this.elements.Select(t => t.EdmType).Distinct().Count() > 1)
			{
				throw new InvalidOperationException("Unable to create an array node of distinct constant values.");
			}
		}

		/// <summary>
		///     Gets the array node elements.
		/// </summary>
		public IReadOnlyList<ConstantNode> Elements => new ReadOnlyCollection<ConstantNode>(this.elements);

		/// <summary>
		///     Gets the kind of query node.
		/// </summary>
		public override QueryNodeKind Kind => QueryNodeKind.Array;

		/// <inheritdoc />
		public override EdmType EdmValueType => this.elements[0].EdmValueType;

		/// <summary>
		///     Gets the CRL type of the array.
		/// </summary>
		public Type ArrayClrType => this.EdmValueType.ClrType.MakeArrayType();

		/// <summary>
		///     Gets the literal text.
		/// </summary>
		public string LiteralText => $"({string.Join(",", this.elements.Select(e => e.LiteralText))})";

		/// <summary>
		///     Gets the constant value as an object.
		/// </summary>
		public object Value
		{
			get
			{
				Array instance = Array.CreateInstance(this.EdmValueType.ClrType, this.elements.Count);
				for(int i = 0; i < this.elements.Count; i++)
				{
					ConstantNode element = this.elements[i];
					instance.SetValue(element.Value, i);
				}

				return instance;
			}
		}

		/// <summary>
		///     Adds an element to the array node.
		/// </summary>
		/// <param name="element"></param>
		/// <exception cref="InvalidOperationException"></exception>
		public void AddElement(ConstantNode element)
		{
			if(element.EdmType != this.EdmValueType)
			{
				throw new InvalidOperationException(
					$"Unable to add element of type {element.EdmValueType} to array of type {this.EdmValueType.Name}");
			}

			this.elements.Add(element);
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return this.LiteralText;
		}
	}
}
