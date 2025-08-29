using System;


namespace F10Y.L0030
{
    public class MethodParameterOperator : IMethodParameterOperator
    {
        #region Infrastructure

        public static IMethodParameterOperator Instance { get; } = new MethodParameterOperator();


        private MethodParameterOperator()
        {
        }

        #endregion
    }
}
