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
		///		Enables or disables the $filter query option.
		/// </summary>
		/// <param name="isEnabled"></param>
		/// <returns></returns>
		IEntitySetOptionsBuilder<T> AllowFilter(bool isEnabled = true);

		/// <summary>
		///		Enables or disables the $orderby query option.
		/// </summary>
		/// <param name="isEnabled"></param>
		/// <returns></returns>
		IEntitySetOptionsBuilder<T> AllowOrderBy(bool isEnabled = true);

		/// <summary>
		///		Enables or disables the $skip query option.
		/// </summary>
		/// <param name="isEnabled"></param>
		/// <returns></returns>
		IEntitySetOptionsBuilder<T> AllowSkip(bool isEnabled = true);

		/// <summary>
		///		Enables or disables the $skiptoken query option.
		/// </summary>
		/// <param name="isEnabled"></param>
		/// <returns></returns>
		IEntitySetOptionsBuilder<T> AllowSkipToken(bool isEnabled = true);

		/// <summary>
		///		Enables or disables the $top query option.
		/// </summary>
		/// <param name="isEnabled"></param>
		/// <returns></returns>
		IEntitySetOptionsBuilder<T> AllowTop(bool isEnabled = true);

		/// <summary>
		///		Enables or disables the $count query option.
		/// </summary>
		/// <param name="isEnabled"></param>
		/// <returns></returns>
		IEntitySetOptionsBuilder<T> AllowCount(bool isEnabled = true);

		/// <summary>
		///		Enables or disables the $select query option.
		/// </summary>
		/// <param name="isEnabled"></param>
		/// <returns></returns>
		IEntitySetOptionsBuilder<T> AllowSelect(bool isEnabled = true);

		/// <summary>
		///		Adds the default search predicate to use.
		/// </summary>
		/// <param name="searchPredicate"></param>
		/// <returns></returns>
		IEntitySetOptionsBuilder<T> AllowSearch(Expression<Func<T, string, bool>> searchPredicate);

		///  <summary>
		/// 	Adds the default search predicate to use.
		///  </summary>
		///  <param name="isEnabled"></param>
		///  <param name="searchPredicate"></param>
		///  <returns></returns>
		IEntitySetOptionsBuilder<T> AllowSearch(bool isEnabled = true, Expression<Func<T, string, bool>> searchPredicate = null);
	}
}
