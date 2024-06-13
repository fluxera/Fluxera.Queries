namespace SampleApp
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
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
				options.EntitySet<CustomerDto>("Customers", "Customer", entityType =>
				{
					// TODO: Required()
					entityType.HasKey(x => x.ID)
							  .Ignore(x => x.IgnoreMe);
				});

				options.ComplexType<AddressDto>("Address", complexType =>
				{
					complexType.Ignore(x => x.IgnoreMe);
				});
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

			using(IServiceScope serviceScope = app.Services.CreateScope())
			{
				IRepository<CustomerDto, CustomerId> repository = serviceScope.ServiceProvider.GetRequiredService<IRepository<CustomerDto, CustomerId>>();

				Expression<Func<CustomerDto, CustomerDto>> expression = 
					x => new CustomerDto 
					{ 
						FirstName = x.FirstName, 
						Address = new AddressDto
						{
							City = x.Address.City
						}
					};

				CustomerDto dtos = await repository.FindOneAsync(x => true, expression);
			}

			//using(IServiceScope serviceScope = app.Services.CreateScope())
				//{
				//	IRepository<CustomerDto, CustomerId> repository = serviceScope.ServiceProvider.GetRequiredService<IRepository<CustomerDto, CustomerId>>();
				//	IUnitOfWorkFactory unitOfWorkFactory = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWorkFactory>();
				//	IUnitOfWork unitOfWork = unitOfWorkFactory.CreateUnitOfWork(repository.RepositoryName);

				//	if(await repository.CountAsync() > 0)
				//	{
				//		return;
				//	}

				//	Faker<AddressDto> addressFaker = new Faker<AddressDto>()
				//		.UseSeed(37)
				//		.CustomInstantiator(x =>
				//		{
				//			string street = x.Address.StreetName();
				//			string number = x.Address.BuildingNumber();
				//			string city = x.Address.City();
				//			string zipCode = x.Address.ZipCode("#####");

				//			return new AddressDto
				//			{
				//				Street = street,
				//				Number = number,
				//				City = city,
				//				ZipCode = new ZipCode(zipCode)
				//			};
				//		});

				//	Faker<CustomerDto> customerFaker = new Faker<CustomerDto>()
				//	   .UseSeed(37)
				//	   .CustomInstantiator(x =>
				//	   {
				//		   string firstName = x.Name.FirstName();
				//		   string lastName = x.Name.LastName();
				//		   string email = x.Internet.Email(firstName, lastName);

				//		   DateTime today = DateTime.Today;
				//		   DateTime dateOfBirth = x.Person.DateOfBirth;
				//		   int age = today.Year - dateOfBirth.Year;

				//		   CustomerState state = Random.Shared.Next(0, 10).IsEven() ? CustomerState.New : CustomerState.Legacy;

				//		   return new CustomerDto
				//		   {
				//			   FirstName = firstName,
				//			   LastName = lastName,
				//			   Email = email,
				//			   Age = new Age(age),
				//			   State = state,
				//			   Address = addressFaker.Generate(1).First()
				//		   };
				//	   });

				//	int counter = 0;
				//	foreach(CustomerDto customer in customerFaker.GenerateForever())
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
