namespace Fluxera.Queries.AspNetCore.ParameterBinding
{
	using System.Reflection;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Microsoft.AspNetCore.Http;

	/// <summary>
	///		A contract for custom parameter binders.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public interface IParameterBinder<T> where T : IParameterBinder<T>
	{
		/// <summary>
		///		The method discovered by RequestDelegateFactory on types used as parameters of route
		///		handler delegates to support custom binding.
		/// </summary>
		/// <param name="context">The <see cref="HttpContext"/>.</param>
		/// <param name="parameter">The <see cref="ParameterInfo"/> for the parameter being bound to.</param>
		/// <returns>The value to assign to the parameter.</returns>
		static abstract ValueTask<T> BindAsync(HttpContext context, ParameterInfo parameter);
	}
}
