namespace Fluxera.Queries.AspNetCore
{
	using System;
	using System.Linq.Expressions;
	using System.Reflection;

	internal sealed class ComplexTypeOptionsBuilder<T> : IComplexTypeOptionsBuilder<T>
		where T : class
	{
		private readonly ComplexTypeOptions complexTypeOptions;

		public ComplexTypeOptionsBuilder(ComplexTypeOptions complexTypeOptions)
		{
			this.complexTypeOptions = complexTypeOptions;
		}

		/// <inheritdoc />
		public IComplexTypeOptionsBuilder<T> Ignore<TValue>(Expression<Func<T, TValue>> propertySelector)
		{
			MemberExpression expression = (MemberExpression)propertySelector.Body;
			PropertyInfo propertyInfo = (PropertyInfo)expression.Member;

			this.complexTypeOptions.IgnoreProperty(propertyInfo);

			return this;
		}
	}
}
