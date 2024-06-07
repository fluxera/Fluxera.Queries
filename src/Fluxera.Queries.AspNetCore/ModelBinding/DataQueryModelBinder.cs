﻿namespace Fluxera.Queries.AspNetCore.ModelBinding
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using Fluxera.Guards;
	using Fluxera.Queries;
	using Fluxera.Queries.AspNetCore;
	using Fluxera.Queries.Options;
	using Microsoft.AspNetCore.Mvc.ModelBinding;
	using Microsoft.Extensions.Logging;

	internal sealed class DataQueryModelBinder : IModelBinder
	{
		private readonly ILogger<DataQueryModelBinder> logger;
		private readonly IQueryParser parser;

		public DataQueryModelBinder(IQueryParser parser, ILogger<DataQueryModelBinder> logger)
		{
			this.parser = parser;
			this.logger = logger;
		}

		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			Guard.Against.Null(bindingContext);

			if(bindingContext.HttpContext.Request.Method != "GET")
			{
				throw new InvalidOperationException("The data queries ca only be used for GET requests.");
			}

			if(!bindingContext.ModelType.IsDataQuery())
			{
				return Task.CompletedTask;
			}

			try
			{
				Type entityType = bindingContext.ModelType.GetGenericArguments().First();

				DataQuery dataQuery = (DataQuery)Activator.CreateInstance(typeof(DataQuery<>).MakeGenericType(entityType));

				if(dataQuery is not null)
				{
					dataQuery.Filter = DataQuery.GetFilterParameterValue(bindingContext.HttpContext.Request.Query);
					dataQuery.OrderBy = DataQuery.GetOrderByParameterValue(bindingContext.HttpContext.Request.Query);

					QueryOptions queryOptions = this.parser.ParseQueryOptions(entityType, dataQuery.ToString());
					dataQuery.QueryOptions = queryOptions;
				}

				bindingContext.Result = ModelBindingResult.Success(dataQuery);
			}
			catch(QueryException ex)
			{
				bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex.Message);
				this.logger.LogWarning(ex, "Parsing error for data query.");
				bindingContext.Result = ModelBindingResult.Failed();
			}

			return Task.CompletedTask;
		}
	}
}
