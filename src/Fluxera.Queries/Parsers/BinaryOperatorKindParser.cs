﻿namespace Fluxera.Queries.Parsers
{
	using System.Collections.Generic;
	using Fluxera.Queries.Nodes;

	internal static class BinaryOperatorKindParser
	{
		private static readonly Dictionary<string, BinaryOperatorKind> OperatorTypeMap = new Dictionary<string, BinaryOperatorKind>
		{
			["add"] = BinaryOperatorKind.Add,
			["and"] = BinaryOperatorKind.And,
			["div"] = BinaryOperatorKind.Divide,
			["eq"] = BinaryOperatorKind.Equal,
			["ge"] = BinaryOperatorKind.GreaterThanOrEqual,
			["gt"] = BinaryOperatorKind.GreaterThan,
			["has"] = BinaryOperatorKind.Has,
			["le"] = BinaryOperatorKind.LessThanOrEqual,
			["lt"] = BinaryOperatorKind.LessThan,
			["mul"] = BinaryOperatorKind.Multiply,
			["mod"] = BinaryOperatorKind.Modulo,
			["ne"] = BinaryOperatorKind.NotEqual,
			["or"] = BinaryOperatorKind.Or,
			["sub"] = BinaryOperatorKind.Subtract,
			["in"] = BinaryOperatorKind.In
		};

		public static BinaryOperatorKind ToBinaryOperatorKind(this string operatorType)
		{
			if(OperatorTypeMap.TryGetValue(operatorType, out BinaryOperatorKind binaryOperatorKind))
			{
				return binaryOperatorKind;
			}

			throw new QueryException(string.Format(Messages.UnknownOperator, operatorType));
		}
	}
}
