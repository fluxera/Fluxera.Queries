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
		private SkipQueryOption skip;
		private TopQueryOption top;
		private CountQueryOption count;

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

		/// <summary>
		///     Gets the skip query option.
		/// </summary>
		public SkipQueryOption Skip
		{
			get
			{
				if(this.skip == null && this.parameters.Skip != null)
				{
					this.skip = new SkipQueryOption(this.parameters.Skip);
				}

				return this.skip;
			}
		}

		/// <summary>
		///     Gets the top query option.
		/// </summary>
		public TopQueryOption Top
		{
			get
			{
				if(this.top == null && this.parameters.Top != null)
				{
					this.top = new TopQueryOption(this.parameters.Top);
				}

				return this.top;
			}
		}

		/// <summary>
		///     Gets the count query option.
		/// </summary>
		public CountQueryOption Count
		{
			get
			{
				if(this.count == null && this.parameters.Count != null)
				{
					this.count = new CountQueryOption(this.parameters.Count);
				}

				return this.count;
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

			if(this.Skip is not null)
			{
				builder.Append($"{this.Skip}; ");
			}

			if(this.Top is not null)
			{
				builder.Append($"{this.Top}; ");
			}

			if(this.Count is not null)
			{
				builder.Append($"{this.Count}; ");
			}

			//if(this.Search is not null)
			//{
			//	builder.Append($"Search={this.Search}; ");
			//}

			//if(this.SkipToken is not null)
			//{
			//	builder.Append($"SkipToken={this.SkipToken}; ");
			//}

			return builder.ToString().TrimEnd();
		}
	}
}
