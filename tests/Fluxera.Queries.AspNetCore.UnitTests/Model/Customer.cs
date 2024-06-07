namespace Fluxera.Queries.AspNetCore.UnitTests.Model
{
	using Fluxera.Entity;

	public class Customer : AggregateRoot<Customer, CustomerId>
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public Age Age { get; set; }

		public CustomerState State { get; set; }
	}
}
