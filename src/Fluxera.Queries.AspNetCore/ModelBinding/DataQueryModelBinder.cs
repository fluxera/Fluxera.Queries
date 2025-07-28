namespace Fluxera.Queries.AspNetCore.ModelBinding
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using Fluxera.Guards;
	using Fluxera.Queries;
	using Fluxera.Queries.AspNetCore;
	using Fluxera.Queries.Model;
	using Fluxera.Queries.Options;
	using Microsoft.AspNetCore.Mvc.ModelBinding;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Options;

	internal sealed class DataQueryModelBinder : IModelBinder
	{
		private readonly IQueryParser parser;
		private readonly IOptions<DataQueriesOptions> dataQueryOptions;
		private readonly ILogger<DataQueryModelBinder> logger;

		public DataQueryModelBinder(IQueryParser parser, IOptions<DataQueriesOptions> dataQueryOptions, ILogger<DataQueryModelBinder> logger)
		{
			this.parser = parser;
			this.dataQueryOptions = dataQueryOptions;
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
					dataQuery.Skip = DataQuery.GetSkipParameterValue(bindingContext.HttpContext.Request.Query);
					dataQuery.SkipToken = DataQuery.GetSkipTokenParameterValue(bindingContext.HttpContext.Request.Query);
					dataQuery.Top = DataQuery.GetTopParameterValue(bindingContext.HttpContext.Request.Query);
					dataQuery.Count = DataQuery.GetCountParameterValue(bindingContext.HttpContext.Request.Query);
					dataQuery.Select = DataQuery.GetSelectParameterValue(bindingContext.HttpContext.Request.Query);
					dataQuery.Search = DataQuery.GetSearchParameterValue(bindingContext.HttpContext.Request.Query);

					EntitySet entitySet = this.dataQueryOptions.Value.GetByType(entityType);
					QueryOptions queryOptions = this.parser.ParseQueryOptions(entitySet, dataQuery.ToString());
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
