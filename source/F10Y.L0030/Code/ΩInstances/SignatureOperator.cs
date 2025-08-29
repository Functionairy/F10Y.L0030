using System;


namespace F10Y.L0030
{
    public class SignatureOperator : ISignatureOperator
    {
        #region Infrastructure

        public static ISignatureOperator Instance { get; } = new SignatureOperator();


        private SignatureOperator()
        {
        }

        #endregion
    }
}
