namespace Fluxera.Queries
{
	using System;
	using JetBrains.Annotations;

	/// <summary>
	///		A contract for a service that creates query executor instances.
	/// </summary>
	[PublicAPI]
	public interface IQueryExecutorFactory
	{
		///  <summary>
		/// 	Creates a <see cref="IQueryExecutor"/> instance.
		///  </summary>
		///  <param name="entityType"></param>
		///  <param name="keyType"></param>
		///  <returns></returns>
		IQueryExecutor Create(Type entityType, Type keyType);
	}
}
