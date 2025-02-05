namespace Fluxera.Queries
{
	using Fluxera.Queries.Model;
	using Fluxera.Queries.Options;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.DependencyInjection.Extensions;
	using Microsoft.Extensions.Options;
	using System;

	/// <summary>
	///		Extensions methods for the <see cref="IServiceCollection"/> type.
	/// </summary>
	[PublicAPI]
	public static class ServiceCollectionExtensions
	{
		///  <summary>
		/// 		Adds the query parser services.
		///  </summary>
		///  <param name="services"></param>
		///  <param name="configure"></param>
		///  <returns></returns>
		public static IServiceCollection AddDataQueries(this IServiceCollection services, Action<IDataQueriesOptionsBuilder> configure)
		{
			services.TryAddTransient<IQueryParser, QueryParser>();
			services.TryAddSingleton<IEdmTypeProvider, EdmTypeProvider>();
			services.TryAddTransient<IQueryExecutorFactory, DefaultQueryExecutorFactory>();
			services.TryAddTransient<ISkipTokenHandler, DefaultSkipTokenHandler>();

			services.Configure<DataQueriesOptions>(options =>
			{
				DataQueriesOptionsBuilder builder = new DataQueriesOptionsBuilder(options);
				configure?.Invoke(builder);
			});

			services.AddTransient<IPostConfigureOptions<DataQueriesOptions>, ConfigureDataQueriesOptions>();

			return services;
		}
	}
}
