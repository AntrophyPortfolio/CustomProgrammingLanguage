using ITEJA_CustomLanguage.AbstractSyntaxTree;
using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks;
using ITEJA_CustomLanguage.Lexer;
using System;
using System.IO;

namespace ITEJA_CustomLanguage
{
    class Program
    {
        const string test1 = "test1_recursion.vpp";
        const string test2 = "test2_cycles.vpp";
        const string test3 = "test3_conditions.vpp";
        const string test4 = "test4_variables.vpp";
        const string test5 = "test5_complex.vpp";
        static void Main(string[] args)
        {
            string sourceCode = File.ReadAllText(test5); // change files

            sourceCode = sourceCode.Replace("\r\n", string.Empty).Replace("\t", string.Empty);

            ILexerCreator lexer = new LexerCreator(sourceCode);

            //PrintLexems(lexer);
            //PrintTokens(lexer);

            SyntaxTreeCreator ast = new SyntaxTreeCreator(lexer.GetFoundTokens());

            MainClass.Run();
        }
        /// <summary>
        /// Prints Lexems to the console.
        /// </summary>
        /// <param name="lexer">Source lexer</param>
        static void PrintLexems(ILexerCreator lexer)
        {
            foreach (var item in lexer.GetFoundLexems())
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        /// Prints Tokens to the console
        /// </summary>
        /// <param name="lexer">Source lexer</param>
        static void PrintTokens(ILexerCreator lexer)
        {
            foreach (var item in lexer.GetFoundTokens())
            {
                Console.WriteLine($"{item.Value}\t{item.Type}");
            }
        }
    }
}
