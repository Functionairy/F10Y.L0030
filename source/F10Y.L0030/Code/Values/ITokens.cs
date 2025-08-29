using System;

using F10Y.T0003;


namespace F10Y.L0030
{
    [ValuesMarker]
    public partial interface ITokens
    {
        /// <summary>
        /// <para><description>"<value>[]</value>", empty set of open and close brackets.</description></para>
        /// </summary>
        public string ArrayTypeToken => "[]";

        /// <summary>
        /// <para><description>"<value>()</value>", empty set of open and close parentheses.</description></para>
        /// </summary>
        public string EmptyParameterListToken => "()";

        /// <summary>
        /// <para>" [Obsolete]", a space, then the obsolete attribute.</para>
        /// Note that the space is essential, since all obsolete signature strings will need to separate from any array output types.
        /// </summary>
        public string ObsoleteToken => " [Obsolete]";

        /// <summary>
        /// <para><description>"<value>*</value>", asterix.</description></para>
        /// </summary>
        public string PointerTypeToken => "*";

        /// <summary>
        /// <para><description>"<value>&amp;</value>", ampersand.</description></para>
        /// </summary>
        public string ReferenceTypeToken => "&";
    }
}
