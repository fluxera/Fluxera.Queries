namespace SampleApp.Model
{
	using Fluxera.Repository.MongoDB;

	public sealed class SampleRepositoryContext : MongoContext
	{
		/// <inheritdoc />
		protected override void ConfigureOptions(MongoContextOptions options)
		{
			options.UseDbContext<SampleMongoContext>();
		}
	}
}
