namespace SampleApp.Model
{
	using Fluxera.Entity;
	using JetBrains.Annotations;

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

		public string Description { get; set; }
	}
}
