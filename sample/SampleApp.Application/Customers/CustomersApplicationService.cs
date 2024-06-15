namespace SampleApp.Application.Customers
{
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Extensions.Hosting.Modules.Application.Contracts.Dtos.Entities;
	using Fluxera.Queries.Options;
	using Fluxera.Results;
	using JetBrains.Annotations;
	using MediatR;
	using SampleApp.Application.Contracts.Customers;
	using SampleApp.Application.Contracts.Customers.FindCustomers;
	using SampleApp.Application.Contracts.Customers.GetCustomer;
	using SampleApp.Application.Contracts.Customers.GetCustomerCount;
	using SampleApp.Domain.Shared.Customers;

	[UsedImplicitly]
	internal sealed class CustomersApplicationService : ICustomersApplicationService
	{
		private readonly ISender sender;

		public CustomersApplicationService(ISender sender)
		{
			this.sender = sender;
		}

		/// <inheritdoc />
		public Task<Result<ListResultDto<CustomerDto>>> FindCustomersAsync(QueryOptions query, CancellationToken cancellationToken = default)
		{
			return this.sender.Send(new FindCustomersQuery(query), cancellationToken);
		}

		/// <inheritdoc />
		public Task<Result<CustomerDto>> GetCustomerAsync(CustomerId customerId, QueryOptions query, CancellationToken cancellationToken = default)
		{
			return this.sender.Send(new GetCustomerQuery(customerId, query), cancellationToken);
		}

		/// <inheritdoc />
		public Task<Result<long>> GetCustomerCountAsync(QueryOptions query, CancellationToken cancellationToken = default)
		{
			return this.sender.Send(new GetCustomerCountQuery(query), cancellationToken);
		}
	}
}
