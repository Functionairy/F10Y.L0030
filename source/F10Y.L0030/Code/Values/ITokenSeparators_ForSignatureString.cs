using System;

using F10Y.T0003;


namespace F10Y.L0030
{
    [ValuesMarker]
    public partial interface ITokenSeparators_ForSignatureString
    {
        /// <summary>
        /// <para>'~' (tilde)</para>
        /// Separates the namespaced, typed, argumented, method name from its output type.
        /// Used for implicit and explicit operator methods.
        /// </summary>
        public const char OutputTypeTokenSeparator_Constant = '~';

        /// <inheritdoc cref="OutputTypeTokenSeparator_Constant"/>
        public char OutputTypeTokenSeparator => OutputTypeTokenSeparator_Constant;

        /// <summary>
        /// <para>'~' (tilde)</para>
        /// Separates the namespaced, typed, argumented, method name from its output type.
        /// Used for implicit and explicit operator methods.
        /// </summary>
        public const string OutputTypeTokenSeparator_String_Constant = "~";

        /// <inheritdoc cref="OutputTypeTokenSeparator_String_Constant"/>
        public string OutputTypeTokenSeparator_String => OutputTypeTokenSeparator_String_Constant;
    }
}
