using System;


namespace F10Y.L0030
{
    public class TokenSeparators_ForSignatureString : ITokenSeparators_ForSignatureString
    {
        #region Infrastructure

        public static ITokenSeparators_ForSignatureString Instance { get; } = new TokenSeparators_ForSignatureString();


        private TokenSeparators_ForSignatureString()
        {
        }

        #endregion
    }
}
