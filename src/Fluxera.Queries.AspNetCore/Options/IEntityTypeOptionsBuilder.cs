namespace Fluxera.Queries.AspNetCore.Options
{
	using System;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	/// <summary>
	///		Build the options for an entity type.
	/// </summary>
	[PublicAPI]
	public interface IEntityTypeOptionsBuilder<T> where T : class
	{
		/// <summary>
		///		Selects the property that is used as key property.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="keySelector"></param>
		/// <returns></returns>
		IEntityTypeOptionsBuilder<T> HasKey<TKey>(Expression<Func<T, TKey>> keySelector);

		/// <summary>
		///		Ignore the property.
		/// </summary>
		/// <param name="propertySelector"></param>
		/// <returns></returns>
		IEntityTypeOptionsBuilder<T> Ignore<TValue>(Expression<Func<T, TValue>> propertySelector);
	}
}
