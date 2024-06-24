namespace Fluxera.Queries.Options
{
	using System;
	using System.Linq.Expressions;

	internal sealed class EntitySetOptionsBuilder<T> : IEntitySetOptionsBuilder<T> 
		where T : class
	{
		private readonly EntitySetOptions options;

		public EntitySetOptionsBuilder(EntitySetOptions options)
		{
			this.options = options;
		}

		/// <inheritdoc />
		public IEntitySetOptionsBuilder<T> AlwaysIncludeCount()
		{
			this.options.AlwaysIncludeCount = true;

			return this;
		}

		/// <inheritdoc />
		public IEntitySetOptionsBuilder<T> WithMetadata(string key, object value)
		{
			this.options.Metadata.Add(key, value);

			return this;
		}

		/// <inheritdoc />
		public IEntitySetOptionsBuilder<T> DefaultSearchPredicate(Expression<Func<T, string, bool>> searchPredicate)
		{
			this.options.SearchPredicate = searchPredicate;

			return this;
		}
	}
}
