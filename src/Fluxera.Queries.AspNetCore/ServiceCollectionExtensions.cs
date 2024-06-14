namespace Fluxera.Queries.AspNetCore
{
	using System;
	using Fluxera.Queries.AspNetCore.ModelBinding;
	using Fluxera.Queries.AspNetCore.Options;
	using Fluxera.Queries.AspNetCore.Swagger;
	using JetBrains.Annotations;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Options;
	using Swashbuckle.AspNetCore.SwaggerGen;

	/// <summary>
	///		Extensions methods for the <see cref="IServiceCollection"/> type.
	/// </summary>
	[PublicAPI]
	public static class ServiceCollectionExtensions
	{
		///  <summary>
		/// 		Adds the data queries services.
		///  </summary>
		///  <param name="services"></param>
		///  <param name="configure"></param>
		///  <returns></returns>
		public static IServiceCollection AddDataQueries(this IServiceCollection services, Action<IDataQueriesOptionsBuilder> configure)
		{
			services.AddQueryParser();

			services.PostConfigure<MvcOptions>(options =>
			{
				options.ModelBinderProviders.Insert(0, new DataQueryModelBinderProvider());
			});

			services.Configure<DataQueriesOptions>(options =>
			{
				DataQueriesOptionsBuilder builder = new DataQueriesOptionsBuilder(options);
				configure?.Invoke(builder);
			});

			services.AddTransient<IPostConfigureOptions<DataQueriesOptions>, ConfigureDataQueriesOptions>();

			services.AddTransient<IPostConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();

			return services;
		}
	}
}
