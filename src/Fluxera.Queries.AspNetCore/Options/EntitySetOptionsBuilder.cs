namespace Fluxera.Queries.AspNetCore.Options
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
	}
}
