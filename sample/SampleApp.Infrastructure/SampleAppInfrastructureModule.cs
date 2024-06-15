namespace SampleApp.Infrastructure
{
	using Fluxera.Extensions.Common;
	using Fluxera.Extensions.Hosting;
	using Fluxera.Extensions.Hosting.Modules;
	using Fluxera.Extensions.Hosting.Modules.DataManagement;
	using Fluxera.Extensions.Hosting.Modules.Infrastructure;
	using Fluxera.Extensions.Hosting.Modules.Persistence;
	using Fluxera.Extensions.Hosting.Modules.Persistence.MongoDB;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using SampleApp.Domain.Customers;
	using SampleApp.Infrastructure.Contributors;
	using SampleApp.Infrastructure.Customers;

	/// <summary>
	///		The infrastructure module.
	/// </summary>
	[PublicAPI]
	[DependsOn<MongoPersistenceModule>]
	[DependsOn<DataManagementModule>]
	[DependsOn<InfrastructureModule>]
	public sealed class SampleAppInfrastructureModule : ConfigureServicesModule
	{
		/// <inheritdoc />
		public override void ConfigureServices(IServiceConfigurationContext context)
		{
			// Add the repository contributor for the 'Default' repository.
			context.Services.AddRepositoryContributor<RepositoryContributor>();

			// Add the data seeding contributor.
			context.Services.AddDataSeedingContributor<DataSeedingContributor>();

			// Add repositories.
			context.Log("AddRepositories", services =>
			{
				services.AddTransient<ICustomerRepository, CustomerRepository>();
			});

			// Add infrastructure services.
			context.Log("AddDateTimeOffsetProvider", 
				services => services.AddDateTimeOffsetProvider());
		}
	}
}
