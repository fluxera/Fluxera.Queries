namespace SampleApp.HttpApi
{
	using Fluxera.Extensions.Hosting;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Modules.AspNetCore.HttpApi;
	using Fluxera.Queries.AspNetCore;
	using JetBrains.Annotations;
	using MadEyeMatt.AspNetCore.Endpoints;
	using Microsoft.Extensions.DependencyInjection;
	using SampleApp.Application.Contracts.Customers;

	/// <summary>
	///		The module for the HTTP API.
	/// </summary>
	[PublicAPI]
	[DependsOn<HttpApiModule>]
	public sealed class SampleAppHttpApiModule : ConfigureApplicationModule
	{
		/// <inheritdoc />
		public override void ConfigureServices(IServiceConfigurationContext context)
		{
			context.Services.Configure<EndpointsOptions>(options =>
			{
				options.EndpointsRoutePrefix = "api";
			});

			context.Services.AddDataQueries(options =>
			{
				options.EntitySet<CustomerDto>("Customers", "Customer", entityType =>
				{
					entityType.HasKey(x => x.ID)
						.Ignore(x => x.IgnoreMe);
				});

				options.ComplexType<AddressDto>("Address", complexType =>
				{
					complexType.Ignore(x => x.IgnoreMe);
				});

				options.ComplexType<CountryDto>("Country");
			});
		}

		/// <inheritdoc />
		public override void PreConfigure(IApplicationInitializationContext context)
		{
			context.UseProblemDetails();
		}
	}
}
