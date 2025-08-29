using System;


namespace F10Y.L0030
{
    public class MethodInfoOperator : IMethodInfoOperator
    {
        #region Infrastructure

        public static IMethodInfoOperator Instance { get; } = new MethodInfoOperator();


        private MethodInfoOperator()
        {
        }

        #endregion
    }
}
