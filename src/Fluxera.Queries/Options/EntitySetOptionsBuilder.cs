namespace Fluxera.Queries.Options
{
	internal sealed class EntitySetOptionsBuilder : IEntitySetOptionsBuilder
	{
		private readonly EntitySetOptions options;

		public EntitySetOptionsBuilder(EntitySetOptions options)
		{
			this.options = options;
		}

		/// <inheritdoc />
		public IEntitySetOptionsBuilder AlwaysIncludeCount()
		{
			this.options.AlwaysIncludeCount = true;

			return this;
		}

		/// <inheritdoc />
		public IEntitySetOptionsBuilder WithMetadata(string key, object value)
		{
			this.options.Metadata.Add(key, value);

			return this;
		}
	}
}
