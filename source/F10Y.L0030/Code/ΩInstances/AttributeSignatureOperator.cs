using System;


namespace F10Y.L0030
{
    public class AttributeSignatureOperator : IAttributeSignatureOperator
    {
        #region Infrastructure

        public static IAttributeSignatureOperator Instance { get; } = new AttributeSignatureOperator();


        private AttributeSignatureOperator()
        {
        }

        #endregion
    }
}
