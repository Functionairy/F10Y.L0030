using System;

using F10Y.T0003;


namespace F10Y.L0030
{
    /// <summary>
    /// 
    /// </summary>
    // Prior work: R5T.L0066.
    [ValuesMarker]
    public partial interface ITypeNameAffixes
    {
        /// <summary>
        /// <para><name>"[]" (open- and close-bracket pair)</name></para>
        /// </summary>
        public string Array_Suffix => "[]";

        /// <summary>
        /// <para><value>Attribute</value></para>
        /// </summary>
        public string AttributeSuffix => "Attribute";

        /// <summary>
        /// <para><name>'&amp;' (ampersand)</name></para>
        /// Indicates that a type is passed by reference.
        /// Used in <see cref="Type.FullName"/>
        /// </summary>
        public const char ByReference_Suffix_Constant = '&';

        /// <inheritdoc cref="ByReference_Suffix_Constant"/>
        public char ByReference_Suffix => ByReference_Suffix_Constant;

        /// <inheritdoc cref="ByReference_Suffix_Constant"/>
        public const string ByReference_Suffix_String_Constant = "&";

        /// <inheritdoc cref="ByReference_Suffix_String_Constant"/>
        public string ByReference_Suffix_String => ByReference_Suffix_String_Constant;

        /// <summary>
        /// <para><name>"`" (tick)</name></para>
        /// </summary>
        public string GenericTypeParameterType_Prefix => "`";

        /// <summary>
        /// <para><name>"``" (double-tick)</name></para>
        /// </summary>
        public string GenericMethodParameterType_Prefix => "``";

        /// <summary>
        /// <para><name>"<value>``</value>" (two back-ticks)</name></para>
        /// </summary>
        public const string MethodTypeParameterMarker_Prefix_Constant = "``";

        /// <inheritdoc cref="MethodTypeParameterMarker_Prefix_Constant"/>
        public string MethodTypeParameterMarker_Prefix => MethodTypeParameterMarker_Prefix_Constant;

        /// <summary>
        /// <para><name>"*" (the asterix)</name></para>
        /// </summary>
        public char Pointer_Suffix => '*';

        /// <inheritdoc cref="Pointer_Suffix"/>
        public string Pointer_Suffix_String => "*";

        /// <summary>
        /// <para><name>'<value>`</value>' (back-tick)</name></para>
        /// </summary>
        public const char TypeParameterMarker_Prefix_Constant = '`';

        /// <inheritdoc cref="TypeParameterMarker_Prefix_Constant"/>
        public char TypeParameterMarker_Prefix => TypeParameterMarker_Prefix_Constant;
    }
}
