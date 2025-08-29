using System;


namespace F10Y.L0030
{
    public class MethodBaseOperator : IMethodBaseOperator
    {
        #region Infrastructure

        public static IMethodBaseOperator Instance { get; } = new MethodBaseOperator();


        private MethodBaseOperator()
        {
        }

        #endregion
    }
}
