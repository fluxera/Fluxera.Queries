namespace Fluxera.Queries.UnitTests.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using FluentAssertions;
	using Fluxera.Queries.Model;
	using Fluxera.Queries.UnitTests;
	using NUnit.Framework;

	[TestFixture]
	public class EdmTypeProviderTests
	{
		[Test]
		[TestCase("Edm.Binary", "Binary")]
		[TestCase("Edm.Boolean", "Boolean")]
		[TestCase("Edm.Byte", "Byte")]
		[TestCase("Edm.Decimal", "Decimal")]
		[TestCase("Edm.Guid", "Guid")]
		[TestCase("Edm.Int16", "Int16")]
		[TestCase("Edm.Int32", "Int32")]
		[TestCase("Edm.Int64", "Int64")]
		[TestCase("Edm.SByte", "SByte")]
		[TestCase("Edm.Single", "Single")]
		[TestCase("Edm.Double", "Double")]
		[TestCase("Edm.String", "String")]
		[TestCase("Edm.TimeOfDay", "TimeOfDay")]
		[TestCase("Edm.Date", "Date")]
		[TestCase("Edm.DateTimeOffset", "DateTimeOffset")]
		[TestCase("Edm.Duration", "Duration")]
		public void ShouldProvidePrimitiveTypeByName(string fullName, string name)
		{
			EdmTypeProvider provider = new EdmTypeProvider();

			EdmType edmType = provider.GetByName(fullName);

			edmType.Should().NotBeNull();
			edmType.Name.Should().Be(name);
			edmType.FullName.Should().Be(fullName);
		}

		[Test]
		[TestCase(typeof(byte[]), "Edm.Binary", "Binary")]
		[TestCase(typeof(bool), "Edm.Boolean", "Boolean")]
		[TestCase(typeof(byte), "Edm.Byte", "Byte")]
		[TestCase(typeof(decimal), "Edm.Decimal", "Decimal")]
		[TestCase(typeof(Guid), "Edm.Guid", "Guid")]
		[TestCase(typeof(short), "Edm.Int16", "Int16")]
		[TestCase(typeof(int), "Edm.Int32", "Int32")]
		[TestCase(typeof(long), "Edm.Int64", "Int64")]
		[TestCase(typeof(sbyte), "Edm.SByte", "SByte")]
		[TestCase(typeof(float), "Edm.Single", "Single")]
		[TestCase(typeof(double), "Edm.Double", "Double")]
		[TestCase(typeof(string), "Edm.String", "String")]
		[TestCase(typeof(TimeOnly), "Edm.TimeOfDay", "TimeOfDay")]
		[TestCase(typeof(DateOnly), "Edm.Date", "Date")]
		[TestCase(typeof(DateTimeOffset), "Edm.DateTimeOffset", "DateTimeOffset")]
		[TestCase(typeof(TimeSpan), "Edm.Duration", "Duration")]
		public void ShouldProvidePrimitiveTypeByClrType(Type clrType, string fullName, string name)
		{
			EdmTypeProvider provider = new EdmTypeProvider();

			EdmType edmType = provider.GetByClrType(clrType);

			edmType.Should().NotBeNull();
			edmType.Name.Should().Be(name);
			edmType.FullName.Should().Be(fullName);
		}

		[Test]
		public void ShouldCreateEnumType_Enum()
		{
			EdmTypeProvider provider = new EdmTypeProvider();

			EdmType edmType = provider.GetByClrType(typeof(CustomerState));

			edmType.Should().NotBeNull();
			edmType.Should().BeOfType<EdmEnumType>();
			edmType.ClrType.Should().Be(typeof(CustomerState));
			edmType.Name.Should().Be("CustomerState");
			edmType.FullName.Should().Be("Fluxera.Queries.UnitTests.CustomerState");

			EdmEnumType edmEnumType = (EdmEnumType)edmType;
			edmEnumType.Members.Should().NotBeNullOrEmpty().And.HaveCount(2);

			edmEnumType.Members[0].Name.Should().Be("Legacy");
			edmEnumType.Members[0].Value.Should().Be(4);

			edmEnumType.Members[1].Name.Should().Be("New");
			edmEnumType.Members[1].Value.Should().Be(6);
		}

		[Test]
		public void ShouldCreateEnumType_Enumeration()
		{
			EdmTypeProvider provider = new EdmTypeProvider();

			EdmType edmType = provider.GetByClrType(typeof(CustomerStateEnum));

			edmType.Should().NotBeNull();
			edmType.Should().BeOfType<EdmEnumType>();
			edmType.ClrType.Should().Be(typeof(CustomerStateEnum));
			edmType.Name.Should().Be("CustomerStateEnum");
			edmType.FullName.Should().Be("Fluxera.Queries.UnitTests.CustomerStateEnum");

			EdmEnumType edmEnumType = (EdmEnumType)edmType;
			edmEnumType.Members.Should().NotBeNullOrEmpty().And.HaveCount(2);

			edmEnumType.Members[0].Name.Should().Be("Legacy");
			edmEnumType.Members[0].Value.Should().Be(4);

			edmEnumType.Members[1].Name.Should().Be("New");
			edmEnumType.Members[1].Value.Should().Be(6);
		}

		[Test]
		public void ShouldCreatePrimitiveType_StronglyTypedId()
		{
			EdmTypeProvider provider = new EdmTypeProvider();

			EdmType edmType = provider.GetByClrType(typeof(CustomerId));

			edmType.Should().NotBeNull();
			edmType.Should().BeOfType<EdmPrimitiveType>();
			edmType.ClrType.Should().Be(typeof(string));
			edmType.Name.Should().Be("String");
			edmType.FullName.Should().Be("Edm.String");
		}

		[Test]
		public void ShouldCreatePrimitiveType_PrimitiveValueObject()
		{
			EdmTypeProvider provider = new EdmTypeProvider();

			EdmType edmType = provider.GetByClrType(typeof(Age));

			edmType.Should().NotBeNull();
			edmType.Should().BeOfType<EdmPrimitiveType>();
			edmType.ClrType.Should().Be(typeof(int));
			edmType.Name.Should().Be("Int32");
			edmType.FullName.Should().Be("Edm.Int32");
		}

		[Test]
		public void ShouldCreateCollectionType()
		{
			EdmTypeProvider provider = new EdmTypeProvider();

			EdmType edmType = provider.GetByClrType(typeof(List<int>));

			edmType.Should().NotBeNull();
			edmType.Should().BeOfType<EdmCollectionType>();
			edmType.ClrType.Should().Be(typeof(List<int>));
			edmType.Name.Should().Be("Collection");
			edmType.FullName.Should().Be("Collection(Edm.Int32)");

			EdmCollectionType edmCollectionType = (EdmCollectionType)edmType;
			edmCollectionType.ItemType.Should().Be(EdmPrimitiveType.Int32);
		}

		[Test]
		public void ShouldCreateComplexType()
		{
			EdmTypeProvider provider = new EdmTypeProvider();

			EdmType edmType = provider.GetByClrType(typeof(Customer));

			edmType.Should().NotBeNull();
			edmType.Should().BeOfType<EdmComplexType>();
			edmType.ClrType.Should().Be(typeof(Customer));
			edmType.Name.Should().Be("Customer");
			edmType.FullName.Should().Be("Fluxera.Queries.UnitTests.Customer");

			EdmComplexType edmComplexType = (EdmComplexType)edmType;

			edmComplexType.Properties.Should().NotBeNullOrEmpty().And.HaveCount(6);

			EdmProperty idProperty = edmComplexType.Properties.FirstOrDefault(x => x.Name == "Id");
			idProperty.Should().NotBeNull();

			EdmProperty firstNameProperty = edmComplexType.Properties.FirstOrDefault(x => x.Name == "FirstName");
			firstNameProperty.Should().NotBeNull();

			EdmProperty lastNameProperty = edmComplexType.Properties.FirstOrDefault(x => x.Name == "LastName");
			lastNameProperty.Should().NotBeNull();

			EdmProperty stateProperty = edmComplexType.Properties.FirstOrDefault(x => x.Name == "State");
			stateProperty.Should().NotBeNull();

			EdmProperty stateEnumProperty = edmComplexType.Properties.FirstOrDefault(x => x.Name == "StateEnum");
			stateEnumProperty.Should().NotBeNull();

			EdmProperty ageProperty = edmComplexType.Properties.FirstOrDefault(x => x.Name == "Age");
			ageProperty.Should().NotBeNull();
		}
	}
}
