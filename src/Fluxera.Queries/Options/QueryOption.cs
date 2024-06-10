namespace Fluxera.Queries.Options
{
	using System;
	using Fluxera.Guards;
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
		/// <param name="expression">The expression in string form.</param>
		/// <exception cref="ArgumentNullException"></exception>
		protected QueryOption(string expression)
		{
			this.StringExpression = Guard.Against.Null(expression);
		}

		/// <summary>
		///		Gets the parameter string of the request.
		/// </summary>
		public string StringExpression { get; set; }
	}
}
