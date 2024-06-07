namespace Fluxera.Queries.UnitTests.Model
{
	using NUnit.Framework;
	using System;
	using FluentAssertions;
	using Fluxera.Queries.Model;

	[TestFixture]
	public class EntitySetTests
	{
		[Test]
		public void ShouldThrowWhen_NameNull()
		{
			EdmTypeProvider provider = new EdmTypeProvider();

			Action action = () => new EntitySet(null, (EdmComplexType)provider.GetByClrType(typeof(Customer)));

			action.Should().Throw<ArgumentNullException>();
		}

		[Test]
		[TestCase("")]
		[TestCase("   ")]
		public void ShouldThrowWhen_Name(string name)
		{
			EdmTypeProvider provider = new EdmTypeProvider();

			Action action = () => new EntitySet(name, (EdmComplexType)provider.GetByClrType(typeof(Customer)));

			action.Should().Throw<ArgumentException>();
		}
	}
}
