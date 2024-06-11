namespace Fluxera.Queries.AspNetCore.Options
{
	using System.Reflection;

	/// <summary>
	///		The options for a property.
	/// </summary>
	internal sealed class PropertyOptions
	{
		public PropertyOptions(PropertyInfo propertyInfo)
		{
			this.PropertyInfo = propertyInfo;
		}

		/// <summary>
		///		Gets the property info.
		/// </summary>
		public PropertyInfo PropertyInfo { get; set; }

		/// <summary>
		///		The property is ignored in the EDM.
		/// </summary>
		public bool IsIgnored { get; set; }
	}
}
