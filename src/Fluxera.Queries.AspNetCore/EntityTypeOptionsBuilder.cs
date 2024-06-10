namespace Fluxera.Queries.AspNetCore
{
	using System;
	using System.Linq.Expressions;
	using System.Reflection;

	internal sealed class EntityTypeOptionsBuilder<T> : IEntityTypeOptionsBuilder<T> 
		where T : class
	{
		private readonly EntitySetOptions entitySetOptions;

		public EntityTypeOptionsBuilder(EntitySetOptions entitySetOptions)
		{
			this.entitySetOptions = entitySetOptions;
		}

		/// <inheritdoc />
		public IEntityTypeOptionsBuilder<T> HasKey<TKey>(Expression<Func<T, TKey>> keySelector)
		{
			this.entitySetOptions.KeyType = keySelector.ReturnType;

			return this;
		}

		/// <inheritdoc />
		public IEntityTypeOptionsBuilder<T> Ignore<TValue>(Expression<Func<T, TValue>> propertySelector)
		{
			MemberExpression expression = (MemberExpression)propertySelector.Body;
			PropertyInfo propertyInfo = (PropertyInfo)expression.Member;

			this.entitySetOptions.ComplexTypeOptions.IgnoreProperty(propertyInfo);

			return this;
		}
	}
}