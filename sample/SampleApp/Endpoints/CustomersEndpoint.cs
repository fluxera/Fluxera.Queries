namespace SampleApp.Endpoints
{
	using Fluxera.Queries.AspNetCore;
	using MadEyeMatt.AspNetCore.Endpoints;
	using SampleApp.Model;

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
