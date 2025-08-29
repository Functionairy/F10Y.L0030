using System;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IMemberNameOperator
    {
        /// <summary>
        /// The name returned by <see cref="System.Reflection.MemberInfo.Name"/> needs to be adjusted for use in identity strings. 
        /// </summary>
        public string Modify_MemberName(string name)
        {
            var output = name;

            // Namespace token separator '.' becomes '#'.
            // (This happens when members are explicitly implemented.)
            output = Instances.StringOperator.Replace_Character(
                output,
                Instances.TokenSeparators.NamespaceNameTokenSeparator,
                Instances.TokenSeparators.ExplicitImplementationNamespaceTokenSeparator);

            // Generic type input lists token separators '<' and '>' become '{' and '}'.
            output = Instances.StringOperator.Replace_Character(
                output,
                Instances.TokenSeparators.TypeArgumentListOpenTokenSeparator,
                Instances.TokenSeparators._ForIdentityString.TypeArgumentListOpenTokenSeparator);

            output = Instances.StringOperator.Replace_Character(
                output,
                Instances.TokenSeparators.TypeArgumentListCloseTokenSeparator,
                Instances.TokenSeparators._ForIdentityString.TypeArgumentListCloseTokenSeparator);

            // The type name separator in generic type input lists for explicitly implemented members changes from ',' to '@'.
            output = Instances.StringOperator.Replace_Character(
                output,
                Instances.TokenSeparators.ArgumentListSeparator,
                Instances.TokenSeparators.ExplicitImplementationArgumentListSeparator);

            return output;
        }

        /// <summary>
        /// The name returned by <see cref="System.Reflection.MemberInfo.Name"/> needs to be adjusted for use in signature strings.
        /// This is primarily about handling the names of explicitly implemented members.
        /// </summary>
        public string Modify_MemberName_ForMemberName(string name)
        {
            var output = name;

            // Namespace token separator '.' becomes '#'.
            // (This happens when members are explicitly implemented.)
            output = Instances.StringOperator.Replace_Character(
                output,
                Instances.TokenSeparators.ExplicitImplementationNamespaceTokenSeparator,
                Instances.TokenSeparators.NamespaceNameTokenSeparator);

            // Generic type input lists token separators '<' and '>' become '{' and '}'.
            output = Instances.StringOperator.Replace_Character(
                output,
                '{',
                Instances.TokenSeparators.TypeArgumentListOpenTokenSeparator);

            output = Instances.StringOperator.Replace_Character(
                output,
                '}',
                Instances.TokenSeparators.TypeArgumentListCloseTokenSeparator);

            //// The type name separator in generic type input lists for explicitly implemented members changes from ',' to '@'.
            //output = Instances.StringOperator.Replace_Character(
            //    output,
            //    Instances.TokenSeparators.ArgumentListSeparator,
            //    Instances.TokenSeparators.ExplicitImplementationArgumentListSeparator);

            return output;
        }

        /// <summary>
        /// The name returned by <see cref="System.Reflection.MemberInfo.Name"/> needs to be adjusted for use in signature strings.
        /// This is primarily about handling the names of explicitly implemented members.
        /// </summary>
        public string Modify_MemberName_ForSignatureString(string name)
        {
            var output = name;

            // Namespace token separator '.' becomes '#'.
            // (This happens when members are explicitly implemented.)
            output = Instances.StringOperator.Replace_Character(
                output,
                Instances.TokenSeparators.NamespaceNameTokenSeparator,
                Instances.TokenSeparators.ExplicitImplementationNamespaceTokenSeparator);

            // Generic type input lists token separators '<' and '>' become '{' and '}'.
            output = Instances.StringOperator.Replace_Character(
                output,
                Instances.TokenSeparators.TypeArgumentListOpenTokenSeparator,
                '{');

            output = Instances.StringOperator.Replace_Character(
                output,
                Instances.TokenSeparators.TypeArgumentListCloseTokenSeparator,
                '}');

            //// The type name separator in generic type input lists for explicitly implemented members changes from ',' to '@'.
            //output = Instances.StringOperator.Replace_Character(
            //    output,
            //    Instances.TokenSeparators.ArgumentListSeparator,
            //    Instances.TokenSeparators.ExplicitImplementationArgumentListSeparator);

            return output;
        }
    }
}
