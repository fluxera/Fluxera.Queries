namespace Fluxera.Queries.Model
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Xml.Linq;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	/// <summary>
	///     An abstract base class for the EDM representation of a <see cref="Type" />.
	/// </summary>
	[PublicAPI]
	[DebuggerDisplay("{FullName}: {ClrType}")]
	public abstract class EdmType : IEquatable<EdmType>
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="EdmType" /> type.
		/// </summary>
		/// <param name="name">The name of the EDM type.</param>
		/// <param name="fullName">The full name of the EDM type.</param>
		/// <param name="clrType">The underlying CLR type.</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		protected EdmType(string name, string fullName, Type clrType)
		{
			this.Name = Guard.Against.NullOrWhiteSpace(name);
			this.FullName = Guard.Against.NullOrWhiteSpace(fullName);
			this.ClrType = Guard.Against.Null(clrType);
		}

		/// <summary>
		///     Gets the underlying CLR type.
		/// </summary>
		public Type ClrType { get; }

		/// <summary>
		///     Gets the full name of the EDM type.
		/// </summary>
		public string FullName { get; private set; }

		/// <summary>
		///     Gets the name of the EDM type.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		///		Gets the type the CRL type was redirected from.
		///		This is the case for strongly-typed IDs, enumerations and primitive value objects.
		/// </summary>
		internal Type RedirectedFromType { get; set; }

		internal void Rename(string edmTypeName)
		{
			this.Name = Guard.Against.NullOrWhiteSpace(edmTypeName);

			IList<string> parts = this.FullName.Split('.')[..^1].ToList();
			parts.Add(this.Name);

			this.FullName = parts.Aggregate((s1, s2) => string.Concat(s1, '.', s2));
		}

		/// <inheritdoc />
		public bool Equals(EdmType other)
		{
			if(other == null)
			{
				return false;
			}

			if(ReferenceEquals(this, other))
			{
				return true;
			}

			return this.ClrType == other.ClrType;
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			return this.Equals(obj as EdmType);
		}

		/// <summary>
		///		The equals operator overload.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator ==(EdmType a, EdmType b)
		{
			if(a is null)
			{
				return b is null;
			}

			if(b is null)
			{
				return false;
			}

			if(a.GetType() != b.GetType())
			{
				return false;
			}

			return a.Equals(b);
		}

		/// <summary>
		///		The not-equals operator overload.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator !=(EdmType a, EdmType b)
		{
			if(a is null)
			{
				return b is not null;
			}

			if(b is null)
			{
				return true;
			}

			if(a.GetType() != b.GetType())
			{
				return true;
			}

			return !a.Equals(b);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return this.ClrType.GetHashCode();
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return this.FullName;
		}
	}
}
