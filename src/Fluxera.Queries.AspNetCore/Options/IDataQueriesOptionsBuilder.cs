namespace Fluxera.Queries.AspNetCore.Options
{
	using System;
	using JetBrains.Annotations;

	/// <summary>
	///		Build options for the queries library.
	/// </summary>
	[PublicAPI]
	public interface IDataQueriesOptionsBuilder
	{
		/// <summary>
		///		Configures a complex type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="complexTypeName"></param>
		/// <param name="configure"></param>
		IDataQueriesOptionsBuilder ComplexType<T>(string complexTypeName, Action<IComplexTypeOptionsBuilder<T>> configure = null) where T : class;

		/// <summary>
		///		Configures a complex type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="configure"></param>
		IDataQueriesOptionsBuilder ComplexType<T>(Action<IComplexTypeOptionsBuilder<T>> configure = null) where T : class;

		/// <summary>
		///		Configures an entity set.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <param name="entityTypeName"></param>
		/// <param name="configure"></param>
		IDataQueriesOptionsBuilder EntitySet<T>(string name, string entityTypeName, Action<IEntityTypeOptionsBuilder<T>> configure = null) where T : class;

		/// <summary>
		///		Configures an entity set.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <param name="configure"></param>
		IDataQueriesOptionsBuilder EntitySet<T>(string name, Action<IEntityTypeOptionsBuilder<T>> configure = null) where T : class;
	}
}
