using System;


namespace F10Y.L0030
{
    public class ElementTypeRelationshipOperator : IElementTypeRelationshipOperator
    {
        #region Infrastructure

        public static IElementTypeRelationshipOperator Instance { get; } = new ElementTypeRelationshipOperator();


        private ElementTypeRelationshipOperator()
        {
        }

        #endregion
    }
}
