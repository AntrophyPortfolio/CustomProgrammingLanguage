using System.Text;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.Variables
{
    class StringVariable : IStringVariable
    {
        public StringBuilder Value { get; set; } = new StringBuilder();
        public StringBuilder Name { get; set; } = new StringBuilder();
    }
}
