using System;


namespace F10Y.L0030
{
    public class MethodInfoOperations : IMethodInfoOperations
    {
        #region Infrastructure

        public static IMethodInfoOperations Instance { get; } = new MethodInfoOperations();


        private MethodInfoOperations()
        {
        }

        #endregion
    }
}
