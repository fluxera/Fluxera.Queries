namespace SampleApp
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using Bogus;
	using Fluxera.Repository;
	using Fluxera.Utilities.Extensions;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.Extensions.DependencyInjection;
	using SampleApp.Model;

	internal static class DataSeeder
	{
		public static async Task SeedData(this WebApplication app)
		{
			using(IServiceScope serviceScope = app.Services.CreateScope())
			{
				IRepository<CustomerDto, CustomerId> repository = serviceScope.ServiceProvider.GetRequiredService<IRepository<CustomerDto, CustomerId>>();
				IUnitOfWorkFactory unitOfWorkFactory = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWorkFactory>();
				IUnitOfWork unitOfWork = unitOfWorkFactory.CreateUnitOfWork(repository.RepositoryName);

				if(await repository.CountAsync() > 0)
				{
					return;
				}

				Faker<CountryDto> countryFaker = new Faker<CountryDto>()
					.UseSeed(37)
					.CustomInstantiator(x =>
					{
						string code = x.Address.CountryCode();
						string name = x.Address.Country();

						return new CountryDto
						{
							Code = code,
							Name = name,
						};
					});

				Faker<AddressDto> addressFaker = new Faker<AddressDto>()
					.UseSeed(37)
					.CustomInstantiator(x =>
					{
						string street = x.Address.StreetName();
						string number = x.Address.BuildingNumber();
						string city = x.Address.City();
						string zipCode = x.Address.ZipCode("#####");

						return new AddressDto
						{
							Street = street,
							Number = number,
							City = city,
							ZipCode = new ZipCode(zipCode),
							Country = countryFaker.Generate(1).First()
						};
					});

				Faker<CustomerDto> customerFaker = new Faker<CustomerDto>()
				   .UseSeed(37)
				   .CustomInstantiator(x =>
				   {
					   string firstName = x.Name.FirstName();
					   string lastName = x.Name.LastName();
					   string email = x.Internet.Email(firstName, lastName);

					   DateTime today = DateTime.Today;
					   DateTime dateOfBirth = x.Person.DateOfBirth;
					   int age = today.Year - dateOfBirth.Year;

					   CustomerState state = Random.Shared.Next(0, 10).IsEven() ? CustomerState.New : CustomerState.Legacy;

					   return new CustomerDto
					   {
						   FirstName = firstName,
						   LastName = lastName,
						   Email = email,
						   Age = new Age(age),
						   State = state,
						   Address = addressFaker.Generate(1).First()
					   };
				   });

				int counter = 0;
				foreach(CustomerDto customer in customerFaker.GenerateForever())
				{
					counter++;

					await repository.AddAsync(customer);

					if(counter == 100)
					{
						break;
					}
				}

				await unitOfWork.SaveChangesAsync();
			}
		}
	}
}
