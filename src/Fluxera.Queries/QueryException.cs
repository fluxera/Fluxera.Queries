namespace Fluxera.Queries
{
	using System;
	using JetBrains.Annotations;

	/// <summary>
	///     An exception which is thrown when a request is not parsed correctly.
	/// </summary>
	[PublicAPI]
	[Serializable]
	public sealed class QueryException : Exception
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="QueryException" /> type.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public QueryException(string message) : base(message)
		{
		}
	}
}
