namespace Fluxera.Queries.UnitTests.Parser
{
	using Fluxera.Queries.Model;
	using NUnit.Framework;
	using System;
	using FluentAssertions;
	using Fluxera.Queries.Options;

	[TestFixture]
	public class QueryParserTests
	{
		[Test]
		public void ShouldThrowWhen_EntityTypeNull()
		{
			IQueryParser queryParser = new QueryParser(new EdmTypeProvider());

			Action action = () => queryParser.ParseQueryOptions(null, "$filter=FirstName eq 'James'");

			action.Should().Throw<ArgumentNullException>();
		}

		[Test]
		public void ShouldThrowWhen_QueryStringNull()
		{
			IEdmTypeProvider typeProvider = new EdmTypeProvider();
			IQueryParser queryParser = new QueryParser(typeProvider);

			EdmComplexType edmType = (EdmComplexType)typeProvider.GetByClrType(typeof(Customer));
			EntitySet entitySet = new EntitySet("Customers", edmType);

			Action action = () => queryParser.ParseQueryOptions(entitySet, null);

			action.Should().Throw<ArgumentNullException>();
		}

		[Test]
		[TestCase("?$filter=FirstName eq 'James'")]
		[TestCase("$filter=FirstName eq 'James'")]
		public void ShouldParseFilterOptions(string queryString)
		{
			IEdmTypeProvider typeProvider = new EdmTypeProvider();
			IQueryParser queryParser = new QueryParser(typeProvider);

			EdmComplexType edmType = (EdmComplexType)typeProvider.GetByClrType(typeof(Customer));
			EntitySet entitySet = new EntitySet("Customers", edmType);

			QueryOptions queryOptions = queryParser.ParseQueryOptions(entitySet, queryString);

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
			IEdmTypeProvider typeProvider = new EdmTypeProvider();
			IQueryParser queryParser = new QueryParser(typeProvider);

			EdmComplexType edmType = (EdmComplexType)typeProvider.GetByClrType(typeof(Customer));
			EntitySet entitySet = new EntitySet("Customers", edmType);

			QueryOptions queryOptions = queryParser.ParseQueryOptions(entitySet, queryString);

			queryOptions.Should().NotBeNull();
			queryOptions.Filter.Should().BeNull();
		}

		[Test]
		[TestCase("?$orderby=Age desc")]
		[TestCase("$orderby=Age desc")]
		public void ShouldParseOrderByOptions(string queryString)
		{
			IEdmTypeProvider typeProvider = new EdmTypeProvider();
			IQueryParser queryParser = new QueryParser(typeProvider);

			EdmComplexType edmType = (EdmComplexType)typeProvider.GetByClrType(typeof(Customer));
			EntitySet entitySet = new EntitySet("Customers", edmType);

			QueryOptions queryOptions = queryParser.ParseQueryOptions(entitySet, queryString);

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
			IEdmTypeProvider typeProvider = new EdmTypeProvider();
			IQueryParser queryParser = new QueryParser(typeProvider);

			EdmComplexType edmType = (EdmComplexType)typeProvider.GetByClrType(typeof(Customer));
			EntitySet entitySet = new EntitySet("Customers", edmType);

			QueryOptions queryOptions = queryParser.ParseQueryOptions(entitySet, queryString);

			queryOptions.Should().NotBeNull();
			queryOptions.OrderBy.Should().BeNull();
		}
	}
}
