namespace Fluxera.Queries.AspNetCore
{
	using JetBrains.Annotations;
	using System.Collections.Generic;
	using System.Reflection;
	using System;

	/// <summary>
	///		The options for a complex type.
	/// </summary>
	[PublicAPI]
	public sealed class ComplexTypeOptions
	{
		private readonly IList<PropertyInfo> ignoredProperties = new List<PropertyInfo>();

		/// <summary>
		///		Gets the underlying CLR type.
		/// </summary>
		internal Type ClrType { get; set; }

		/// <summary>
		///		Gets the alternate type name to use.
		/// </summary>
		internal string TypeName { get; set; }

		/// <summary>
		///		Gets the properties to ignore.
		/// </summary>
		internal IList<PropertyInfo> IgnoredProperties => this.ignoredProperties;

		internal void IgnoreProperty(PropertyInfo propertyInfo)
		{
			if(!this.ignoredProperties.Contains(propertyInfo))
			{
				this.ignoredProperties.Add(propertyInfo);
			}
		}
	}
}
