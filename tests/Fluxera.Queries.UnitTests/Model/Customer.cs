namespace Fluxera.Queries.UnitTests.Model
{
	public class Customer
	{
		public CustomerId Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public CustomerState State { get; set; }

		public CustomerStateEnum StateEnum { get; set; }

		public Age Age { get; set; }
	}
}
