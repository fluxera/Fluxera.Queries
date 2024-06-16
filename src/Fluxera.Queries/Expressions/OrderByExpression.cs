namespace Fluxera.Queries.Expressions
{
	using System;
	using System.Linq.Expressions;
	using Fluxera.Queries.Options;
	using JetBrains.Annotations;

	/// <summary>
	///		A container for holding the property expression and the order direction.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public readonly struct OrderByExpression<T>
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="OrderByExpression{T}"/> type.
		/// </summary>
		/// <param name="selectorExpression"></param>
		/// <param name="direction"></param>
		public OrderByExpression(Expression<Func<T, object>> selectorExpression,
			OrderByDirection direction)
		{
			this.SelectorExpression = selectorExpression;
			this.Direction = direction;
		}

		/// <summary>
		///		Gets the property selector expression.
		/// </summary>
		public Expression<Func<T, object>> SelectorExpression { get; }

		/// <summary>
		///		Gets the order direction.
		/// </summary>
		public OrderByDirection Direction { get; }
	}
}
