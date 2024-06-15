namespace SampleApp.Application.Contracts.Customers.GetCustomerCount
{
	using Fluxera.Extensions.Hosting.Modules.Application.Contracts;
	using Fluxera.Queries.Options;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class GetCustomerCountQuery : IApplicationQuery<long>
	{
		public GetCustomerCountQuery(QueryOptions query)
		{
			this.Query = query;
		}

		public QueryOptions Query { get; }
	}
}
