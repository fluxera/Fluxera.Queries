namespace SampleApp.Model
{
	using Fluxera.Enumeration;

	public sealed class CustomerState : Enumeration<CustomerState>
	{
		public static CustomerState Legacy = new CustomerState(4, "Legacy");

		public static CustomerState New = new CustomerState(6, "New");

		/// <inheritdoc />
		private CustomerState(int value, string name)
			: base(value, name)
		{
		}
	}
}
