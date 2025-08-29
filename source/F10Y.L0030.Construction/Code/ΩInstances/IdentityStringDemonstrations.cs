using System;


namespace F10Y.L0030.Construction
{
    public class IdentityStringDemonstrations : IIdentityStringDemonstrations
    {
        #region Infrastructure

        public static IIdentityStringDemonstrations Instance { get; } = new IdentityStringDemonstrations();


        private IdentityStringDemonstrations()
        {
        }

        #endregion
    }
}
