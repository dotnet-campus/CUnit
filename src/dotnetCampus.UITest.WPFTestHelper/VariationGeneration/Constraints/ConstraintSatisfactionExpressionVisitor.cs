// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

﻿
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Microsoft.Test.VariationGeneration.Constraints
{
    class ConstraintSatisfactionExpressionVisitor
    {
        Dictionary<Expression, CachedExpressionConstraintData> mapExpressionsToRequiredParams = new Dictionary<Expression, CachedExpressionConstraintData>();

        public ConstraintSatisfactionExpressionVisitor(Dictionary<Expression, CachedExpressionConstraintData> expressionData)
        {
            mapExpressionsToRequiredParams = expressionData;
        }

        public ConstraintSatisfaction SatisfiesConstraint<T>(Expression expression, Model<T> model, ValueCombination combination) where T : new()
        {
            switch(expression.NodeType)
            {
                case ExpressionType.AndAlso:
                    {
                        var andAlso = (BinaryExpression)expression;
                        ConstraintSatisfaction left = SatisfiesConstraint(andAlso.Left, model, combination);
                        if (left == ConstraintSatisfaction.Unsatisfied)
                        {
                            return ConstraintSatisfaction.Unsatisfied;
                        }

                        ConstraintSatisfaction right = SatisfiesConstraint(andAlso.Right, model, combination);
                        if (right == ConstraintSatisfaction.Unsatisfied)
                        {
                            return ConstraintSatisfaction.Unsatisfied;
                        }

                        if (left == right)
                        {
                            return left;
                        }
                        else
                        {
                            return ConstraintSatisfaction.InsufficientData;
                        }
                    }

                case ExpressionType.Conditional:
                    var conditional = (ConditionalExpression)expression;
                    ConstraintSatisfaction test = SatisfiesConstraint(conditional.Test, model, combination);
                    if (test == ConstraintSatisfaction.InsufficientData)
                    {
                        return ConstraintSatisfaction.InsufficientData;
                    }

                    if (test == ConstraintSatisfaction.Satisfied)
                    {
                        return SatisfiesConstraint(conditional.IfTrue, model, combination);
                    }
                    else
                    {
                        return SatisfiesConstraint(conditional.IfFalse, model, combination);
                    }
                case ExpressionType.Lambda:
                    return SatisfiesConstraint(((LambdaExpression)expression).Body, model, combination);
                case ExpressionType.OrElse:
                    {
                        var orElse = (BinaryExpression)expression;
                        ConstraintSatisfaction left = SatisfiesConstraint(orElse.Left, model, combination);
                        if (left == ConstraintSatisfaction.Satisfied)
                        {
                            return ConstraintSatisfaction.Satisfied;
                        }

                        ConstraintSatisfaction right = SatisfiesConstraint(orElse.Right, model, combination);
                        if (right == ConstraintSatisfaction.Satisfied)
                        {
                            return ConstraintSatisfaction.Satisfied;
                        }

                        if (left == right)
                        {
                            return left;
                        }
                        else
                        {
                            return ConstraintSatisfaction.InsufficientData;
                        }
                    }

                default:
                    if (!mapExpressionsToRequiredParams.ContainsKey(expression))
                    {
                        throw new InternalVariationGenerationException("Expected data for expression not found.");
                    }
                    return InternalConstraintHelpers.SatisfiesContraint(model, combination, mapExpressionsToRequiredParams[expression].Interaction);
            }
        }
    }
}
