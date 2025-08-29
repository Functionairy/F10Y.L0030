using System;
using System.Reflection;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IIdentityStringOperator
    {
        /// <summary>
        /// Append does not insert a <see cref="L0000.ITokenSeparators.NamespaceNameTokenSeparator"/>.
        /// </summary>
        public string Append(string part1, string part2)
        {
            var output = $"{part1}{part2}";
            return output;
        }

        public string Append_ElementTypeRelationshipMarkers(
            string typeName,
            ElementTypeRelationships elementTypeRelationships)
        {
            var output = Instances.ElementTypeRelationshipOperator.Append_ElementTypeRelationshipMarkers(
                typeName,
                elementTypeRelationships,
                Instances.TypeNameAffixes.Array_Suffix,
                Instances.TypeNameAffixes.ByReference_Suffix_String,
                Instances.TypeNameAffixes.Pointer_Suffix_String);

            return output;
        }

        public string Append_NestedTypeTypeName(
            string parentTypeName,
            string nestedTypeName)
        {
            var output = this.Combine(
                parentTypeName,
                nestedTypeName,
                Instances.TokenSeparators._ForIdentityString.NestedTypeNameTokenSeparator_String);

            return output;
        }

        /// <summary>
        /// Combine inserts a <see cref="L0000.ITokenSeparators.NamespaceNameTokenSeparator"/>.
        /// </summary>
        public string Combine(string part1, string part2)
        {
            var output = this.Combine(
                part1,
                part2,
                Instances.TokenSeparators.NamespaceNameTokenSeparator_String);

            return output;
        }

        public string Combine(
            string part1,
            string part2,
            string separator)
        {
            var output = $"{part1}{separator}{part2}";
            return output;
        }

        public string Get_GenericParameterCountToken(
            int genericTypeParameterCount,
            string parameterCountTokenSeparator)
        {
            var output = $"{parameterCountTokenSeparator}{genericTypeParameterCount}";
            return output;
        }

        public string Get_GenericTypeParameterCountToken(int genericTypeParameterCount)
        {
            var output = this.Get_GenericParameterCountToken(
                genericTypeParameterCount,
                Instances.TokenSeparators.TypeParameterCountSeparator_String);

            return output;
        }

        public string Get_GenericMethodParameterCountToken(int genericMethodParameterCount)
        {
            var output = this.Get_GenericParameterCountToken(
                genericMethodParameterCount,
                Instances.TokenSeparators.MethodTypeParameterCountSeparator);

            return output;
        }

        public string Get_ErrorIdentityString(string identityStringValue)
        {
            var output = Instances.KindMarkerOperator.Make_ErrorKindMarked(identityStringValue);
            return output;
        }

        public string Get_EventIdentityString(string identityStringValue)
        {
            var output = Instances.KindMarkerOperator.Make_EventKindMarked(identityStringValue);
            return output;
        }

        public string Get_FieldIdentityString(string identityStringValue)
        {
            var output = Instances.KindMarkerOperator.Make_FieldKindMarked(identityStringValue);
            return output;
        }

        public string Get_IdentityString(Signature signature)
        {
            var output = Instances.SignatureOperator.Get_IdentityString(signature);
            return output;
        }

        public string Get_IdentityString(MemberInfo memberInfo)
        {
            var signature = Instances.SignatureOperator.Get_Signature(memberInfo);

            var output = this.Get_IdentityString(signature);
            return output;
        }

        public string Get_MethodIdentityString(string identityStringValue)
        {
            var output = Instances.KindMarkerOperator.Make_MethodKindMarked(identityStringValue);
            return output;
        }

        public string Get_PropertyIdentityString(string identityStringValue)
        {
            var output = Instances.KindMarkerOperator.Make_PropertyKindMarked(identityStringValue);
            return output;
        }

        public string Get_TypeIdentityString(string identityStringValue)
        {
            var output = Instances.KindMarkerOperator.Make_TypeKindMarked(identityStringValue);
            return output;
        }

        /// <inheritdoc cref="IMemberNameOperator.Modify_MemberName(string)"/>
        public string Modify_MemberName(string name)
        {
            var output = Instances.MemberNameOperator.Modify_MemberName(name);
            return output;
        }
    }
}
