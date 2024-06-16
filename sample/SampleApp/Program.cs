
namespace SimpleApp
{
	using System.Reflection;
	using System.Threading.Tasks;
	using Fluxera.Queries;
	using Fluxera.Queries.AspNetCore;
	using Fluxera.Repository;
	using Fluxera.Repository.MongoDB;
	using MadEyeMatt.MongoDB.DbContext;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using SimpleApp.Contexts;
	using SimpleApp.Model;

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
					repositoryOptionsBuilder.UseFor<Customer>();

					repositoryOptionsBuilder.EnableUnitOfWork();
				});
			});

			builder.Services.AddDataQueries(options =>
			{
				options.EntitySet<Customer>("Customers", entityType =>
				{
					// TODO: Required()
					entityType.HasKey(x => x.ID)
							  .Ignore(x => x.IgnoreMe);
				});

				options.ComplexType<Address>(complexType =>
				{
					complexType.Ignore(x => x.IgnoreMe);
				});

				options.ComplexType<Country>();
			});

			builder.Services.AddDataQueriesSwagger();

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
