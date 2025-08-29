using System;
using System.Collections.Generic;
using System.Linq;

using F10Y.L0000.Extensions;
using F10Y.T0002;
using F10Y.T0011;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface INamespacedTypeNameOperator :
        L0000.INamespacedTypeNameOperator
    {
#pragma warning disable IDE1006 // Naming Styles

        [Ignore]
        public L0000.INamespacedTypeNameOperator _L0000 => L0000.NamespacedTypeNameOperator.Instance;

#pragma warning restore IDE1006 // Naming Styles


        public string Get_NamespacedTypeName(
            TypeSignature typeSignature,
            // If the type is the type of a method parameter, generic type inputs are output as full recurive namespaced type names.
            // If not, just the generic type input count is output.
            bool isInParameterContext)
        {
            var typeGenericTypeParameters = Instances.TypeSignatureOperator.Get_NestedTypeGenericTypeParameters(typeSignature);

            var typeIndex = 0;
            var genericTypeParameterPositionsByName = typeGenericTypeParameters
                .ToDictionary(
                    x => x.TypeName,
                    x => typeIndex++);

            // A type will never have method generic parameters (since a type cannot be declared inside a method).
            var methodGenericTypeParameterPositionsByName = Instances.DictionaryOperator.Empty<string, int>();

            var output = this.Get_NamespacedTypeName(
                typeSignature,
                genericTypeParameterPositionsByName,
                methodGenericTypeParameterPositionsByName,
                isInParameterContext);

            return output;
        }

        public string Get_NamespacedTypeName(
            TypeSignature typeSignature,
            IDictionary<string, int> genericTypeParameterPositionsByName,
            IDictionary<string, int> methodGenericTypeParameterPositionsByName,
            bool isInParameterContext)
        {
            string output;

            if (typeSignature.Is_Nested)
            {
                output = this.Get_NamespacedTypeName_ForNestedType(
                    typeSignature,
                    genericTypeParameterPositionsByName,
                    methodGenericTypeParameterPositionsByName,
                    isInParameterContext);
            }
            else
            {
                if (typeSignature.Has_ElementType)
                {
                    output = this.Get_NamespacedTypeName_ForHasElementType(
                        typeSignature,
                        genericTypeParameterPositionsByName,
                        methodGenericTypeParameterPositionsByName,
                        isInParameterContext);
                }
                else
                {
                    if (typeSignature.Is_GenericTypeParameter)
                    {
                        var genericTypeName = typeSignature.TypeName;

                        var genericTypeParameterPosition = genericTypeParameterPositionsByName[genericTypeName];

                        output = Instances.TypeNameOperator.Get_PositionalTypeName_ForGenericTypeParameter(genericTypeParameterPosition);
                    }
                    else if (typeSignature.Is_GenericMethodParameter)
                    {
                        var genericTypeName = typeSignature.TypeName;

                        var genericMethodParameterPosition = methodGenericTypeParameterPositionsByName[genericTypeName];

                        output = Instances.TypeNameOperator.Get_PositionalTypeName_ForGenericMethodParameter(genericMethodParameterPosition);
                    }
                    else
                    {
                        output = this.Get_NamespacedTypeName(
                            typeSignature.NamespaceName,
                            typeSignature.TypeName);
                    }
                }
            }

            var genericTypeInputsListToken = isInParameterContext
                ? this.Get_TypeParameterList(
                    typeSignature,
                    genericTypeParameterPositionsByName,
                    methodGenericTypeParameterPositionsByName)
                : Instances.SignatureStringOperator.Get_GenericTypeParameterCountToken(typeSignature.GenericTypeInputs)
                ;

            output = Instances.SignatureStringOperator.Append_TypeParameterList(
                output,
                genericTypeInputsListToken);

            return output;

        }

        public string Get_TypeParameterList(
            TypeSignature declaringTypeSignature,
            IDictionary<string, int> genericTypeParameterPositionsByName,
            IDictionary<string, int> methodGenericTypeParameterPositionsByName)
        {
            var genericTypeParameters = declaringTypeSignature.GenericTypeInputs;

            var isNull = Instances.NullOperator.Is_Null(genericTypeParameters);
            if (isNull)
            {
                genericTypeParameters = Instances.ArrayOperator.Empty<TypeSignature>();
            }

            var isEmpty = Instances.ArrayOperator.Is_Empty(genericTypeParameters);
            if (isEmpty)
            {
                return Instances.Strings.Empty;
            }

            var output = genericTypeParameters
                .Select(parameter => this.Get_TypeParameterToken(
                    parameter,
                    genericTypeParameterPositionsByName,
                    methodGenericTypeParameterPositionsByName))
                .Join(Instances.TokenSeparators.ArgumentListSeparator)
                .Wrap(
                    Instances.TokenSeparators.TypeArgumentListOpenTokenSeparator,
                    Instances.TokenSeparators.TypeArgumentListCloseTokenSeparator);

            return output;
        }

        public string Get_TypeParameterToken(
            TypeSignature typeParameter,
            IDictionary<string, int> genericTypeParameterPositionsByName,
            IDictionary<string, int> methodGenericTypeParameterPositionsByName)
        {
            if (typeParameter.Is_GenericTypeParameter)
            {
                var index = genericTypeParameterPositionsByName[typeParameter.TypeName];

                var output = Instances.TypeNameOperator.Get_PositionalTypeName_ForGenericTypeParameter(index);
                return output;
            }
            else if (typeParameter.Is_GenericMethodParameter)
            {
                var index = methodGenericTypeParameterPositionsByName[typeParameter.TypeName];

                var output = Instances.TypeNameOperator.Get_PositionalTypeName_ForGenericMethodParameter(index);
                return output;
            }
            else
            {
                var parameterTypeIdentityStringValue = this.Get_NamespacedTypeName(
                    typeParameter,
                    genericTypeParameterPositionsByName,
                    methodGenericTypeParameterPositionsByName,
                    true);

                // For identity names, the parameter name is not included.
                var output = parameterTypeIdentityStringValue;
                return output;
            }
        }

        public string Get_NamespacedTypeName_ForHasElementType(
            TypeSignature typeSignature,
            IDictionary<string, int> genericTypeParameterPositionsByName,
            IDictionary<string, int> methodGenericTypeParameterPositionsByName,
            bool isInParameterContext)
        {
            var elementTypeNamespacedTypeName = this.Get_NamespacedTypeName(
                typeSignature.ElementType,
                genericTypeParameterPositionsByName,
                methodGenericTypeParameterPositionsByName,
                isInParameterContext);

            var output = Instances.IdentityStringOperator.Append_ElementTypeRelationshipMarkers(
                elementTypeNamespacedTypeName,
                typeSignature.ElementTypeRelationships);

            return output;
        }

        public string Get_NamespacedTypeName_ForNestedType(
            TypeSignature typeSignature,
            IDictionary<string, int> genericTypeParameterPositionsByName,
            IDictionary<string, int> methodGenericTypeParameterPositionsByName,
            bool isInParameterContext)
        {
            var nestedParentNamespacedTypeName = this.Get_NamespacedTypeName(
                typeSignature.NestedTypeParent,
                genericTypeParameterPositionsByName,
                methodGenericTypeParameterPositionsByName,
                isInParameterContext);

            var typeName = typeSignature.TypeName;

            var output = Instances.IdentityStringOperator.Append_NestedTypeTypeName(
                nestedParentNamespacedTypeName,
                typeName);

            return output;
        }
    }
}
