namespace Fluxera.Queries.UnitTests.EdmModel
{
	using FluentAssertions;
	using Fluxera.Queries.EdmModel;
	using Fluxera.Queries.UnitTests.Model;
	using NUnit.Framework;
	using System;
	using System.Collections.Generic;

	[TestFixture]
	public class EdmComplexTypeTests
	{
		[Test]
		public void ShouldThrowWhen_ClrTypeNull()
		{
			Action action = () => new EdmComplexType(null, new List<EdmProperty>());

			action.Should().Throw<ArgumentNullException>();
		}

		[Test]
		public void ShouldThrowWhen_PropertiesNull()
		{
			Action action = () => new EdmComplexType(typeof(Customer), null);

			action.Should().Throw<ArgumentNullException>();
		}

		[Test]
		public void ShouldCreateInstance()
		{
			EdmComplexType edmType = new EdmComplexType(typeof(Customer), new List<EdmProperty>());

			edmType.Should().NotBeNull();
			edmType.ClrType.Should().Be(typeof(Customer));
			edmType.Name.Should().Be("Customer");
			edmType.FullName.Should().Be("Fluxera.Queries.UnitTests.Model.Customer");
			edmType.Properties.Should().NotBeNull();
			edmType.BaseType.Should().BeNull();
		}
	}
}
