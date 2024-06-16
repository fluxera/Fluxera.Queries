namespace Fluxera.Queries.UnitTests.Parser
{
	using Fluxera.Queries.Model;
	using NUnit.Framework;
	using System;
	using FluentAssertions;
	using Fluxera.Queries.Options;
	using Microsoft.Extensions.DependencyInjection;

	[TestFixture]
	public class QueryParserTests
	{
		private IQueryParser queryParser;
		private IEdmTypeProvider typeProvider;

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			IServiceCollection services = new ServiceCollection();

			services.AddDataQueries(builder =>
			{
				builder.EntitySet<Customer>("Customers", entitySet =>
				{
					entitySet.HasKey(x => x.Id);
				});
			});

			IServiceProvider serviceProvider = services.BuildServiceProvider();

			this.queryParser = serviceProvider.GetRequiredService<IQueryParser>();
			this.typeProvider = serviceProvider.GetRequiredService<IEdmTypeProvider>();
		}

		[Test]
		public void ShouldThrowWhen_EntityTypeNull()
		{
			Action action = () => this.queryParser.ParseQueryOptions(null, "$filter=FirstName eq 'James'");

			action.Should().Throw<ArgumentNullException>();
		}

		[Test]
		public void ShouldThrowWhen_QueryStringNull()
		{
			EdmComplexType edmType = (EdmComplexType)this.typeProvider.GetByType(typeof(Customer));
			EntitySet entitySet = new EntitySet("Customers", edmType);

			Action action = () => this.queryParser.ParseQueryOptions(entitySet, null);

			action.Should().Throw<ArgumentNullException>();
		}

		[Test]
		[TestCase("?$filter=FirstName eq 'James'")]
		[TestCase("$filter=FirstName eq 'James'")]
		public void ShouldParseFilterOptions(string queryString)
		{
			EdmComplexType edmType = (EdmComplexType)this.typeProvider.GetByType(typeof(Customer));
			EntitySet entitySet = new EntitySet("Customers", edmType);

			QueryOptions queryOptions = this.queryParser.ParseQueryOptions(entitySet, queryString);

			queryOptions.Should().NotBeNull();
			queryOptions.Filter.Should().NotBeNull();
			queryOptions.Filter.Expression.Should().NotBeNull();
			queryOptions.Filter.StringExpression.Should().Be("FirstName eq 'James'");
		}

		[Test]
		[TestCase("?")]
		[TestCase("")]
		public void ShouldNotParseFilterOptions(string queryString)
		{
			EdmComplexType edmType = (EdmComplexType)this.typeProvider.GetByType(typeof(Customer));
			EntitySet entitySet = new EntitySet("Customers", edmType);

			QueryOptions queryOptions = this.queryParser.ParseQueryOptions(entitySet, queryString);

			queryOptions.Should().NotBeNull();
			queryOptions.Filter.Should().BeNull();
		}

		[Test]
		[TestCase("?$orderby=Age desc")]
		[TestCase("$orderby=Age desc")]
		public void ShouldParseOrderByOptions(string queryString)
		{
			EdmComplexType edmType = (EdmComplexType)this.typeProvider.GetByType(typeof(Customer));
			EntitySet entitySet = new EntitySet("Customers", edmType);

			QueryOptions queryOptions = this.queryParser.ParseQueryOptions(entitySet, queryString);

			queryOptions.Should().NotBeNull();
			queryOptions.OrderBy.Should().NotBeNull();
			queryOptions.OrderBy.Properties.Should().NotBeNull().And.HaveCount(1);
			queryOptions.OrderBy.StringExpression.Should().Be("Age desc");
		}

		[Test]
		[TestCase("?")]
		[TestCase("")]
		public void ShouldNotParseOrderByOptions(string queryString)
		{
			EdmComplexType edmType = (EdmComplexType)this.typeProvider.GetByType(typeof(Customer));
			EntitySet entitySet = new EntitySet("Customers", edmType);

			QueryOptions queryOptions = this.queryParser.ParseQueryOptions(entitySet, queryString);

			queryOptions.Should().NotBeNull();
			queryOptions.OrderBy.Should().BeNull();
		}
	}
}
