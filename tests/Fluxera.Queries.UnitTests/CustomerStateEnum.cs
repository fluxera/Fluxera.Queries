namespace Fluxera.Queries.UnitTests
{

	/* Unmerged change from project 'Fluxera.Queries.UnitTests (net8.0)'
	Before:
		using Fluxera.Enumeration;
	After:
		using Fluxera.Enumeration;
		using Fluxera.Queries;
		using Fluxera.Queries.UnitTests;
		using Fluxera.Queries.UnitTests;
		using Fluxera.Queries.UnitTests.Model;
	*/
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
