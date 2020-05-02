using ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.BodyStatements;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses
{
    public interface IStatement
    {
        /// <summary>
        /// Executes all statements within itself
        /// </summary>
        void Execute();
        /// <summary>
        /// Parent of the statement for local variables
        /// </summary>
        IBodyStatement Parent { get; set; }
    }
}
