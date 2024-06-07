namespace Fluxera.Queries
{
	using Fluxera.Queries.Model;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///		Extensions methods for the <see cref="IServiceCollection"/> type.
	/// </summary>
	[PublicAPI]
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		///		Adds the query parser services.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddQueryParser(this IServiceCollection services)
		{
			services.AddSingleton<IQueryParser, QueryParser>();
			services.AddSingleton<EdmTypeProvider>();
			return services;
		}
	}
}
