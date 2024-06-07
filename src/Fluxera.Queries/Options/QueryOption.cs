namespace Fluxera.Queries.Options
{
	using System;
	using JetBrains.Annotations;

	/// <summary>
	///		A base class for the query option classes.
	/// </summary>
	[PublicAPI]
	public abstract class QueryOption
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="QueryOption"/> type.
		/// </summary>
		/// <param name="stringExpression">The expression in string form.</param>
		/// <exception cref="ArgumentNullException"></exception>
		protected QueryOption(string stringExpression)
		{
			this.StringExpression = Guard.ThrowIfNull(stringExpression);
		}

		/// <summary>
		///		Gets the parameter string of the request.
		/// </summary>
		public string StringExpression { get; set; }
	}
}
