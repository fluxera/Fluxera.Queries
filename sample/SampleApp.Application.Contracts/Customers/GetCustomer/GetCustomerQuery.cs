namespace SampleApp.Application.Contracts.Customers.GetCustomer
{
	using Fluxera.Extensions.Hosting.Modules.Application.Contracts;
	using Fluxera.Queries.Options;
	using JetBrains.Annotations;
	using SampleApp.Domain.Shared.Customers;

	[PublicAPI]
	public sealed class GetCustomerQuery : IApplicationQuery<CustomerDto>
	{
		public GetCustomerQuery(CustomerId customerId, QueryOptions query)
		{
			this.CustomerId = customerId;
			this.Query = query;
		}

		public CustomerId CustomerId { get; }

		public QueryOptions Query { get; }
	}
}
