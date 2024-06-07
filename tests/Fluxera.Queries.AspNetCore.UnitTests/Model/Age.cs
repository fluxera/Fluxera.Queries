﻿namespace Fluxera.Queries.AspNetCore.UnitTests.Model
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
