using System;


namespace F10Y.L0030
{
    public class TokenSeparators : ITokenSeparators
    {
        #region Infrastructure

        public static ITokenSeparators Instance { get; } = new TokenSeparators();


        private TokenSeparators()
        {
        }

        #endregion
    }
}
