namespace Fluxera.Queries.Parsers
{
	using System;
	using System.Collections.Generic;
	using Fluxera.Queries.Model;
	using Fluxera.Queries.Nodes;
	using Fluxera.StronglyTypedId;
	using Fluxera.ValueObject;

	internal static class FilterExpressionParser
	{
		public static QueryNode Parse(string expression, EdmComplexType model, IEdmTypeProvider typeProvider)
		{
			FilterExpressionParserImpl parserImpl = new FilterExpressionParserImpl(model, typeProvider);
			QueryNode queryNode = parserImpl.Parse(new FilterExpressionLexer(expression));

			return queryNode;
		}

		private sealed class FilterExpressionParserImpl
		{
			private readonly EdmComplexType model;
			private readonly Stack<QueryNode> nodeStack = new Stack<QueryNode>();
			private readonly Queue<Token> tokens = new Queue<Token>();
			private readonly IEdmTypeProvider typeProvider;
			private int groupingDepth;
			private BinaryOperatorKind nextBinaryOperatorKind = BinaryOperatorKind.None;

			internal FilterExpressionParserImpl(EdmComplexType model, IEdmTypeProvider typeProvider)
			{
				this.model = model;
				this.typeProvider = typeProvider;
			}

			internal QueryNode Parse(FilterExpressionLexer filterExpressionLexer)
			{
				while(filterExpressionLexer.MoveNext())
				{
					Token token = filterExpressionLexer.Current;

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
					throw new QueryException(Messages.UnableToParseFilter);
				}

				QueryNode node = this.nodeStack.Pop();

				if(node is BinaryOperatorNode binaryNode && (binaryNode.Left == null || binaryNode.Right == null))
				{
					throw new QueryException(Messages.UnableToParseFilter);
				}

				return node;
			}

			private QueryNode ParseFunctionCallNode()
			{
				BinaryOperatorNode binaryNode = null;
				FunctionCallNode node = null;

				Stack<FunctionCallNode> stack = new Stack<FunctionCallNode>();

				while(this.tokens.Count > 0)
				{
					Token token = this.tokens.Dequeue();

					switch(token.TokenType)
					{
						case TokenType.OpenParentheses:
							if(this.tokens.Peek().TokenType == TokenType.CloseParentheses)
							{
								// All OData functions have at least 1 or 2 parameters
								throw new QueryException(Messages.UnableToParseFilter);
							}

							this.groupingDepth++;
							stack.Push(node);
							break;

						case TokenType.CloseParentheses:
							if(this.groupingDepth == 0)
							{
								throw new QueryException(Messages.UnableToParseFilter);
							}

							this.groupingDepth--;

							if(stack.Count > 0)
							{
								FunctionCallNode lastNode = stack.Pop();

								if(stack.Count > 0)
								{
									stack.Peek().AddParameter(lastNode);
								}
								else
								{
									if(binaryNode != null)
									{
										binaryNode.Right = lastNode;
									}
									else
									{
										node = lastNode;
									}
								}
							}

							break;

						case TokenType.FunctionName:
							node = new FunctionCallNode(token.Value);
							break;

						case TokenType.BinaryOperator:
							binaryNode = new BinaryOperatorNode(node, token.Value.ToBinaryOperatorKind(), null);
							break;

						case TokenType.PropertyName:

							PropertyAccessNode propertyAccessNode = CreatePropertyAccessNode(token.Value, this.model);

							if(stack.Count > 0)
							{
								stack.Peek().AddParameter(propertyAccessNode);
							}
							else
							{
								if(binaryNode == null)
								{
									throw new InvalidOperationException("binaryNode is null in TokenType.PropertyName");
								}

								binaryNode.Right = propertyAccessNode;
							}

							break;

						case TokenType.Date:
						case TokenType.DateTimeOffset:
						case TokenType.Decimal:
						case TokenType.Double:
						case TokenType.Duration:
						case TokenType.Enum:
						case TokenType.False:
						case TokenType.Guid:
						case TokenType.Integer:
						case TokenType.Null:
						case TokenType.Single:
						case TokenType.String:
						case TokenType.TimeOfDay:
						case TokenType.True:
							ConstantNode constantNode = ConstantNodeParser.ParseConstantNode(token, this.typeProvider);

							if(stack.Count > 0)
							{
								stack.Peek().AddParameter(constantNode);
							}
							else
							{
								if(binaryNode == null)
								{
									throw new InvalidOperationException("binaryNode is null in TokenType.True");
								}

								binaryNode.Right = constantNode;
							}

							break;

						case TokenType.Comma:
							if(this.tokens.Count < 2)
							{
								// If there is a comma in a function call, there should be another argument followed by a closing comma
								throw new QueryException(Messages.UnableToParseFilter);
							}

							break;
					}
				}

				if(binaryNode != null)
				{
					return binaryNode;
				}

				return node;
			}

			private QueryNode ParsePropertyAccessNode()
			{
				QueryNode result = null;

				QueryNode leftNode = null;
				BinaryOperatorKind operatorKind = BinaryOperatorKind.None;
				QueryNode rightNode = null;

				while(this.tokens.Count > 0)
				{
					Token token = this.tokens.Dequeue();

					switch(token.TokenType)
					{
						case TokenType.BinaryOperator:
							if(operatorKind != BinaryOperatorKind.None)
							{
								result = new BinaryOperatorNode(leftNode, operatorKind, rightNode);
								leftNode = null;
								rightNode = null;
							}

							operatorKind = token.Value.ToBinaryOperatorKind();
							break;

						case TokenType.OpenParentheses:
							this.groupingDepth++;
							break;

						case TokenType.CloseParentheses:
							this.groupingDepth--;
							break;

						case TokenType.FunctionName:

							if(leftNode == null)
							{
								leftNode = new FunctionCallNode(token.Value);
							}
							else if(rightNode == null)
							{
								rightNode = new FunctionCallNode(token.Value);
							}

							break;

						case TokenType.PropertyName:
							PropertyAccessNode propertyAccessNode = CreatePropertyAccessNode(token.Value, this.model);

							if(leftNode == null)
							{
								leftNode = propertyAccessNode;
							}
							else if(rightNode == null)
							{
								rightNode = propertyAccessNode;
							}

							break;

						case TokenType.Date:
						case TokenType.DateTimeOffset:
						case TokenType.Decimal:
						case TokenType.Double:
						case TokenType.Duration:
						case TokenType.Enum:
						case TokenType.False:
						case TokenType.Guid:
						case TokenType.Integer:
						case TokenType.Null:
						case TokenType.Single:
						case TokenType.String:
						case TokenType.TimeOfDay:
						case TokenType.True:
							ConstantNode constantNode = ConstantNodeParser.ParseConstantNode(token, this.typeProvider);

							if(rightNode is ConstantNode existingConstant)
							{
								ArrayNode arrayNode = ConstantNode.Array(existingConstant);
								arrayNode.AddElement(constantNode);
								rightNode = arrayNode;
							}
							else if(rightNode is ArrayNode arrayNode)
							{
								arrayNode.AddElement(constantNode);
							}
							else
							{
								rightNode = constantNode;
							}

							break;
					}
				}

				result = result == null
					? new BinaryOperatorNode(leftNode, operatorKind, rightNode)
					: new BinaryOperatorNode(result, operatorKind, leftNode ?? rightNode);

				return result;
			}

			private static PropertyAccessNode CreatePropertyAccessNode(string tokenValue, EdmComplexType edmComplexType)
			{
				IEnumerable<EdmProperty> properties =
					PropertyParseHelper.ParseNestedProperties(tokenValue, edmComplexType);

				PropertyAccessNode propertyAccessNode = new PropertyAccessNode(properties);
				return propertyAccessNode;
			}

			private QueryNode ParseQueryNode()
			{
				QueryNode node = null;

				if(this.tokens.Count == 0)
				{
					throw new QueryException(Messages.UnableToParseFilter);
				}

				switch(this.tokens.Peek().TokenType)
				{
					case TokenType.FunctionName:
						node = this.ParseFunctionCallNode();
						break;

					case TokenType.UnaryOperator:
						Token token = this.tokens.Dequeue();
						node = this.ParseQueryNode();
						node = new UnaryOperatorNode(node, token.Value.ToUnaryOperatorKind());
						break;

					case TokenType.OpenParentheses:
						this.groupingDepth++;
						this.tokens.Dequeue();
						node = this.ParseQueryNode();
						break;

					case TokenType.PropertyName:
						node = this.ParsePropertyAccessNode();
						break;

					case TokenType.True:
						node = ConstantNode.True;
						break;

					case TokenType.False:
						node = ConstantNode.False;
						break;

					default:
						throw new QueryException(this.tokens.Peek().TokenType.ToString());
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
