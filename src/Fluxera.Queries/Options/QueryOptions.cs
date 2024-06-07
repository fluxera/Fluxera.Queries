namespace Fluxera.Queries.Options
{
	using JetBrains.Annotations;
	using System.Text;
	using Fluxera.Queries.Model;
	using Fluxera.Guards;

	/// <summary>
	///     An object which contains the parsed query options.
	/// </summary>
	[PublicAPI]
	public sealed class QueryOptions
	{
		private FilterQueryOption filter;
		private OrderByQueryOption orderBy;

		private readonly EdmTypeProvider typeProvider;
		private readonly QueryStringParameters parameters;

		/// <summary>
		///     Initializes a new instance of the <see cref="QueryOptions" /> type.
		/// </summary>
		/// <param name="parameters">The query string parameters of the request.</param>
		/// <param name="entitySet">The entity set of the query.</param>
		/// <param name="typeProvider">The type provider.</param>
		internal QueryOptions(QueryStringParameters parameters, EntitySet entitySet, EdmTypeProvider typeProvider)
		{
			this.parameters = parameters;
			this.typeProvider = typeProvider;

			this.EntitySet = Guard.Against.Null(entitySet);
		}

		/// <summary>
		///     Gets the entity set.
		/// </summary>
		public EntitySet EntitySet { get; }

		/// <summary>
		///     Gets the filter query option.
		/// </summary>
		public FilterQueryOption Filter
		{
			get
			{
				if(this.filter == null && this.parameters.Filter != null)
				{
					this.filter = new FilterQueryOption(this.parameters.Filter, this.EntitySet.EdmType, this.typeProvider);
				}

				return this.filter;
			}
		}

		/// <summary>
		///     Gets the order by query option.
		/// </summary>
		public OrderByQueryOption OrderBy
		{
			get
			{
				if(this.orderBy == null && this.parameters.OrderBy != null)
				{
					this.orderBy = new OrderByQueryOption(this.parameters.OrderBy, this.EntitySet.EdmType);
				}

				return this.orderBy;
			}
		}

		/// <inheritdoc />
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder($"QueryOptions[{this.EntitySet.EdmType.Name}]: ");

			if(this.Filter is not null)
			{
				builder.Append($"{this.Filter}; ");
			}

			if(this.OrderBy is not null)
			{
				builder.Append($"{this.OrderBy}; ");
			}

			//if(this.Search is not null)
			//{
			//	builder.Append($"Search={this.Search}; ");
			//}

			//if(this.Skip is not null)
			//{
			//	builder.Append($"Skip={this.Skip}; ");
			//}

			//if(this.Top is not null)
			//{
			//	builder.Append($"Top={this.Top}; ");
			//}

			//if(this.SkipToken is not null)
			//{
			//	builder.Append($"SkipToken={this.SkipToken}; ");
			//}

			//builder.Append($"ShowCount={this.ShowCount}");

			return builder.ToString();
		}
	}
}
