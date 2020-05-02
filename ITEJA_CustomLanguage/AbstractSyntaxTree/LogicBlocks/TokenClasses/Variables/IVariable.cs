using System.Text;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.Variables
{
    public interface IVariable
    {
        StringBuilder Value { get; set; }
        StringBuilder Name { get; set; }
    }
}
