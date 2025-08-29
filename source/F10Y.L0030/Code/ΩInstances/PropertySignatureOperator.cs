using System;


namespace F10Y.L0030
{
    public class PropertySignatureOperator : IPropertySignatureOperator
    {
        #region Infrastructure

        public static IPropertySignatureOperator Instance { get; } = new PropertySignatureOperator();


        private PropertySignatureOperator()
        {
        }

        #endregion
    }
}
