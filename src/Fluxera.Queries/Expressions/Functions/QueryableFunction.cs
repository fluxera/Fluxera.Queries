namespace Fluxera.Queries.Expressions.Functions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;

	internal abstract class QueryableFunction
	{
		public abstract string FunctionName { get; }

		protected void ValidateParameterCount(IList<Expression> arguments, params int[] count)
		{
			if(count.Length == 1 && arguments.Count != count[0])
			{
				throw new InvalidOperationException($"{this.FunctionName} needs {count[0]} parameters.");
			}

			if(!count.Contains(arguments.Count))
			{
				string counts = string.Join(", ", count.Take(count.Length - 1)) + " or " + count.Last();
				throw new InvalidOperationException($"{this.FunctionName} needs {counts} parameters.");
			}
		}

		public abstract Expression CreateExpression(IList<Expression> arguments);

		protected Expression InvalidParameterTypes(string supportedTypes)
		{
			throw new InvalidOperationException(
				$"Unsupported parameters provided to function '{this.FunctionName}', supported types: {supportedTypes}.");
		}
	}
}
