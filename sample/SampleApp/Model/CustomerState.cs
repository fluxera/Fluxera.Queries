namespace SampleApp.Model
{
	using Fluxera.Enumeration;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class CustomerState : Enumeration<CustomerState>
	{
		public static readonly CustomerState Legacy = new CustomerState(69, "Legacy");

		public static readonly CustomerState New = new CustomerState(42, "New");

		/// <inheritdoc />
		private CustomerState(int value, string name)
			: base(value, name)
		{
		}
	}
}
