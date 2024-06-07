namespace SampleApp.Model
{
	using Fluxera.Utilities.Extensions;
	using MadEyeMatt.MongoDB.DbContext;

	public sealed class SampleMongoContext : MongoDbContext
	{
		/// <inheritdoc />
		protected override void OnConfiguring(MongoDbContextOptionsBuilder builder)
		{
			builder.UseDatabase("mongodb://localhost:27017", "queries-sample");
		}

		/// <inheritdoc />
		public override string GetCollectionName<TDocument>()
		{
			return base.GetCollectionName<TDocument>().Pluralize();
		}
	}
}
