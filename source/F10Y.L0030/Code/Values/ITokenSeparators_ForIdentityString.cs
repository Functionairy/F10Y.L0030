using System;

using F10Y.T0003;


namespace F10Y.L0030
{
    [ValuesMarker]
    public partial interface ITokenSeparators_ForIdentityString
    {
        /// <summary>
        /// <para>'.' (period)</para>
        /// Separates tokens in a nested type name (parent type name, child type name) from each other.
        /// The nested type name token separator is the same as the namespaced token separator.
        /// <para>Note: this is different from the standard nested type name token separator (used by <see cref="Type.FullName"/>), which is '+' (plus).</para>
        /// </summary>
        public const char NestedTypeNameTokenSeparator_Constant = '.';

        /// <inheritdoc cref="NestedTypeNameTokenSeparator_Constant"/>
        public char NestedTypeNameTokenSeparator => NestedTypeNameTokenSeparator_Constant;

        /// <inheritdoc cref="NestedTypeNameTokenSeparator_Constant"/>
        public const string NestedTypeNameTokenSeparator_String_Constant = ".";

        /// <inheritdoc cref="NestedTypeNameTokenSeparator_String_Constant"/>
        public string NestedTypeNameTokenSeparator_String => NestedTypeNameTokenSeparator_String_Constant;

        /// <summary>
        /// <para>'{' (open-brace)</para>
        /// </summary>
        public const char TypeArgumentListOpenTokenSeparator_Constant = '{';

        /// <inheritdoc cref="TypeArgumentListOpenTokenSeparator_Constant"/>
        public char TypeArgumentListOpenTokenSeparator => TypeArgumentListOpenTokenSeparator_Constant;

        /// <summary>
        /// <para>'}' (close-brace)</para>
        /// </summary>
        public const char TypeArgumentListCloseTokenSeparator_Constant = '}';

        /// <inheritdoc cref="TypeArgumentListCloseTokenSeparator_Constant"/>
        public char TypeArgumentListCloseTokenSeparator => TypeArgumentListCloseTokenSeparator_Constant;
    }
}
