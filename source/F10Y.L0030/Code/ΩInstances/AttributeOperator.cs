using System;


namespace F10Y.L0030
{
    public class AttributeOperator : IAttributeOperator
    {
        #region Infrastructure

        public static IAttributeOperator Instance { get; } = new AttributeOperator();


        private AttributeOperator()
        {
        }

        #endregion
    }
}
