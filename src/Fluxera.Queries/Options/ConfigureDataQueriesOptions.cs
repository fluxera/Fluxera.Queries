namespace Fluxera.Queries.Options
{
	using System.Reflection;
	using Fluxera.Queries.Model;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Options;

	[UsedImplicitly]
	internal sealed class ConfigureDataQueriesOptions : IPostConfigureOptions<DataQueriesOptions>
	{
		private readonly IEdmTypeProvider typeProvider;

		public ConfigureDataQueriesOptions(IEdmTypeProvider edmTypeProvider)
		{
			this.typeProvider = edmTypeProvider;
		}

		/// <inheritdoc />
		public void PostConfigure(string name, DataQueriesOptions options)
		{
			foreach(EntitySetOptions entitySetOptions in options.EntitySetOptions.Values)
			{
				EdmComplexType edmType = (EdmComplexType)this.typeProvider.GetByType(entitySetOptions.ComplexTypeOptions.ClrType);

				entitySetOptions.EntitySet = new EntitySet(entitySetOptions.Name, edmType);

				this.ConfigureComplexType(entitySetOptions.ComplexTypeOptions);
			}

			foreach(ComplexTypeOptions complexTypeOptions in options.ComplexTypeOptions.Values)
			{
				this.ConfigureComplexType(complexTypeOptions);
			}
		}

		private void ConfigureComplexType(ComplexTypeOptions complexTypeOptions)
		{
			EdmComplexType edmType = (EdmComplexType)this.typeProvider.GetByType(complexTypeOptions.ClrType);

			if(!string.IsNullOrWhiteSpace(complexTypeOptions.TypeName))
			{
				edmType.Rename(complexTypeOptions.TypeName);
			}

			foreach(PropertyInfo ignoredProperty in complexTypeOptions.IgnoredProperties)
			{
				edmType.IgnoreProperty(ignoredProperty);
			}
		}
	}
}
