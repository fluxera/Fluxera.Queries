namespace Fluxera.Queries.AspNetCore
{
	using Fluxera.Queries.AspNetCore.ModelBinding;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.AspNetCore.Mvc;

	/// <summary>
	///		Extensions methods for the <see cref="IServiceCollection"/> type.
	/// </summary>
	[PublicAPI]
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		///		Adds the data queries services.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddDataQueries(this IServiceCollection services)
		{
			services.AddQueryParser();

			services.PostConfigure<MvcOptions>(o =>
			{
				o.ModelBinderProviders.Insert(0, new DataQueryModelBinderProvider());
			});

			return services;
		}
	}
}
