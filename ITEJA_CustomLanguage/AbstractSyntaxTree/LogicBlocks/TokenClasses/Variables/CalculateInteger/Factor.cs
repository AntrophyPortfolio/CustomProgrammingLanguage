using System;
using System.Collections.Generic;
using System.Text;

namespace ITEJA_CustomLanguage.AbstractSyntaxTree.LogicBlocks.TokenClasses.Variables.CalculateInteger
{
    class Factor
    {
        public double Value { get; }
        public Factor(double pValue)
        {
            Value = pValue;
        }
    }
}
