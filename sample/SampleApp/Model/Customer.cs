namespace SampleApp.Model
{
	using Fluxera.Entity;

	public sealed class Customer : AggregateRoot<Customer, CustomerId>
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public int Age { get; set; }

		public CustomerState State { get; set; }
	}
}
