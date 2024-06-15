namespace SampleApp.Application.Contracts.Customers.FindCustomers
{
	using Fluxera.Extensions.Hosting.Modules.Application.Contracts;
	using Fluxera.Extensions.Hosting.Modules.Application.Contracts.Dtos.Entities;
	using Fluxera.Queries.Options;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class FindCustomersQuery : IApplicationQuery<ListResultDto<CustomerDto>>
    {
		public FindCustomersQuery(QueryOptions query)
		{
			this.Query = query;
		}

		public QueryOptions Query { get; }
    }
}
