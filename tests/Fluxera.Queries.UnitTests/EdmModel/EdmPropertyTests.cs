namespace Fluxera.Queries.UnitTests.EdmModel
{
	using System;
	using System.Collections.Generic;
	using FluentAssertions;
	using Fluxera.Queries.EdmModel;
	using Fluxera.Queries.UnitTests.Model;
	using NUnit.Framework;

	[TestFixture]
	public class EdmPropertyTests
	{
		[Test]
		public void ShouldThrowWhen_NameNull()
		{
			Action action = () => new EdmProperty(null, EdmPrimitiveType.String, new EdmComplexType(typeof(Customer), new List<EdmProperty>()));

			action.Should().Throw<ArgumentNullException>();
		}

		[Test]
		[TestCase("")]
		[TestCase("   ")]
		public void ShouldThrowWhen_Name(string name)
		{
			Action action = () => new EdmProperty(name, EdmPrimitiveType.String, new EdmComplexType(typeof(Customer), new List<EdmProperty>()));

			action.Should().Throw<ArgumentException>();
		}

		[Test]
		public void ShouldThrowWhen_PropertyTypeNull()
		{
			Action action = () => new EdmProperty("FirstName", null, new EdmComplexType(typeof(Customer), new List<EdmProperty>()));

			action.Should().Throw<ArgumentNullException>();
		}

		[Test]
		public void ShouldThrowWhen_DeclaringTypeNull()
		{
			Action action = () => new EdmProperty("FirstName", EdmPrimitiveType.String, null);

			action.Should().Throw<ArgumentNullException>();
		}

		[Test]
		public void ShouldCreateInstance()
		{
			EdmProperty edmProperty = new EdmProperty("FirstName", EdmPrimitiveType.String, new EdmComplexType(typeof(Customer), new List<EdmProperty>()));

			edmProperty.Should().NotBeNull();
			edmProperty.Name.Should().Be("FirstName");
			edmProperty.PropertyType.Should().Be(EdmPrimitiveType.String);
			edmProperty.DeclaringType.Should().NotBeNull();
		}
	}
}
