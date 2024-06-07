namespace Fluxera.Queries.UnitTests.Model
{
	using System;
	using System.Collections.Generic;
	using FluentAssertions;
	using Fluxera.Queries.Model;
	using Fluxera.Queries.UnitTests;
	using NUnit.Framework;

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
			edmType.FullName.Should().Be("Fluxera.Queries.UnitTests.Customer");
			edmType.Properties.Should().NotBeNull();
			edmType.BaseType.Should().BeNull();
		}
	}
}
