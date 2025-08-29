using System;


namespace F10Y.L0030
{
    public class MethodNameOperator : IMethodNameOperator
    {
        #region Infrastructure

        public static IMethodNameOperator Instance { get; } = new MethodNameOperator();


        private MethodNameOperator()
        {
        }

        #endregion
    }
}
