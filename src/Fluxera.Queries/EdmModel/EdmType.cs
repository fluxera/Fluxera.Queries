namespace Fluxera.Queries.EdmModel
{
	using System;
	using System.Diagnostics;
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
			this.Name = Guard.ThrowIfNullOrWhiteSpace(name);
			this.FullName = Guard.ThrowIfNullOrWhiteSpace(fullName);
			this.ClrType = Guard.ThrowIfNull(clrType);
		}

		/// <summary>
		///     Gets the underlying CLR type.
		/// </summary>
		public Type ClrType { get; }

		/// <summary>
		///     Gets the full name of the EDM type.
		/// </summary>
		public string FullName { get; }

		/// <summary>
		///     Gets the name of the EDM type.
		/// </summary>
		public string Name { get; }

		/// <inheritdoc />
		public bool Equals(EdmType other)
		{
			if (other == null)
			{
				return false;
			}

			if (ReferenceEquals(this, other))
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
			if (a is null)
			{
				return b is null;
			}

			if (b is null)
			{
				return false;
			}

			if (a.GetType() != b.GetType())
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
			if (a is null)
			{
				return b is not null;
			}

			if (b is null)
			{
				return true;
			}

			if (a.GetType() != b.GetType())
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
