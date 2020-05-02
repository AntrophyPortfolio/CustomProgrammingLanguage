using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace ITEJA_CustomLanguage.Lexer
{
    public class LexerCreator : ILexerCreator
    {
        private static readonly IDictionary<string, TokenType> keyWords = new Dictionary<string, TokenType>
        {
            {"integer", TokenType.IntegerDataType},
            {"string", TokenType.StringDataType},
            {"if", TokenType.If},
            {"else", TokenType.Else},
            {"forcycle", TokenType.Forcycle},
            {"run", TokenType.Run},
            {"program", TokenType.Program},
            {"println", TokenType.PrintLn}
        };
        private static readonly IDictionary<char, TokenType> operatorsAndPunctators = new Dictionary<char, TokenType>
        {
            {'+', TokenType.Plus},
            {'-', TokenType.Minus},
            {'*', TokenType.Multiply},
            {'/', TokenType.Divide},
            {';', TokenType.Semicolon},
            {'(', TokenType.LeftParenthesis},
            {')', TokenType.RightParenthesis},
            {'{', TokenType.LeftBracket},
            {'}', TokenType.RightBracket},
            {'=', TokenType.Equals},
            {',', TokenType.Comma},
            {'<', TokenType.LessThan},
            {'>', TokenType.HigherThan},
            {'!', TokenType.ExclMark}
        };
        private readonly IList<Token> tokensList = new List<Token>();
        private readonly StringReader reader;
        public LexerCreator(string sourceCode)
        {
            reader = new StringReader(sourceCode);
            FindTokens();
        }

        public IEnumerable<string> GetFoundLexems()
        {
            if (tokensList.Count == 0)
            {
                throw new InvalidDataException("No tokens were found!");
            }
            return tokensList.Select(x => x.Value);
        }
        public IEnumerable<Token> GetFoundTokens()
        {
            if (tokensList.Count == 0)
            {
                throw new InvalidDataException("No tokens were found!");
            }
            return tokensList;
        }
        private void FindTokens()
        {
            char character;
            while (!(character = (char)reader.Read()).Equals('\uffff'))
            {
                if (operatorsAndPunctators.ContainsKey(character))
                {
                    CreateOperatorAndPunctatorToken(character);
                }
                else if (character.Equals('"'))
                {
                    CreateStringToken(character);
                }
                else if (char.IsLetter(character))
                {
                    CheckCharacterMeaning(character);
                }
                else if (char.IsDigit(character))
                {
                    CreateIntegerToken(character);
                }
            }
        }

        private void CreateStringToken(char character)
        {
            StringBuilder stringBuilder = new StringBuilder();
            while (!(character = (char)reader.Read()).Equals('"'))
            {
                stringBuilder.Append(character);
            }
            AddToken(TokenType.StringCharacters, stringBuilder.ToString());
        }

        private void CreateIntegerToken(char character)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(character);
            while (!(character = (char)reader.Peek()).Equals(',') && !char.IsWhiteSpace(character) && !character.Equals(';') && !character.Equals(')'))
            {
                stringBuilder.Append(character);
                reader.Read();
            }
            AddToken(TokenType.NumberCharacters, stringBuilder.ToString());
        }

        private void CheckCharacterMeaning(char character)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(character);
            while (char.IsLetterOrDigit(character = (char)reader.Peek()))
            {
                stringBuilder.Append(character);
                reader.Read();
            }

            if (keyWords.ContainsKey(stringBuilder.ToString()))
            {
                AddToken(keyWords[stringBuilder.ToString()], stringBuilder.ToString());
            }
            else
            {
                AddToken(TokenType.Identifier, stringBuilder.ToString());
            }
        }

        private void CreateOperatorAndPunctatorToken(char character)
        {
            AddToken(operatorsAndPunctators[character], character.ToString());
        }
        private void AddToken(TokenType type, string value)
        {
            tokensList.Add(new Token()
            {
                Type = type,
                Value = value
            });
        }
    }
}
