namespace SampleApp
{
	using System.Reflection;
	using System.Threading.Tasks;
	using Fluxera.Queries.AspNetCore;
	using Fluxera.Queries.AspNetCore.Options;
	using Fluxera.Queries.Repository;
	using Fluxera.Repository;
	using Fluxera.Repository.MongoDB;
	using MadEyeMatt.MongoDB.DbContext;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using SampleApp.Model;

	public static class Program
	{
		public static async Task Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddMediatR(options =>
			{
				options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
			});

			builder.Services.AddMongoDbContext<SampleMongoContext>();

			builder.Services.AddRepository(repositoryBuilder =>
			{
				repositoryBuilder.AddMongoRepository<SampleRepositoryContext>(repositoryOptionsBuilder =>
				{
					repositoryOptionsBuilder.UseFor<CustomerDto>();

					repositoryOptionsBuilder.EnableUnitOfWork();
				});
			});

			builder.Services.AddDataQueries(options =>
			{
				IEntitySetOptionsBuilder entitySet = options.EntitySet<CustomerDto>("Customers", "Customer", entityType =>
				{
					entityType.HasKey(x => x.ID);
					entityType.Ignore(x => x.IgnoreMe);
				});
				entitySet.AlwaysIncludeCount();

				options.ComplexType<AddressDto>("Address", complexType =>
				{
					complexType.Ignore(x => x.IgnoreMe);
				});

				options.ComplexType<CountryDto>("Country");
			});

			builder.Services.AddRepositoryQueryExecutor();

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			WebApplication app = builder.Build();

			// Configure the HTTP request pipeline.
			if(app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.MapDataQueriesEndpoints();

			await app.SeedData();

			await app.RunAsync();
		}
	}
}
