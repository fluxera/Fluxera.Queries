namespace Fluxera.Queries
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	[UsedImplicitly]			
	internal sealed class DefaultQueryExecutorFactory : IQueryExecutorFactory
	{
		private readonly IServiceProvider serviceProvider;

		public DefaultQueryExecutorFactory(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public IQueryExecutor Create(Type entityType, Type keyType)
		{
			Type queryExecutorType = typeof(IQueryExecutor<,>).MakeGenericType(entityType, keyType);

			IQueryExecutor queryExecutor = (IQueryExecutor)this.serviceProvider.GetRequiredService(queryExecutorType);
			return queryExecutor;
		}
	}
}
