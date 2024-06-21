namespace Fluxera.Queries.Options
{
	using System;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class DataQueriesOptionsBuilder : IDataQueriesOptionsBuilder
	{
		private readonly DataQueriesOptions options;

		public DataQueriesOptionsBuilder(DataQueriesOptions options)
		{
			this.options = options;
		}

		/// <inheritdoc />
		public IEntitySetOptionsBuilder<T> EntitySet<T>(string name, string entityTypeName, Action<IEntityTypeOptionsBuilder<T>> configure = null) where T : class
		{
			EntitySetOptions entitySetOptions = this.options.EntitySet(name, entityTypeName, configure);

			return new EntitySetOptionsBuilder<T>(entitySetOptions);
		}

		/// <inheritdoc />
		public IEntitySetOptionsBuilder<T> EntitySet<T>(string name, Action<IEntityTypeOptionsBuilder<T>> configure = null) where T : class
		{
			EntitySetOptions entitySetOptions = this.options.EntitySet(name, configure);

			return new EntitySetOptionsBuilder<T>(entitySetOptions);
		}

		/// <inheritdoc />
		public void ComplexType<T>(string complexTypeName, Action<IComplexTypeOptionsBuilder<T>> configure = null) where T : class
		{
			this.options.ComplexType(complexTypeName, configure);
		}

		/// <inheritdoc />
		public void ComplexType<T>(Action<IComplexTypeOptionsBuilder<T>> configure = null) where T : class
		{
			this.options.ComplexType(configure);
		}
	}
}
