namespace SampleApp.Application.Customers.GetCustomer
{
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Extensions.Hosting.Modules.Application.Handlers;
	using Fluxera.Results;
	using JetBrains.Annotations;
	using SampleApp.Application.Contracts.Customers;
	using SampleApp.Application.Contracts.Customers.GetCustomer;

	[UsedImplicitly]
	internal sealed class GetCustomerQueryHandler : IApplicationQueryHandler<GetCustomerQuery, CustomerDto>
	{
		/// <inheritdoc />
		public Task<Result<CustomerDto>> HandleAsync(GetCustomerQuery query, CancellationToken cancellationToken)
		{
			// TODO: Implement query.

			return Task.FromResult(Result.Ok((CustomerDto)null));
		}
	}
}
