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
		///		Configure the entity set to use this max top value, even if a higher values
		///		was requested in the $top query option.
		/// </summary>
		/// <returns></returns>
		IEntitySetOptionsBuilder<T> SetMaxTop(int maxTop);

		/// <summary>
		///		Configure the default top value, if none was provided.
		/// </summary>
		/// <param name="defaultTop"></param>
		/// <returns></returns>
		IEntitySetOptionsBuilder<T> SetDefaultTop(int defaultTop);

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
		IEntitySetOptionsBuilder<T> EnableSearch(Expression<Func<T, string, bool>> searchPredicate);
	}
}
