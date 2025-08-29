using System;


namespace F10Y.L0030.Construction
{
    public class SignatureDemonstrations : ISignatureDemonstrations
    {
        #region Infrastructure

        public static ISignatureDemonstrations Instance { get; } = new SignatureDemonstrations();


        private SignatureDemonstrations()
        {
        }

        #endregion
    }
}
