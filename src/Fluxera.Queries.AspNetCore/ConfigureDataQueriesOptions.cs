namespace Fluxera.Queries.AspNetCore
{
	using Fluxera.Queries.Model;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Options;

	[UsedImplicitly]
	internal sealed class ConfigureDataQueriesOptions : IPostConfigureOptions<DataQueriesOptions>
	{
		private readonly IEdmTypeProvider edmTypeProvider;

		public ConfigureDataQueriesOptions(IEdmTypeProvider edmTypeProvider)
		{
			this.edmTypeProvider = edmTypeProvider;
		}

		/// <inheritdoc />
		public void PostConfigure(string name, DataQueriesOptions options)
		{
			foreach (EntitySetOptions entitySetOptions in options.EntitySetOptions)
			{
				EdmComplexType edmType = (EdmComplexType)this.edmTypeProvider.GetByClrType(entitySetOptions.EntityType);
				entitySetOptions.EntitySet = new EntitySet(entitySetOptions.Name, edmType);
			}
		}
	}
}
