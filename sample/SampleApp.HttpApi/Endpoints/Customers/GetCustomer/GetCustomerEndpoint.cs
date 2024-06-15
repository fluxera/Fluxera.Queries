namespace SampleApp.HttpApi.Endpoints.Customers.GetCustomer
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
	using SampleApp.Domain.Shared.Customers;

	[PublicAPI]
	[EndpointGroup("Customers")]
	public sealed class GetCustomerEndpoint : EndpointBase
    {
        /// <inheritdoc />
        public override void Map(IEndpointRouteBuilder endpoints)
        {
            endpoints
                .MapGet(ExecuteAsync, "{id:required}")
                .AllowAnonymous();
        }

        public static async Task<IResult> ExecuteAsync(
            HttpContext httpContext,
            [FromRoute] string id,
            [FromServices] ICustomersApplicationService customersApplicationService,
            CancellationToken cancellationToken = default)
        {
			CustomerId customerId = new CustomerId(id);
			Result<CustomerDto> result = await customersApplicationService.GetCustomerAsync(customerId, null, cancellationToken);
            return result.ToHttpResult();
        }
    }
}
