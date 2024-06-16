namespace Fluxera.Queries.Options
{
	using System;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	/// <summary>
	///		Build the options for a complex type.
	/// </summary>
	[PublicAPI]
	public interface IComplexTypeOptionsBuilder<T> where T : class
	{
		/// <summary>
		///		Ignore the property.
		/// </summary>
		/// <param name="propertySelector"></param>
		/// <returns></returns>
		IComplexTypeOptionsBuilder<T> Ignore<TValue>(Expression<Func<T, TValue>> propertySelector);
	}
}
