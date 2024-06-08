namespace SampleApp.Endpoints
{
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Queries;
	using Fluxera.Queries.AspNetCore;
	using Fluxera.Queries.Repository;
	using Fluxera.Repository;
	using MadEyeMatt.AspNetCore.Endpoints;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Routing;
	using SampleApp.Model;

	[EndpointGroup("Customers")]
	public sealed class FindCustomersEndpoint : EndpointBase
	{
		/// <inheritdoc />
		public override void Map(IEndpointRouteBuilder endpoints)
		{
			endpoints.MapGet(ExecuteAsync, "customers");
		}

		private static async Task<IResult> ExecuteAsync(
			DataQuery<Customer> query,
			IRepository<Customer, CustomerId> repository,
			CancellationToken cancellationToken = default)
		{
			QueryResult result = await repository.ExecuteFindManyAsync(query, cancellationToken);

			return Results.Ok(result);
		}
	}

	[EndpointGroup("Customers")]
	public sealed class GetCustomersCountEndpoint : EndpointBase
	{
		/// <inheritdoc />
		public override void Map(IEndpointRouteBuilder endpoints)
		{
			endpoints.MapGet(ExecuteAsync, "customers/$count");
		}

		private static async Task<IResult> ExecuteAsync(
			DataQuery<Customer> query,
			IRepository<Customer, CustomerId> repository,
			CancellationToken cancellationToken = default)
		{
			long result = await repository.ExecuteCountAsync(query, cancellationToken);

			return Results.Ok(result);
		}
	}

	[EndpointGroup("Customers")]
	public sealed class GetCustomerEndpoint : EndpointBase
	{
		/// <inheritdoc />
		public override void Map(IEndpointRouteBuilder endpoints)
		{
			endpoints.MapGet(ExecuteAsync, "customers/{id}");
		}

		private static async Task<IResult> ExecuteAsync(
			CustomerId id,
			DataQuery<Customer> query,
			IRepository<Customer, CustomerId> repository,
			CancellationToken cancellationToken = default)
		{
			object result = await repository.ExecuteGetAsync(id, query, cancellationToken);

			return Results.Ok(result);
		}
	}
}
