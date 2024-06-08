namespace Fluxera.Queries.AspNetCore.ParameterBinding
{
	using JetBrains.Annotations;

	/// <summary>
	///		A contract for custom parameter value parsers.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public interface IValueParser<T> where T : IValueParser<T>
	{
		/// <summary>
		///		Parses the value from the given parameter string value.
		/// </summary>
		/// <param name="parameter"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		static abstract bool TryParse(string parameter, out T value);
	}
}
