namespace Fluxera.Queries.Parsers
{
	using System;
	using System.Collections.Generic;
	using Fluxera.Queries.Nodes;

	internal static class SearchExpressionParser
	{
		public static QueryNode Parse(string expression)
		{
			SearchExpressionParserImpl parserImpl = new SearchExpressionParserImpl();
			QueryNode queryNode = parserImpl.Parse(new SearchExpressionLexer(expression));

			return queryNode;
		}

		private static QueryNode ParseSearchString(string value)
		{
			string stringText = UnescapeStringText(value);
			return ConstantNode.String(value, stringText);
		}

		private static string UnescapeStringText(string tokenValue)
		{
			return tokenValue
				.Trim('\"')
				.Replace("\\\"", "\"")
				.Replace("%2B", "+")
				.Replace("%2F", "/")
				.Replace("%3F", "?")
				.Replace("%25", "%")
				.Replace("%23", "#")
				.Replace("%26", "&");
		}

		private sealed class SearchExpressionParserImpl
		{
			private readonly Stack<QueryNode> nodeStack = new Stack<QueryNode>();
			private readonly Queue<Token> tokens = new Queue<Token>();
			private int groupingDepth;
			private BinaryOperatorKind nextBinaryOperatorKind = BinaryOperatorKind.None;

			public QueryNode Parse(SearchExpressionLexer searchExpressionLexer)
			{
				while(searchExpressionLexer.MoveNext())
				{
					Token token = searchExpressionLexer.Current;

					switch(token.TokenType)
					{
						case TokenType.And:
							this.nextBinaryOperatorKind = BinaryOperatorKind.And;
							this.UpdateExpressionTree();
							break;

						case TokenType.Or:
							this.nextBinaryOperatorKind = BinaryOperatorKind.Or;
							this.UpdateExpressionTree();
							break;

						default:
							this.tokens.Enqueue(token);
							break;
					}
				}

				this.nextBinaryOperatorKind = BinaryOperatorKind.None;
				this.UpdateExpressionTree();

				if(this.groupingDepth != 0 || this.nodeStack.Count != 1)
				{
					throw new QueryException(Messages.UnableToParseSearch);
				}

				QueryNode node = this.nodeStack.Pop();

				if(node is BinaryOperatorNode binaryNode && (binaryNode.Left == null || binaryNode.Right == null))
				{
					throw new QueryException(Messages.UnableToParseSearch);
				}

				return node;
			}

			private QueryNode ParseQueryNode()
			{
				QueryNode node;

				if(this.tokens.Count == 0)
				{
					throw new QueryException(Messages.UnableToParseSearch);
				}

				switch(this.tokens.Peek().TokenType)
				{
					case TokenType.UnaryOperator:
						Token unaryToken = this.tokens.Dequeue();
						node = this.ParseQueryNode();
						node = new UnaryOperatorNode(node, unaryToken.Value.ToUnaryOperatorKind());
						break;

					case TokenType.OpenParentheses:
						this.groupingDepth++;
						this.tokens.Dequeue();
						node = this.ParseQueryNode();
						break;

					case TokenType.String:
						node = this.ParseStringNode();
						break;

					default:
						throw new QueryException(this.tokens.Peek().TokenType.ToString());
				}

				return node;
			}

			private QueryNode ParseStringNode()
			{
				QueryNode node = null;

				while(this.tokens.Count > 0)
				{
					Token token = this.tokens.Dequeue();

					switch(token.TokenType)
					{
						case TokenType.CloseParentheses:
							this.groupingDepth--;
							break;
						case TokenType.String:
							node = ParseSearchString(token.Value);
							break;
					}
				}

				return node;
			}

			private void UpdateExpressionTree()
			{
				int initialGroupingDepth = this.groupingDepth;

				QueryNode node = this.ParseQueryNode();

				if(this.groupingDepth == initialGroupingDepth)
				{
					if(this.nodeStack.Count == 0)
					{
						if(this.nextBinaryOperatorKind == BinaryOperatorKind.None)
						{
							this.nodeStack.Push(node);
						}
						else
						{
							this.nodeStack.Push(new BinaryOperatorNode(node, this.nextBinaryOperatorKind, null));
						}
					}
					else
					{
						QueryNode leftNode = this.nodeStack.Pop();

						if(leftNode is BinaryOperatorNode binaryNode && binaryNode.Right == null)
						{
							binaryNode.Right = node;

							if(this.nextBinaryOperatorKind != BinaryOperatorKind.None)
							{
								binaryNode = new BinaryOperatorNode(binaryNode, this.nextBinaryOperatorKind, null);
							}
						}
						else
						{
							binaryNode = new BinaryOperatorNode(leftNode, this.nextBinaryOperatorKind, node);
						}

						this.nodeStack.Push(binaryNode);
					}
				}
				else if(this.groupingDepth > initialGroupingDepth)
				{
					this.nodeStack.Push(new BinaryOperatorNode(node, this.nextBinaryOperatorKind, null));
				}
				else if(this.groupingDepth < initialGroupingDepth)
				{
					BinaryOperatorNode binaryNode = (BinaryOperatorNode)this.nodeStack.Pop();
					binaryNode.Right = node;

					if(this.nextBinaryOperatorKind == BinaryOperatorKind.None)
					{
						this.nodeStack.Push(binaryNode);

						while(this.nodeStack.Count > 1)
						{
							QueryNode rightNode = this.nodeStack.Pop();

							BinaryOperatorNode binaryParent = (BinaryOperatorNode)this.nodeStack.Pop();
							binaryParent.Right = rightNode;

							this.nodeStack.Push(binaryParent);
						}
					}
					else
					{
						if(this.groupingDepth == 0 && this.nodeStack.Count > 0)
						{
							BinaryOperatorNode binaryParent = (BinaryOperatorNode)this.nodeStack.Pop();
							binaryParent.Right = binaryNode;

							binaryNode = binaryParent;
						}

						this.nodeStack.Push(new BinaryOperatorNode(binaryNode, this.nextBinaryOperatorKind, null));
					}
				}
			}
		}
	}
}
