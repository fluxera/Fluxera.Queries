namespace SampleApp.Model
{
	using Fluxera.StronglyTypedId;

	public sealed class CustomerId : StronglyTypedId<CustomerId, string>
	{
		/// <inheritdoc />
		public CustomerId(string value) : base(value)
		{
		}
	}
}
