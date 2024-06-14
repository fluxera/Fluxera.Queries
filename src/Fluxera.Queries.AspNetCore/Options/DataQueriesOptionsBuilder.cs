﻿namespace Fluxera.Queries.AspNetCore.Options
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
		public IDataQueriesOptionsBuilder ComplexType<T>(string complexTypeName, Action<IComplexTypeOptionsBuilder<T>> configure = null) where T : class
		{
			this.options.ComplexType(complexTypeName, configure);

			return this;
		}

		/// <inheritdoc />
		public IDataQueriesOptionsBuilder ComplexType<T>(Action<IComplexTypeOptionsBuilder<T>> configure = null) where T : class
		{
			this.options.ComplexType(configure);

			return this;
		}

		/// <inheritdoc />
		public IDataQueriesOptionsBuilder EntitySet<T>(string name, string entityTypeName, Action<IEntityTypeOptionsBuilder<T>> configure = null) where T : class
		{
			this.options.EntitySet(name, entityTypeName, configure);

			return this;
		}

		/// <inheritdoc />
		public IDataQueriesOptionsBuilder EntitySet<T>(string name, Action<IEntityTypeOptionsBuilder<T>> configure = null) where T : class
		{
			this.options.EntitySet(name, configure);

			return this;
		}
	}
}
