using System;


namespace F10Y.L0030
{
    public class NamedArgumentSignatureOperator : INamedArgumentSignatureOperator
    {
        #region Infrastructure

        public static INamedArgumentSignatureOperator Instance { get; } = new NamedArgumentSignatureOperator();


        private NamedArgumentSignatureOperator()
        {
        }

        #endregion
    }
}
