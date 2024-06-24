namespace Fluxera.Queries.Options
{
	using System;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	/// <summary>
	///		Build the options for the entity set.
	/// </summary>
	[PublicAPI]
	public interface IEntitySetOptionsBuilder<T>
		where T : class
	{
		/// <summary>
		///		Configure the entity set to always include the @odata.count inline count property.
		/// </summary>
		/// <returns></returns>
		IEntitySetOptionsBuilder<T> AlwaysIncludeCount();

		/// <summary>
		///		Adds a metadata entry for the entity set.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IEntitySetOptionsBuilder<T> WithMetadata(string key, object value);

		/// <summary>
		///		Adds the default search predicate to use.
		/// </summary>
		/// <param name="searchPredicate"></param>
		/// <returns></returns>
		IEntitySetOptionsBuilder<T> DefaultSearchPredicate(Expression<Func<T, string, bool>> searchPredicate);
	}
}
