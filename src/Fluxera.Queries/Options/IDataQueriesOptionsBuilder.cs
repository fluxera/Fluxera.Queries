namespace Fluxera.Queries.Options
{
	using System;
	using JetBrains.Annotations;

	/// <summary>
	///		Build the options for the data queries.
	/// </summary>
	[PublicAPI]
	public interface IDataQueriesOptionsBuilder
	{
		/// <summary>
		///		Configures an entity set.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <param name="entityTypeName"></param>
		/// <param name="configure"></param>
		IEntitySetOptionsBuilder<T> EntitySet<T>(string name, string entityTypeName, Action<IEntityTypeOptionsBuilder<T>> configure = null) where T : class;

		/// <summary>
		///		Configures an entity set.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <param name="configure"></param>
		IEntitySetOptionsBuilder<T> EntitySet<T>(string name, Action<IEntityTypeOptionsBuilder<T>> configure = null) where T : class;

		/// <summary>
		///		Configures a complex type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="complexTypeName"></param>
		/// <param name="configure"></param>
		void ComplexType<T>(string complexTypeName, Action<IComplexTypeOptionsBuilder<T>> configure = null) where T : class;

		/// <summary>
		///		Configures a complex type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="configure"></param>
		void ComplexType<T>(Action<IComplexTypeOptionsBuilder<T>> configure = null) where T : class;
	}
}
