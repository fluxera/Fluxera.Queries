namespace Fluxera.Queries.AspNetCore.UnitTests.Endpoints
{
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Queries.AspNetCore;
	using Fluxera.Queries.AspNetCore.UnitTests.Model;
	using MadEyeMatt.AspNetCore.Endpoints;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Routing;

	[EndpointGroup("Customers")]
	public sealed class CustomersEndpoint : EndpointBase
	{
		private static IList<Customer> customers = new List<Customer>();

		/// <inheritdoc />
		public override void Map(IEndpointRouteBuilder endpoints)
		{
			endpoints.MapGet(ExecuteAsync, "customers");
		}

		private static async Task<IResult> ExecuteAsync(DataQuery<Customer> dataQuery, CancellationToken cancellationToken = default)
		{
			return Results.Ok();
		}
	}
}
