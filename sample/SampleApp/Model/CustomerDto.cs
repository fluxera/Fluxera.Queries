namespace SampleApp.Model
{
	using Fluxera.Entity;

	public sealed class CustomerDto : AggregateRoot<CustomerDto, CustomerId>
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public Age Age { get; set; }

		public CustomerState State { get; set; }

		public AddressDto Address { get; set; }

		public double IgnoreMe { get; set; }
	}
}
