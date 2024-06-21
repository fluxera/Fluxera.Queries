namespace Fluxera.Queries
{
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	/// <summary>
	///		A contract for a class that provides a predicate that is used with search parameter.
	/// </summary>
	[PublicAPI]
	public interface ISearchPredicateProvider
	{
		/// <summary>
		///		Gets the search predicate.
		/// </summary>
		LambdaExpression SearchPredicate { get; }
	}
}
