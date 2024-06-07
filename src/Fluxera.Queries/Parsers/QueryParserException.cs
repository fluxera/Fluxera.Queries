namespace Fluxera.Queries.Parsers
{
	using System;
	using JetBrains.Annotations;

	/// <summary>
	///     An exception which is thrown when a request is not parsed correctly.
	/// </summary>
	[PublicAPI]
	[Serializable]
	public sealed class QueryParserException : Exception
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="QueryParserException" /> type.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public QueryParserException(string message) : base(message)
		{
		}
	}
}
