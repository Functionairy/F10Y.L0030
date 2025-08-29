using System;


namespace F10Y.L0030
{
    public class TokenSeparators_ForIdentityString : ITokenSeparators_ForIdentityString
    {
        #region Infrastructure

        public static ITokenSeparators_ForIdentityString Instance { get; } = new TokenSeparators_ForIdentityString();


        private TokenSeparators_ForIdentityString()
        {
        }

        #endregion
    }
}
