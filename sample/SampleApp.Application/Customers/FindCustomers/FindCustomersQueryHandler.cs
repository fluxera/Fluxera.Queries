namespace SampleApp.Application.Customers.FindCustomers
{
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using AutoMapper;
	using Fluxera.Extensions.Hosting.Modules.Application.Contracts.Dtos.Entities;
	using Fluxera.Extensions.Hosting.Modules.Application.Handlers;
	using Fluxera.Repository.Query;
	using Fluxera.Results;
	using JetBrains.Annotations;
	using SampleApp.Application.Contracts.Customers;
	using SampleApp.Application.Contracts.Customers.FindCustomers;
	using SampleApp.Domain.Customers;

	[UsedImplicitly]
	internal sealed class FindCustomersQueryHandler : IApplicationQueryHandler<FindCustomersQuery, ListResultDto<CustomerDto>>
	{
        private readonly IMapper mapper;
        private readonly IQueryOptionsBuilder<Customer> queryOptionsBuilder;
        private readonly ICustomerRepository customerRepository;

        public FindCustomersQueryHandler(ICustomerRepository customerRepository, IMapper mapper, IQueryOptionsBuilder<Customer> queryOptionsBuilder)
        {
			this.mapper = mapper;
			this.queryOptionsBuilder = queryOptionsBuilder;
			this.customerRepository = customerRepository;
        }

		/// <inheritdoc />
		public Task<Result<ListResultDto<CustomerDto>>> HandleAsync(FindCustomersQuery query, CancellationToken cancellationToken)
		{
			IList<CustomerDto> customerDtos = new List<CustomerDto>();
			return Task.FromResult(Result.Ok(new ListResultDto<CustomerDto>(customerDtos.AsReadOnly())));
		}
	}
}
