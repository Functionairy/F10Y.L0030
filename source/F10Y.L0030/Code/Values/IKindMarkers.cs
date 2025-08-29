using System;

using F10Y.T0003;


namespace F10Y.L0030
{
    /// <summary>
    /// Kind marker values (for .NET identity strings).
    /// </summary>
    /// <remarks>
    /// For documentation see:
    /// <list type="bullet">
    /// <item><inheritdoc cref="Documentation.For_IDStrings.MSDocs_APIDocumentationLink" path="descendant::link"/></item>
    /// <item><inheritdoc cref="Documentation.For_IDStrings.MSDocs_DocumentationCommentsLink" path="descendant::link"/></item>
    /// </list>
    /// </remarks>
    [ValuesMarker]
    public partial interface IKindMarkers
    {
        /// <summary>
        /// <para>'!' (exclamation point)</para>
        /// </summary>
        public const char Error_Constant = '!';

        /// <inheritdoc cref="Error_Constant"/>
        public char Error => Error_Constant;

        /// <summary>
        /// <para>'E' (capital E)</para>
        /// </summary>
        public const char Event_Constant = 'E';

        /// <inheritdoc cref="Event_Constant"/>
        public char Event => Event_Constant;

        /// <summary>
        /// <para>'F' (capital F)</para>
        /// </summary>
        public const char Field_Constant = 'F';

        /// <inheritdoc cref="Field_Constant"/>
        public char Field => Field_Constant;

        /// <summary>
        /// <para>'M' (capital M)</para>
        /// </summary>
        public const char Method_Constant = 'M';

        /// <inheritdoc cref="Method_Constant"/>
        public char Method => Method_Constant;

        /// <summary>
        /// <para>'N' (capital N)</para>
        /// </summary>
        public const char Namespace_Constant = 'N';

        /// <inheritdoc cref="Namespace_Constant"/>
        public char Namespace => Namespace_Constant;

        /// <summary>
        /// <para>'P' (capital P)</para>
        /// </summary>
        public const char Property_Constant = 'P';

        /// <inheritdoc cref="Property_Constant"/>
        public char Property => Property_Constant;

        /// <summary>
        /// <para>'T' (capital T)</para>
        /// </summary>
        public const char Type_Constant = 'T';

        /// <inheritdoc cref="Type_Constant"/>
        public char Type => Type_Constant;


#pragma warning disable IDE1006 // Naming Styles

        /// <inheritdoc cref="Error_Constant"/>
        public char _ExclamationPoint => Error_Constant;

        /// <inheritdoc cref="Event_Constant"/>
        public char _E => Event_Constant;

        /// <inheritdoc cref="Field_Constant"/>
        public char _F => Field_Constant;

        /// <inheritdoc cref="Method_Constant"/>
        public char _M => Method_Constant;

        /// <inheritdoc cref="Namespace_Constant"/>
        public char _N => Namespace_Constant;

        /// <inheritdoc cref="Property_Constant"/>
        public char _P => Property_Constant;

        /// <inheritdoc cref="Type_Constant"/>
        public char _T => Type_Constant;

#pragma warning restore IDE1006 // Naming Styles
    }
}
