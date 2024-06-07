namespace Fluxera.Queries.AspNetCore.ModelBinding
{
	using Fluxera.Guards;
	using Microsoft.AspNetCore.Mvc.ModelBinding;
	using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

	internal sealed class DataQueryModelBinderProvider : IModelBinderProvider
	{
		public IModelBinder GetBinder(ModelBinderProviderContext context)
		{
			Guard.Against.Null(context);

			return context.Metadata.ModelType.IsDataQuery() 
				? new BinderTypeModelBinder(typeof(DataQueryModelBinder)) 
				: null;
		}
	}
}
