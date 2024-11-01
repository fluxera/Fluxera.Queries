namespace Fluxera.Queries.Options
{
	using System;
	using System.Linq.Expressions;
	using Fluxera.Guards;

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
		public IEntitySetOptionsBuilder<T> SetMaxTop(int maxTop)
		{
			this.options.MaxTop = maxTop;

			return this;
		}

		/// <inheritdoc />
		public IEntitySetOptionsBuilder<T> SetDefaultTop(int defaultTop)
		{
			this.options.DefaultTop = defaultTop;

			return this;
		}

		/// <inheritdoc />
		public IEntitySetOptionsBuilder WithMetadata(string key, object value) 
		{
			this.options.Metadata[key] = value;

			return this;
		}

		/// <inheritdoc />
		public IEntitySetOptionsBuilder<T> WithMetadataTyped(string key, object value)
		{
			this.WithMetadata(key, value);

			return this;
		}

		/// <inheritdoc />
		public IEntitySetOptionsBuilder<T> AllowFilter(bool isEnabled = true)
		{
			this.options.AllowFilter = isEnabled;

			return this;
		}

		/// <inheritdoc />
		public IEntitySetOptionsBuilder<T> AllowOrderBy(bool isEnabled = true)
		{
			this.options.AllowOrderBy = isEnabled;

			return this;
		}

		/// <inheritdoc />
		public IEntitySetOptionsBuilder<T> AllowSkip(bool isEnabled = true)
		{
			this.options.AllowSkip = isEnabled;

			return this;
		}

		/// <inheritdoc />
		public IEntitySetOptionsBuilder<T> AllowTop(bool isEnabled = true)
		{
			this.options.AllowTop = isEnabled;

			return this;
		}

		/// <inheritdoc />
		public IEntitySetOptionsBuilder<T> AllowCount(bool isEnabled = true)
		{
			this.options.AllowCount = isEnabled;

			return this;
		}

		/// <inheritdoc />
		public IEntitySetOptionsBuilder<T> AllowSelect(bool isEnabled = true)
		{
			this.options.AllowSelect = isEnabled;

			return this;
		}

		/// <inheritdoc />
		public IEntitySetOptionsBuilder<T> AllowSearch(Expression<Func<T, string, bool>> searchPredicate)
		{
			Guard.Against.Null(searchPredicate);

			return this.AllowSearch(true, searchPredicate);
		}

		/// <inheritdoc />
		public IEntitySetOptionsBuilder<T> AllowSearch(bool isEnabled = true, Expression<Func<T, string, bool>> searchPredicate = null)
		{
			this.options.AllowSearch = isEnabled;
			this.options.SearchPredicate = searchPredicate;

			return this;
		}
	}
}
