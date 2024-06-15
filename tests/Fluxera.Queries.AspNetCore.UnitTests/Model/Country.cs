namespace Fluxera.Queries.AspNetCore.UnitTests.Model
{
	using Fluxera.ValueObject;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class Country : ValueObject<Country>
	{
		public string Code { get; set; }

		public string Name { get; set; }
	}
}
