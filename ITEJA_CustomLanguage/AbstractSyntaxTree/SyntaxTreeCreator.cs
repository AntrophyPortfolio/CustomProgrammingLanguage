using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks;
using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses;
using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.BodyStatements;
using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.BodyStatements.Classes;
using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.Variables;
using ITEJA_CustomLanguage.Lexer;
using System;
using System.Collections.Generic;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree
{
    public class SyntaxTreeCreator : ISyntaxTreeCreator
    {
        private readonly Stack<Token> tokenStack;
        private readonly Stack<IBodyStatement> parentBodyStatements = new Stack<IBodyStatement>();
        public SyntaxTreeCreator(IEnumerable<Token> tokens)
        {
            tokenStack = new Stack<Token>(new Stack<Token>(tokens));
            CreateSyntaxTree();
        }
        private MainClass CreateSyntaxTree()
        {
            while (tokenStack.Count != 0)
            {
                Token newToken = tokenStack.Pop();
                if (newToken.Type == TokenType.IntegerDataType)
                {
                    IIntegerVariable integerVariable = CreateIntegerVariable();
                    DetermineGlobalOrLocalVariable(integerVariable);
                }
                else if (newToken.Type == TokenType.StringDataType)
                {
                    IStringVariable stringVariable = CreateStringVariable();
                    DetermineGlobalOrLocalVariable(stringVariable);
                }
                else if (IsMethodFound(newToken))
                {
                    CreateMethod(newToken);
                }
                else if (newToken.Type == TokenType.PrintLn)
                {
                    CreatePrintStatement();
                }
                else if (newToken.Type == TokenType.Identifier)
                {
                    CreateRedefinitionStatement(newToken);
                }
                else if (newToken.Type == TokenType.If)
                {
                    CreateConditionStatement();
                }
                else if (newToken.Type == TokenType.Run)
                {
                    CreateRunMethodStatement();
                }
                else if (IsElseStatementFound(newToken))
                {
                    CreateElseStatement();
                }
                else if (IsEndOfBodyStatement(newToken))
                {
                    parentBodyStatements.Pop();
                }
                else if (newToken.Type == TokenType.Forcycle)
                {
                    CreateForCycleStatement();
                }
            }
            return null;
        }

        private void CreateForCycleStatement()
        {
            IForCycleStatement forCycle = new ForCycleStatement();
            FillConditionParametersCycle(forCycle);
            forCycle.Parent = parentBodyStatements.Peek();
            parentBodyStatements.Peek().Statements.Add(forCycle);
            parentBodyStatements.Push(forCycle);
        }

        private void FillConditionParametersCycle(IForCycleStatement forCycle)
        {
            CheckAndRemoveLeftParenthesis();
            CheckAndRemoveDataTypeInForCycle();

            FindInnerCounterValue(forCycle);
            FindComparisonOperators(forCycle);
            FindMaxCounterValue(forCycle);
            FindIncrementOrDecrement(forCycle);
        }

        private void FindIncrementOrDecrement(IForCycleStatement forCycle)
        {
            Token newToken;
            while ((newToken = tokenStack.Pop()).Type != TokenType.RightParenthesis)
            {
                if (newToken.Type == TokenType.Plus && tokenStack.Peek().Type == TokenType.Plus)
                {
                    forCycle.IsIncremental = true;
                    tokenStack.Pop();
                    return;
                }
                if (newToken.Type == TokenType.Minus && tokenStack.Peek().Type == TokenType.Minus)
                {
                    forCycle.IsIncremental = false;
                    tokenStack.Pop();
                    return;
                }
            }
            throw new ArgumentException("Invalid increment or decrement of value, make sure forcycle syntax is correct.");
        }

        private void FindMaxCounterValue(IForCycleStatement forCycle)
        {
            Token newToken;
            IIntegerVariable maxCounter = new IntegerVariable();

            while ((newToken = tokenStack.Pop()).Type != TokenType.Comma)
            {
                maxCounter.TokensExpression.Push(newToken);
            }
            maxCounter.Calculate();
            forCycle.MaximumAllowedCounter = maxCounter;
        }

        private void FindComparisonOperators(IForCycleStatement forCycle)
        {
            Token newToken;
            while ((newToken = tokenStack.Peek()).Type != TokenType.NumberCharacters)
            {
                if (IsComparisonOperator(newToken))
                {
                    forCycle.ComparisonOperators.Add(newToken);
                }
                tokenStack.Pop();
            }
        }

        private void FindInnerCounterValue(IForCycleStatement forCycle)
        {
            IIntegerVariable innerCounter = new IntegerVariable();

            innerCounter.Name.Append(tokenStack.Pop().Value);

            CheckAndRemoveEqualsSign();
            Token newToken;
            while ((newToken = tokenStack.Pop()).Type != TokenType.Comma)
            {
                innerCounter.TokensExpression.Push(newToken);
            }
            innerCounter.Calculate();
            forCycle.InnerCounterVariable = innerCounter;
            forCycle.LocalVariables.Add(innerCounter);
        }

        private void CheckAndRemoveDataTypeInForCycle()
        {
            if (tokenStack.Peek().Type != TokenType.IntegerDataType)
            {
                throw new ArgumentException("Missing data type or wrong data type in forcycle declaration.");
            }
            tokenStack.Pop();
        }

        private bool IsComparisonOperator(Token newToken)
        {
            return newToken.Type == TokenType.LessThan || newToken.Type == TokenType.HigherThan
                || newToken.Type == TokenType.Equals || newToken.Type == TokenType.ExclMark;
        }

        private bool IsEndOfBodyStatement(Token newToken)
        {
            return newToken.Type == TokenType.RightBracket;
        }

        private void CreateElseStatement()
        {
            IConditionIfStatement elseCondition = new ConditionIfStatement();
            IConditionIfStatement ifCondition = (IConditionIfStatement)parentBodyStatements.Pop();
            ifCondition.ElseCondition = elseCondition;
            elseCondition.Parent = parentBodyStatements.Peek();
            parentBodyStatements.Push(elseCondition);
        }

        private bool IsElseStatementFound(Token newToken)
        {
            if (parentBodyStatements.Peek() is IConditionIfStatement)
            {
                if (newToken.Type == TokenType.RightBracket)
                {
                    if (tokenStack.Peek().Type == TokenType.Else)
                    {
                        tokenStack.Pop();
                        return true;
                    }
                }
            }
            return false;
        }
        private void CreateRunMethodStatement()
        {
            IRunStatement runStatement = new RunStatement();
            if (tokenStack.Peek().Type == TokenType.Identifier)
            {
                runStatement.Name.Append(tokenStack.Pop().Value);
            }
            else
            {
                throw new ArgumentException("Identifier for method name missing!");
            }
            CheckAndRemoveLeftParenthesis();
            CreateInputParameters(runStatement);
            parentBodyStatements.Peek().Statements.Add(runStatement);
        }

        private void CreateInputParameters(IRunStatement runStatement)
        {
            Token newToken;
            while ((newToken = tokenStack.Peek()).Type != TokenType.Semicolon)
            {
                if (newToken.Type == TokenType.Comma)
                {
                    tokenStack.Pop();
                }
                else if (newToken.Type == TokenType.NumberCharacters)
                {
                    runStatement.TokenParametersList.Add(CreateIntegerVariable());
                }
                else if (newToken.Type == TokenType.StringCharacters)
                {
                    runStatement.TokenParametersList.Add(CreateStringVariable());
                }
                else if (newToken.Type == TokenType.Identifier)
                {
                    runStatement.TokenParametersList.Add(MainClass.FindIdentifier(parentBodyStatements.Peek(), newToken.Value));
                    tokenStack.Pop();
                }
                else
                {
                    tokenStack.Pop();
                }
            }
        }

        private void CreateConditionStatement()
        {
            IConditionIfStatement conditionStatement = new ConditionIfStatement();
            CreateConditionTokens(conditionStatement);
            conditionStatement.Parent = parentBodyStatements.Peek();
            parentBodyStatements.Peek().Statements.Add(conditionStatement);
            parentBodyStatements.Push(conditionStatement);
        }

        private void CreateConditionTokens(IConditionIfStatement condition)
        {
            CheckAndRemoveLeftParenthesis();
            Token newToken;
            while ((newToken = tokenStack.Pop()).Type != TokenType.RightParenthesis)
            {
                condition.ExpressionTokens.Push(newToken);
            }
        }

        private void CheckAndRemoveLeftParenthesis()
        {
            if (tokenStack.Peek().Type == TokenType.LeftParenthesis)
            {
                tokenStack.Pop();
            }
            else
            {
                throw new ArgumentException("Left parenthesis is missing");
            }
        }

        private void CreatePrintStatement()
        {
            CheckAndRemoveLeftParenthesis();
            PrintLnStatement printStatement = new PrintLnStatement
            {
                Parent = parentBodyStatements.Peek()
            };
            if (tokenStack.Peek().Type == TokenType.StringCharacters)
            {
                printStatement.Variable = CreateStringVariable();
            }
            else if (tokenStack.Peek().Type == TokenType.NumberCharacters)
            {
                printStatement.Variable = CreateIntegerVariable();
            }
            else if (tokenStack.Peek().Type == TokenType.Identifier)
            {
                printStatement.Variable = MainClass.FindIdentifier(parentBodyStatements.Peek(), tokenStack.Pop().Value);
            }
            parentBodyStatements.Peek().Statements.Add(printStatement);
        }

        private void CreateRedefinitionStatement(Token newToken)
        {
            IRedefinitionStatement redefStatement = new RedefinitionStatement
            {
                IdentifierToken = newToken,
                Parent = parentBodyStatements.Peek()
            };
            CheckAndRemoveEqualsSign();
            RedefinitionReadTokens(redefStatement);
            parentBodyStatements.Peek().Statements.Add(redefStatement);
        }

        private void RedefinitionReadTokens(IRedefinitionStatement redefStatement)
        {
            Token expressionToken;
            while ((expressionToken = tokenStack.Pop()).Type != TokenType.Semicolon)
            {
                redefStatement.TokensExpression.Push(expressionToken);
            }
        }

        private void CreateMethod(Token newToken)
        {
            IMethod newMethod = new Method();
            newMethod.Name.Append(newToken.Value);

            CreateMethodParameters(newMethod);
            parentBodyStatements.Push(newMethod);
            MainClass.Methods.Add(newMethod);
        }

        private void CreateMethodParameters(IMethod newMethod)
        {
            Token newToken;
            while ((newToken = tokenStack.Pop()).Type != TokenType.LeftBracket)
            {
                if (newToken.Type == TokenType.StringDataType)
                {
                    IStringVariable stringVariable = CreateStringVariable();
                    newMethod.ParametersList.Add(stringVariable);
                    newMethod.LocalVariables.Add(stringVariable);
                }
                else if (newToken.Type == TokenType.IntegerDataType)
                {
                    IIntegerVariable integerVariable = CreateIntegerVariable();
                    newMethod.ParametersList.Add(integerVariable);
                    newMethod.LocalVariables.Add(integerVariable);
                }
            }
        }

        private bool IsMethodFound(Token token)
        {
            return (token.Type == TokenType.Identifier || token.Type == TokenType.Program) && tokenStack.Peek().Type == TokenType.LeftParenthesis;
        }

        private IStringVariable CreateStringVariable()
        {
            IStringVariable stringVariable = new StringVariable();
            if (tokenStack.Peek().Type == TokenType.Identifier)
            {
                stringVariable.Name.Append(tokenStack.Pop().Value);
            }
            CreateVariableDefinition(stringVariable);

            return stringVariable;
        }

        private IIntegerVariable CreateIntegerVariable()
        {
            IIntegerVariable integerVariable = new IntegerVariable();
            if (tokenStack.Peek().Type == TokenType.Identifier)
            {
                integerVariable.Name.Append(tokenStack.Pop().Value);
            }
            CreateVariableDefinition(integerVariable);

            return integerVariable;
        }

        private void DetermineGlobalOrLocalVariable(IVariable variable)
        {
            if (parentBodyStatements.Count == 0)
            {
                MainClass.GlobalVariables.Add(variable);
            }
            else
            {
                parentBodyStatements.Peek().LocalVariables.Add(variable);
            }
        }

        private void CreateVariableDefinition(IVariable variable)
        {
            CheckAndRemoveEqualsSign();
            Token newToken;
            while ((newToken = tokenStack.Pop()).Type != TokenType.Semicolon && newToken.Type != TokenType.Comma && newToken.Type != TokenType.RightParenthesis)
            {
                if (variable is IIntegerVariable integerVariable)
                {
                    integerVariable.TokensExpression.Push(newToken);
                }
                else if (variable is IStringVariable stringVariable)
                {
                    if (newToken.Type == TokenType.Plus)
                    {
                        continue;
                    }
                    stringVariable.Value.Append(newToken.Value);
                }
            }
            if (variable is IIntegerVariable intVariable)
            {
                intVariable.Calculate();
            }
        }

        private void CheckAndRemoveEqualsSign()
        {
            if (tokenStack.Peek().Type == TokenType.Equals)
            {
                tokenStack.Pop();
            }
        }
    }
}
