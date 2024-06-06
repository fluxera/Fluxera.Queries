namespace Fluxera.Queries.UnitTests.EdmModel
{
	using FluentAssertions;
	using Fluxera.Queries.EdmModel;
	using Fluxera.Queries.UnitTests.Model;
	using NUnit.Framework;
	using System;
	using System.Collections.Generic;

	[TestFixture]
	public class EdmEnumTypeTests
	{
		[Test]
		public void ShouldThrowWhen_ClrTypeNull()
		{
			Action action = () => new EdmEnumType(null, new List<EdmEnumMember>());

			action.Should().Throw<ArgumentNullException>();
		}

		[Test]
		public void ShouldThrowWhen_MembersNull()
		{
			Action action = () => new EdmEnumType(typeof(CustomerState), null);

			action.Should().Throw<ArgumentNullException>();
		}

		[Test]
		public void ShouldCreateInstance()
		{
			EdmEnumType edmType = new EdmEnumType(typeof(CustomerState), new List<EdmEnumMember>());

			edmType.Should().NotBeNull();
			edmType.ClrType.Should().Be(typeof(CustomerState));
			edmType.Name.Should().Be("CustomerState");
			edmType.FullName.Should().Be("Fluxera.Queries.UnitTests.Model.CustomerState");
			edmType.Members.Should().NotBeNull();
		}
	}
}
