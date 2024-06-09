namespace Fluxera.Queries
{
	using JetBrains.Annotations;

	/// <summary>
	///     A class that represents a single result of a query.
	/// </summary>
	[PublicAPI]
	public sealed class SingleResult
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="SingleResult"/>.
		/// </summary>
		/// <param name="item"></param>
		public SingleResult(object item)
		{
			this.Item = item;
		}

		/// <summary>
		///		Gets the item of the result.
		/// </summary>
		public object Item { get; }

		/// <summary>
		///		Flag, indicating if the result has a value.
		/// </summary>
		public bool HasValue => this.Item is not null;
	}
}
