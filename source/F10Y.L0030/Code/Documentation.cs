using System;

using F10Y.T0001;


namespace F10Y.L0030
{
	/// <summary>
	/// Signature type, signature string, and identity string library.
	/// </summary>
	[DocumentationsMarker]
	public static class Documentation
	{
		[DocumentationsMarker]
		public static class For_IDStrings
		{
            /// <summary>
            /// <link><see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/#id-strings"/></link>
            /// </summary>
            public static readonly object MSDocs_APIDocumentationLink;

            /// <summary>
            /// <link><see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments#d42-id-string-format"/></link>
            /// </summary>
            public static readonly object MSDocs_DocumentationCommentsLink;

            /// <summary>
            /// <link><see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments#d43-id-string-examples"/></link>
            /// </summary>
            public static readonly object MSDocs_Examples;
        }

        /// <summary>
        /// All parameters <em>should</em> have names, but somehow it's possible that they do not.
        /// </summary>
        public static readonly object ParametersShouldHaveParameterNames;
    }
}