using System;


namespace F10Y.L0030
{
    public class MethodSignatureOperator : IMethodSignatureOperator
    {
        #region Infrastructure

        public static IMethodSignatureOperator Instance { get; } = new MethodSignatureOperator();


        private MethodSignatureOperator()
        {
        }

        #endregion
    }
}
