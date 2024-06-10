namespace SampleApp.Model
{
	using Fluxera.ValueObject;

	public class AddressDto : ValueObject<AddressDto>
	{
		public string Street { get; set; }

		public string Number { get; set; }

		public string City { get; set; }

		public ZipCode ZipCode { get; set; }

		public decimal IgnoreMe { get; set; }
	}
}
