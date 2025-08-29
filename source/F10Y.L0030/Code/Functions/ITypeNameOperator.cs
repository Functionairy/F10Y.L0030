using System;

using F10Y.T0002;
using F10Y.T0011;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface ITypeNameOperator :
        L0000.ITypeNameOperator
    {
#pragma warning disable IDE1006 // Naming Styles

        [Ignore]
        public L0000.ITypeNameOperator _L0000 => L0000.TypeNameOperator.Instance;

#pragma warning restore IDE1006 // Naming Styles


        public string Append_OutputTypeName(
            string typeName,
            string outputTypeName)
        {
            var output = $"{typeName}{Instances.TokenSeparators.OutputTypeNameTokenSeparator}{outputTypeName}";
            return output;
        }

        /// <summary>
        /// Simple type name includes:
        /// <list type="bullet">
        /// <item>Generic type parameter and generic method type parameter marker token prefixes ("`" and "``") for generic type and generic method parameters.</item>
        /// <item>Element type relationship marker token suffixes for types with element types ("[]" for arrays, "&amp;" for references, and "*" for pointers).</item>
        /// </list>
        /// It does not include:
        /// <list type="bullet">
        /// <item>Namespace for the type.</item>
        /// <item>Nested parent simple type name (if the type is nested).</item>
        /// <item>Generic type parameters list.</item>
        /// </list>
        /// </summary>
        public string Get_SimpleTypeName(TypeSignature typeSignature)
        {
            string typeIdentityString;

            if (typeSignature.Is_GenericTypeParameter)
            {
                typeIdentityString = Instances.TypeNameOperator.Get_GenericTypeParameterMarkedTypeName(typeSignature.TypeName);
            }
            else if (typeSignature.Is_GenericMethodParameter)
            {
                typeIdentityString = Instances.TypeNameOperator.Get_GenericMethodParameterMarkedTypeName(typeSignature.TypeName);
            }
            else if (typeSignature.Has_ElementType)
            {
                var elementTypeIdentityString = this.Get_SimpleTypeName(typeSignature.ElementType);

                typeIdentityString = Instances.ElementTypeRelationshipOperator.Append_ElementTypeRelationshipMarkers(
                    elementTypeIdentityString,
                    typeSignature.ElementTypeRelationships);
            }
            else
            {
                typeIdentityString = typeSignature.TypeName;
            }

            // Do not include the generic type parameters list.
            var output = typeIdentityString;
            return output;
        }

        /// <summary>
        /// Full type name includes:
        /// <list type="bullet">
        /// <item>Namespace for the type.</item>
        /// <item>Nested parent simple type names (if the type is nested).</item>
        /// <item>Generic type parameter and generic method type parameter marker tokens ("`" and "``") for generic type and generic method parameters.</item>
        /// <item>Element type relationship marker token for types with element types ("[]" for arrays, "&amp;" for references, and "*" for pointers).</item>
        /// <item>Generic type parameters list.</item>
        /// </list>
        /// </summary>
        public string Get_FullTypeName(TypeSignature typeSignature)
        {
            string typeIdentityString;

            if (typeSignature.Is_Nested)
            {
                var parentTypeSignature = this.Get_FullTypeName(typeSignature.NestedTypeParent);

                typeIdentityString = Instances.SignatureStringOperator.Append_NestedTypeTypeName(
                    parentTypeSignature,
                    typeSignature.TypeName);
            }
            else
            {
                if (typeSignature.Is_GenericTypeParameter)
                {
                    typeIdentityString = Instances.TypeNameOperator.Get_GenericTypeParameterMarkedTypeName(typeSignature.TypeName);
                }
                else if (typeSignature.Is_GenericMethodParameter)
                {
                    typeIdentityString = Instances.TypeNameOperator.Get_GenericMethodParameterMarkedTypeName(typeSignature.TypeName);
                }
                else if (typeSignature.Has_ElementType)
                {
                    var elementTypeIdentityString = this.Get_FullTypeName(typeSignature.ElementType);

                    typeIdentityString = Instances.ElementTypeRelationshipOperator.Append_ElementTypeRelationshipMarkers(
                        elementTypeIdentityString,
                        typeSignature.ElementTypeRelationships);
                }
                else
                {
                    var namespaceName = typeSignature.NamespaceName;

                    var namespaceNameIsNullOrEmpty = Instances.StringOperator.Is_NullOrEmpty(namespaceName);
                    if (namespaceNameIsNullOrEmpty)
                    {
                        typeIdentityString = typeSignature.TypeName;
                    }
                    else
                    {
                        typeIdentityString = Instances.SignatureStringOperator.Combine(
                            typeSignature.NamespaceName,
                            typeSignature.TypeName);
                    }
                }
            }

            // No need to check if null or empty; if so, an empty string will be returned.
            var genericTypeInputsList = Instances.SignatureOperator.Get_GenericTypeInputsList(typeSignature.GenericTypeInputs);

            var output = Instances.SignatureStringOperator.Append_TypeParameterList(
                typeIdentityString,
                genericTypeInputsList);

            return output;
        }

        /// <summary>
        /// Prefixes the <see cref="ITypeNameAffixes.TypeParameterMarker_Prefix"/> (<inheritdoc cref="ITypeNameAffixes.TypeParameterMarker_Prefix" path="descendant::name"/>) to the generic type parameter name.
        /// </summary>
        public string Get_GenericTypeParameterMarkedTypeName(string genericTypeParameterName)
        {
            var output = $"{Instances.TypeNameAffixes.TypeParameterMarker_Prefix}{genericTypeParameterName}";
            return output;
        }

        /// <summary>
        /// Prefixes the <see cref="ITypeNameAffixes.MethodTypeParameterMarker_Prefix"/> (<inheritdoc cref="ITypeNameAffixes.MethodTypeParameterMarker_Prefix" path="descendant::name"/>) to the generic type parameter name.
        /// </summary>
        public string Get_GenericMethodParameterMarkedTypeName(string genericTypeParameterName)
        {
            var output = $"{Instances.TypeNameAffixes.MethodTypeParameterMarker_Prefix}{genericTypeParameterName}";
            return output;
        }

        public string Get_PositionalTypeName_ForGenericTypeParameter(Type type)
        {
            var position = type.GenericParameterPosition;

            var output = this.Get_PositionalTypeName_ForGenericTypeParameter(position);
            return output;
        }

        public string Get_PositionalTypeName_ForGenericTypeParameter(int genericTypeParameterPosition)
        {
            var output = $"{Instances.TypeNameAffixes.GenericTypeParameterType_Prefix}{genericTypeParameterPosition}";
            return output;
        }

        public string Get_PositionalTypeName_ForGenericMethodParameter(Type type)
        {
            var position = type.GenericParameterPosition;

            var output = this.Get_PositionalTypeName_ForGenericMethodParameter(position);
            return output;
        }
        public string Get_PositionalTypeName_ForGenericMethodParameter(int genericTypeParameterPosition)
        {
            var output = $"{Instances.TypeNameAffixes.GenericMethodParameterType_Prefix}{genericTypeParameterPosition}";
            return output;
        }
    }
}
