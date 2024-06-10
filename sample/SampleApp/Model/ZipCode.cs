namespace SampleApp.Model
{
	using Fluxera.ValueObject;

	public class ZipCode : PrimitiveValueObject<ZipCode, string>
	{
		/// <inheritdoc />
		public ZipCode(string value) : base(value)
		{
		}
	}
}
