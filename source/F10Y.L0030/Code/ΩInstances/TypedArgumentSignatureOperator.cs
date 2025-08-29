using System;


namespace F10Y.L0030
{
    public class TypedArgumentSignatureOperator : ITypedArgumentSignatureOperator
    {
        #region Infrastructure

        public static ITypedArgumentSignatureOperator Instance { get; } = new TypedArgumentSignatureOperator();


        private TypedArgumentSignatureOperator()
        {
        }

        #endregion
    }
}
