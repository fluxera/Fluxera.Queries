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
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Routing;
	using SampleApp.Model;

	[EndpointGroup("Customers")]
	public sealed class CustomersEndpoint : EndpointBase
	{
		/// <inheritdoc />
		public override void Map(IEndpointRouteBuilder endpoints)
		{
			endpoints.MapGet(ExecuteAsync, "customers");
		}

		private static async Task<IResult> ExecuteAsync(DataQuery<Customer> query,
			[FromServices] IRepository<Customer, CustomerId> repository,
			CancellationToken cancellationToken = default)
		{
			QueryResult<Customer> result = await repository.ExecuteQueryAsync(query, cancellationToken);

			return Results.Ok(result);
		}
	}
}
