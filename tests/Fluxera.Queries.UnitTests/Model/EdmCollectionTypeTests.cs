namespace Fluxera.Queries.UnitTests.Model
{
	using System;
	using FluentAssertions;
	using Fluxera.Queries.Model;
	using NUnit.Framework;

	[TestFixture]
	public class EdmCollectionTypeTests
	{
		[Test]
		public void ShouldThrowWhen_ClrTypeNull()
		{
			Action action = () => new EdmCollectionType(null, EdmPrimitiveType.Int32);

			action.Should().Throw<ArgumentNullException>();
		}

		[Test]
		public void ShouldThrowWhen_ItemTypeNull()
		{
			Action action = () => new EdmCollectionType(typeof(int), null);

			action.Should().Throw<ArgumentNullException>();
		}

		[Test]
		public void ShouldCreateInstance()
		{
			EdmCollectionType edmType = new EdmCollectionType(typeof(int), EdmPrimitiveType.Int32);

			edmType.Should().NotBeNull();
			edmType.ItemType.Should().Be(EdmPrimitiveType.Int32);
			edmType.ClrType.Should().Be(typeof(int));
			edmType.Name.Should().Be("Collection");
			edmType.FullName.Should().Be("Collection(Edm.Int32)");
		}
	}
}
