namespace Fluxera.Queries.UnitTests
{
	using Fluxera.Enumeration;

	public sealed class CustomerStateEnum : Enumeration<CustomerStateEnum>
	{
		public static CustomerStateEnum Legacy = new CustomerStateEnum(4, "Legacy");

		public static CustomerStateEnum New = new CustomerStateEnum(6, "New");

		/// <inheritdoc />
		public CustomerStateEnum(int value, string name)
			: base(value, name)
		{
		}
	}
}
