namespace SampleApp
{
	using System.Reflection;
	using System.Threading.Tasks;
	using Fluxera.Queries.AspNetCore;
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
				options.EntitySet<CustomerDto>("Customers");
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

			//using(IServiceScope serviceScope = app.Services.CreateScope())
			//{
			//	IRepository<Customer, CustomerId> repository = serviceScope.ServiceProvider.GetRequiredService<IRepository<Customer, CustomerId>>();
			//	IUnitOfWorkFactory unitOfWorkFactory = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWorkFactory>();
			//	IUnitOfWork unitOfWork = unitOfWorkFactory.CreateUnitOfWork(repository.RepositoryName);

			//	if(await repository.CountAsync() > 0)
			//	{
			//		return;
			//	}

			//	Faker<Customer> faker = new Faker<Customer>()
			//		.UseSeed(37)
			//		.CustomInstantiator(x =>
			//		{
			//			string firstName = x.Name.FirstName();
			//			string lastName = x.Name.LastName();
			//			string email = x.Internet.Email(firstName, lastName);

			//			DateTime today = DateTime.Today;
			//			DateTime dateOfBirth = x.Person.DateOfBirth;
			//			int age = today.Year - dateOfBirth.Year;

			//			CustomerState state = Random.Shared.Next(0, 10).IsEven() ? CustomerState.New : CustomerState.Legacy;

			//			return new Customer
			//			{
			//				FirstName = firstName,
			//				LastName = lastName,
			//				Email = email,
			//				Age = age,
			//				State = state
			//			};
			//		});

			//	int counter = 0;
			//	foreach(Customer customer in faker.GenerateForever())
			//	{
			//		counter++;

			//		await repository.AddAsync(customer);

			//		if(counter == 100)
			//		{
			//			break;
			//		}
			//	}

			//	await unitOfWork.SaveChangesAsync();
			//}

			await app.RunAsync();
		}
	}
}
