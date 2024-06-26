﻿namespace Fluxera.Queries.UnitTests.Model
{
	using System;
	using FluentAssertions;
	using Fluxera.Queries.Model;
	using NUnit.Framework;

	[TestFixture]
	public class EdmEnumMemberTests
	{
		[Test]
		public void ShouldThrowWhen_NameNull()
		{
			Action action = () => new EdmEnumMember(null, 0);

			action.Should().Throw<ArgumentNullException>();
		}

		[Test]
		[TestCase("")]
		[TestCase("   ")]
		public void ShouldThrowWhen_Name(string name)
		{
			Action action = () => new EdmEnumMember(name, 0);

			action.Should().Throw<ArgumentException>();
		}

		[Test]
		public void ShouldCreateInstance()
		{
			EdmEnumMember edmEnumMember = new EdmEnumMember("Legacy", 0);

			edmEnumMember.Should().NotBeNull();
			edmEnumMember.Name.Should().Be("Legacy");
			edmEnumMember.Value.Should().Be(0);
		}
	}
}
