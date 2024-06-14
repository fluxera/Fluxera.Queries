namespace SampleApp.Model
{
	using Fluxera.ValueObject;
	using JetBrains.Annotations;

	[PublicAPI]
	public sealed class CountryDto : ValueObject<CountryDto>
	{
		public string Code { get; set; }

		public string Name { get; set; }
	}
}
