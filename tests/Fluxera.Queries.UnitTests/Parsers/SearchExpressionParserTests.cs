namespace Fluxera.Queries.UnitTests.Parsers
{
	using FluentAssertions;
	using Fluxera.Queries.Nodes;
	using Fluxera.Queries.Parsers;
	using NUnit.Framework;

	[TestFixture]
	public class SearchExpressionParserTests
	{
		[Test]
		[TestCase("$search=food")]
		[TestCase("$search= food ")]
		[TestCase("$search=\"food\"")]
		public void ShouldParseSearchWithSimpleWord(string expression)
		{
			QueryNode queryNode = SearchExpressionParser.Parse(expression);

			queryNode.Should().BeOfType<ConstantNode>();
			queryNode.Kind.Should().Be(QueryNodeKind.Constant);

			ConstantNode constantNode = queryNode as ConstantNode;
			constantNode.Value.Should().Be("food");
		}

		[Test]
		[TestCase("$search=\"food with something\"")]
		public void ShouldParseSearchWithSimpleString(string expression)
		{
			QueryNode queryNode = SearchExpressionParser.Parse(expression);

			queryNode.Should().BeOfType<ConstantNode>();
			queryNode.Kind.Should().Be(QueryNodeKind.Constant);

			ConstantNode constantNode = queryNode as ConstantNode;
			constantNode.Value.Should().Be("food with something");
		}

		[Test]
		[TestCase("$search=NOT food")]
		[TestCase("$search=NOT \"food\"")]
		public void ShouldParseSearchWithNot(string expression)
		{
			QueryNode queryNode = SearchExpressionParser.Parse(expression);

			queryNode.Should().BeOfType<UnaryOperatorNode>();
			queryNode.Kind.Should().Be(QueryNodeKind.UnaryOperator);

			UnaryOperatorNode unaryOperatorNode = queryNode as UnaryOperatorNode;
			unaryOperatorNode.OperatorKind.Should().Be(UnaryOperatorKind.Not);

			unaryOperatorNode.Operand.Should().NotBeNull();
			unaryOperatorNode.Operand.Should().BeOfType<ConstantNode>();
			unaryOperatorNode.Operand.Kind.Should().Be(QueryNodeKind.Constant);

			ConstantNode constantNode = unaryOperatorNode.Operand as ConstantNode;
			constantNode.Value.Should().Be("food");
		}

		[Test]
		[TestCase("$search=food AND drink")]
		[TestCase("$search=\"food\" AND \"drink\"")]
		public void ShouldParseSearchWithAnd(string expression)
		{
			QueryNode queryNode = SearchExpressionParser.Parse(expression);

			queryNode.Should().BeOfType<BinaryOperatorNode>();
			queryNode.Kind.Should().Be(QueryNodeKind.BinaryOperator);

			BinaryOperatorNode binaryOperatorNode = queryNode as BinaryOperatorNode;
			binaryOperatorNode.OperatorKind.Should().Be(BinaryOperatorKind.And);

			binaryOperatorNode.Left.Should().NotBeNull();
			binaryOperatorNode.Left.Kind.Should().Be(QueryNodeKind.Constant);
			ConstantNode leftConstantNode = binaryOperatorNode.Left as ConstantNode;
			leftConstantNode.Value.Should().Be("food");
			
			binaryOperatorNode.Right.Should().NotBeNull();
			binaryOperatorNode.Right.Kind.Should().Be(QueryNodeKind.Constant);
			ConstantNode rightConstantNode = binaryOperatorNode.Right as ConstantNode;
			rightConstantNode.Value.Should().Be("drink");
		}

		[Test]
		[TestCase("$search=food OR drink")]
		[TestCase("$search=\"food\" OR \"drink\"")]
		public void ShouldParseSearchWithOr(string expression)
		{
			QueryNode queryNode = SearchExpressionParser.Parse(expression);

			queryNode.Should().BeOfType<BinaryOperatorNode>();
			queryNode.Kind.Should().Be(QueryNodeKind.BinaryOperator);

			BinaryOperatorNode binaryOperatorNode = queryNode as BinaryOperatorNode;
			binaryOperatorNode.OperatorKind.Should().Be(BinaryOperatorKind.Or);

			binaryOperatorNode.Left.Should().NotBeNull();
			binaryOperatorNode.Left.Kind.Should().Be(QueryNodeKind.Constant);
			ConstantNode leftConstantNode = binaryOperatorNode.Left as ConstantNode;
			leftConstantNode.Value.Should().Be("food");

			binaryOperatorNode.Right.Should().NotBeNull();
			binaryOperatorNode.Right.Kind.Should().Be(QueryNodeKind.Constant);
			ConstantNode rightConstantNode = binaryOperatorNode.Right as ConstantNode;
			rightConstantNode.Value.Should().Be("drink");
		}

		[Test]
		[TestCase("$search=food OR drink AND cheese OR wine")]
		[TestCase("$search=\"food\" OR \"drink\" AND \"cheese\" OR \"wine\"")]
		[TestCase("$search=(food OR drink) AND (cheese OR wine)")]
		[TestCase("$search=(\"food\" OR \"drink\") AND (\"cheese\" OR \"wine\")")]
		[TestCase("$search=(cheese AND wine) OR beer")]
		[TestCase("$search=cheese AND (wine OR beer)")]
		public void ShouldParseSearchWithComplexExpression(string expression)
		{
			QueryNode queryNode = SearchExpressionParser.Parse(expression);

			queryNode.Should().BeOfType<BinaryOperatorNode>();
			queryNode.Kind.Should().Be(QueryNodeKind.BinaryOperator);
		}
	}
}
