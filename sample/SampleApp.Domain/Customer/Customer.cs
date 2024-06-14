namespace SampleApp.Domain.Customer
{
	using Fluxera.Entity;
	using JetBrains.Annotations;
	using SampleApp.Domain.Shared.Customer;

	[PublicAPI]
	public sealed class Customer : AggregateRoot<Customer, CustomerId>
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public Age Age { get; set; }

		public CustomerState State { get; set; }

		public Address Address { get; set; }

		public double IgnoreMe { get; set; }
	}
}
