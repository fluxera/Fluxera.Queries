namespace Fluxera.Queries.AspNetCore
{
	using System;

	internal static class TypeExtensions
	{
		public static bool IsDataQuery(this Type bindingContextModelType)
		{
			if(bindingContextModelType is null)
			{
				return false;
			}

			return
				bindingContextModelType.IsGenericType &&
				bindingContextModelType.GetGenericTypeDefinition() == typeof(DataQuery<>);
		}
	}
}
