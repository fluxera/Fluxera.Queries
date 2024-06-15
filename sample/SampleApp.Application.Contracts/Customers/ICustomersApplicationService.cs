namespace SampleApp.Application.Contracts.Customers
{
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Extensions.Hosting.Modules.Application.Contracts;
	using Fluxera.Extensions.Hosting.Modules.Application.Contracts.Dtos.Entities;
	using Fluxera.Queries.Options;
	using Fluxera.Results;
	using JetBrains.Annotations;
	using SampleApp.Domain.Shared.Customers;

	[PublicAPI]
	public interface ICustomersApplicationService : IApplicationService
	{
		Task<Result<ListResultDto<CustomerDto>>> FindCustomersAsync(QueryOptions query, CancellationToken cancellationToken = default);

		Task<Result<CustomerDto>> GetCustomerAsync(CustomerId customerId, QueryOptions query, CancellationToken cancellationToken = default);

		Task<Result<long>> GetCustomerCountAsync(QueryOptions query, CancellationToken cancellationToken = default);
	}
}
