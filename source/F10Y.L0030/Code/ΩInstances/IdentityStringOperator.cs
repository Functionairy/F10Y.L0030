using System;


namespace F10Y.L0030
{
    public class IdentityStringOperator : IIdentityStringOperator
    {
        #region Infrastructure

        public static IIdentityStringOperator Instance { get; } = new IdentityStringOperator();


        private IdentityStringOperator()
        {
        }

        #endregion
    }
}
