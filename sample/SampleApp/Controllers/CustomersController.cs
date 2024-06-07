//namespace SampleApp.Controllers
//{
//	using System.Threading;
//	using System.Threading.Tasks;
//	using Fluxera.Queries;
//	using Fluxera.Queries.AspNetCore;
//	using Fluxera.Queries.Repository;
//	using Fluxera.Repository;
//	using Microsoft.AspNetCore.Mvc;
//	using SampleApp.Model;

//	[Route("api")]
//	[ApiController]
//	public class CustomersController : ControllerBase
//	{
//		private readonly IRepository<Customer, CustomerId> repository;

//		public CustomersController(IRepository<Customer, CustomerId> repository)
//		{
//			this.repository = repository;
//		}

//		[HttpGet("customers")]
//		public async Task<IActionResult> Get(DataQuery<Customer> query, CancellationToken cancellationToken)
//		{
//			QueryResult<Customer> result = await this.repository.ExecuteQueryAsync(query, cancellationToken);

//			return this.Ok(result);
//		}
//	}
//}


