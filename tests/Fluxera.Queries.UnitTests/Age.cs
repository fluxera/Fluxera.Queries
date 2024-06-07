
/* Unmerged change from project 'Fluxera.Queries.UnitTests (net8.0)'
Before:
namespace Fluxera.Queries.UnitTests.Model
After:
namespace Fluxera.Queries.UnitTests
{
	using Fluxera;
	using Fluxera.Queries;
	using Fluxera.Queries.UnitTests;
	using Fluxera.Queries.UnitTests;
	using Fluxera.Queries.UnitTests.Model
*/
namespace Fluxera.Queries.UnitTests
{
	using Fluxera.ValueObject;

	public class Age : PrimitiveValueObject<Age, int>
	{
		/// <inheritdoc />
		public Age(int value) : base(value)
		{
		}
	}
}
