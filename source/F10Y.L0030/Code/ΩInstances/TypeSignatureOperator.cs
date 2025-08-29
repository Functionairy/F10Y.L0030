using System;


namespace F10Y.L0030
{
    public class TypeSignatureOperator : ITypeSignatureOperator
    {
        #region Infrastructure

        public static ITypeSignatureOperator Instance { get; } = new TypeSignatureOperator();


        private TypeSignatureOperator()
        {
        }

        #endregion
    }
}
