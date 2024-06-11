namespace Fluxera.Queries.AspNetCore.Options
{
	using System.Collections.Generic;
	using System.Reflection;
	using System;
	using System.Linq;

	/// <summary>
	///		The options for a complex type.
	/// </summary>
	internal sealed class ComplexTypeOptions
	{
		private readonly IDictionary<string, PropertyOptions> properties = new Dictionary<string, PropertyOptions>();

		/// <summary>
		///		Gets the underlying CLR type.
		/// </summary>
		public Type ClrType { get; set; }

		/// <summary>
		///		Gets the alternate type name to use.
		/// </summary>
		public string TypeName { get; set; }

		/// <summary>
		///		Gets the properties to ignore.
		/// </summary>
		public IList<PropertyInfo> IgnoredProperties => this.properties
			.Values
			.Where(x => x.IsIgnored)
			.Select(x => x.PropertyInfo).ToList();

		public void IgnoreProperty(PropertyInfo propertyInfo)
		{
			if(!this.properties.ContainsKey(propertyInfo.Name))
			{
				this.properties.Add(propertyInfo.Name, new PropertyOptions(propertyInfo));
			}

			this.properties[propertyInfo.Name].IsIgnored = true;
		}
	}
}
