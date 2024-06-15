namespace SampleApp.Application.Customers.GetCustomerCount
{
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Extensions.Hosting.Modules.Application.Handlers;
	using Fluxera.Results;
	using JetBrains.Annotations;
	using SampleApp.Application.Contracts.Customers.GetCustomerCount;

	[UsedImplicitly]
	internal sealed class GetCustomerCountQueryHandler : IApplicationQueryHandler<GetCustomerCountQuery, long>
	{
		/// <inheritdoc />
		public Task<Result<long>> HandleAsync(GetCustomerCountQuery query, CancellationToken cancellationToken)
		{
			// TODO: Implement query.

			return Task.FromResult(Result.Ok(0L));
		}
	}
}
