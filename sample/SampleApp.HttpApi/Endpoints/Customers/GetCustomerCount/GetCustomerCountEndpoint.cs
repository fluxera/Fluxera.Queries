namespace SampleApp.HttpApi.Endpoints.Customers.GetCustomerCount
{
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Results;
	using Fluxera.Results.AspNetCore;
	using JetBrains.Annotations;
	using MadEyeMatt.AspNetCore.Endpoints;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Routing;
	using SampleApp.Application.Contracts.Customers;

	[PublicAPI]
	[EndpointGroup("Customers")]
	public sealed class GetCustomerCountEndpoint : EndpointBase
    {
        /// <inheritdoc />
        public override void Map(IEndpointRouteBuilder endpoints)
        {
            endpoints
                .MapGet(ExecuteAsync, "count")
                .AllowAnonymous();
        }

        public static async Task<IResult> ExecuteAsync(
            HttpContext httpContext,
            [FromServices] ICustomersApplicationService customersApplicationService,
            CancellationToken cancellationToken = default)
        {
            Result<long> result = await customersApplicationService.GetCustomerCountAsync(null, cancellationToken);
            return result.ToHttpResult();
        }
    }
}
