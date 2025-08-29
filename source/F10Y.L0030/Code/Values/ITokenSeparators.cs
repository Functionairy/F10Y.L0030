using System;

using F10Y.T0003;
using F10Y.T0011;


namespace F10Y.L0030
{
    [ValuesMarker]
    public partial interface ITokenSeparators :
        L0000.ITokenSeparators
    {
#pragma warning disable IDE1006 // Naming Styles

        [Ignore]
        public L0000.ITokenSeparators _L0000 => L0000.TokenSeparators.Instance;

        [Ignore]
        public ITokenSeparators_ForIdentityString _ForIdentityString => TokenSeparators_ForIdentityString.Instance;

        [Ignore]
        public ITokenSeparators_ForSignatureString _ForSignatureString => TokenSeparators_ForSignatureString.Instance;

#pragma warning restore IDE1006 // Naming Styles


        /// <summary>
        /// <para>':' (colon)</para>
        /// Separates the first character (which is the kind marker_ from the rest of the identity name (which is the identity name value).
        /// </summary>
        public const char KindMarkerTokenSeparator_Constant = ':';

        /// <inheritdoc cref="KindMarkerTokenSeparator_Constant"/>
        public char KindMarkerTokenSeparator => KindMarkerTokenSeparator_Constant;

        /// <summary>
        /// <para>',' (comma)</para>
        /// </summary>
        public const char ListItemSeparator_Constant = ',';

        /// <inheritdoc cref="ListItemSeparator_Constant"/>
        public char ListItemSeparator => ListItemSeparator_Constant;

        /// <summary>
        /// <para><name>'~' (tilde)</name></para>
        /// Separates tokens in an output-type named type name from each other.
        /// </summary>
        public const char OutputTypeNameTokenSeparator_Constant = '~';

        /// <inheritdoc cref="OutputTypeNameTokenSeparator_Constant"/>
        public char OutputTypeNameTokenSeparator => OutputTypeNameTokenSeparator_Constant;
    }
}
