using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using F10Y.T0002;


namespace F10Y.L0030
{
    /// <summary>
    /// All methdods related to signatures:
    /// <list type="bullet">
    /// <item>From <see cref="MemberInfo"/></item>
    /// <item>To signature string from <see cref="Signature"/></item>
    /// <item>To <see cref="Signature"/> from signature string.</item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <inheritdoc cref="Documentation.Project_SelfDescription" path="/summary"/>
    /// </remarks>
    [FunctionsMarker]
    public partial interface ISignatureOperator
    {
        Signature Get_Signature(string signatureString)
        {
            var kindMarker = this.Get_KindMarker(signatureString);

            var signatureStringValueMaybeObsolete = this.Get_SignatureStringValue(signatureString);

            var hasAttributes = Instances.SignatureStringOperator.Has_Attributes(
                signatureStringValueMaybeObsolete,
                out var nonAttributedSegment,
                out var attributeListSegment_OrDefault);

            Signature output = kindMarker switch
            {
                IKindMarkers.Error_Constant => throw Instances.ExceptionOperator.Get_ErrorSignatureDoesNotExistException(),
                IKindMarkers.Event_Constant => this.Get_EventSignature_Internal(nonAttributedSegment),
                IKindMarkers.Field_Constant => this.Get_FieldSignature_Internal(nonAttributedSegment),
                IKindMarkers.Method_Constant => this.Get_MethodSignature_Internal(nonAttributedSegment),
                IKindMarkers.Namespace_Constant => throw Instances.ExceptionOperator.Get_NamespaceSignatureDoesNotExistException(),
                IKindMarkers.Property_Constant => this.Get_PropertySignature_Internal(nonAttributedSegment),
                IKindMarkers.Type_Constant => this.Get_TypeSignature_Internal(nonAttributedSegment),
                _ => throw Instances.ExceptionOperator.Get_UnrecognizedKindMarkerException(kindMarker)
            };

            if(hasAttributes)
            {
                output.Attributes = Instances.SignatureStringOperator.Parse_AttributeSignatureList(attributeListSegment_OrDefault);
            }

            return output;
        }

        PropertySignature Get_PropertySignature_Internal(string propertySignatureStringValue)
        {
            (string signatureStringPart, string outputTypeName) = this.Get_OutputTypeParts(propertySignatureStringValue);

            var propertyType = this.Get_TypeSignature_Internal(outputTypeName);

            var (declaringTypeSignature, modifiedPropertyName, parameterListValue) = this.Decompose_PropertySignatureSegments(signatureStringPart);

            var declaringType = this.Get_TypeSignature_Internal(declaringTypeSignature);

            var parameters = this.Parse_MethodParameters(parameterListValue);

            var propertyName = this.Modify_MemberName_ForMemberName(modifiedPropertyName);

            var output = new PropertySignature
            {
                DeclaringType = declaringType,
                Parameters = parameters,
                PropertyName = propertyName,
                PropertyType = propertyType,
                //IsObsolete = , // Handled in top-level caller.
            };

            return output;
        }

        /// <summary>
        /// Does the signature string contain the explicit implementation namespace token separator (<see cref="L0000.ITokenSeparators.ExplicitImplementationNamespaceTokenSeparator"/>)?
        /// </summary>
        bool Is_ExplicitlyImplemented(string signatureStringValue)
        {
            var output = Instances.StringOperator.Contains(
                signatureStringValue,
                Instances.TokenSeparators.ExplicitImplementationNamespaceTokenSeparator);

            return output;
        }

        (
            string declaringTypeNamespacedTypeName,
            string propertyName,
            string parameterListValue)
            Decompose_PropertySignatureSegments(string propertySignatureValue)
        {
            var isExplicitlyImplemented = this.Is_ExplicitlyImplemented(propertySignatureValue);

            // Properties may not have parameter list open and close tokens.
            int indexOfParameterListOpenTokenSeparator_OrNotFound;
            if (isExplicitlyImplemented)
            {
                var lastIndexOfExplicitImplementationNamespaceTokenSeparator = Instances.StringOperator.Get_LastIndexOf(
                    Instances.TokenSeparators.ExplicitImplementationNamespaceTokenSeparator,
                    propertySignatureValue);

                indexOfParameterListOpenTokenSeparator_OrNotFound = Instances.StringOperator.Get_LastIndexOf_OrNotFound(
                    Instances.TokenSeparators.ParameterListOpenTokenSeparator,
                    propertySignatureValue,
                    lastIndexOfExplicitImplementationNamespaceTokenSeparator);
            }
            else
            {
                indexOfParameterListOpenTokenSeparator_OrNotFound = Instances.StringOperator.Get_LastIndexOf_OrNotFound(
                    Instances.TokenSeparators.ParameterListOpenTokenSeparator,
                    propertySignatureValue);
            }

            var (nonParameterListedPropertySignatureValue, parameterList) = Instances.StringOperator.Partition_Inclusive_OnSecondPart_OrFirstPartIfNotFound(
                indexOfParameterListOpenTokenSeparator_OrNotFound,
                propertySignatureValue);

            var parameterListValue = this.Get_ParameterListValue(parameterList);

            // Property names do not have generic type lists.
            // Thus the last namespace token is the property name.
            var indexOfLastNamespaceTokenSeparator = Instances.StringOperator.Get_LastIndexOf(
                Instances.TokenSeparators.NamespaceNameTokenSeparator,
                nonParameterListedPropertySignatureValue);

            var (declaringTypeNamespacedTypeName, propertyName) = Instances.StringOperator.Partition(
                indexOfLastNamespaceTokenSeparator,
                nonParameterListedPropertySignatureValue);

            return (declaringTypeNamespacedTypeName, propertyName, parameterListValue);
        }

        /// <summary>
        /// Removes the leading and trailing open and close parameter list token separators.
        /// Note: can handle null and empty strings; returns empty if empty or null.
        /// </summary>
        string Get_ParameterListValue(string parameterList)
        {
            parameterList = Instances.StringOperator.Empty_IfNull(parameterList);

            var output = parameterList
                .TrimStart(Instances.TokenSeparators.ParameterListOpenTokenSeparator)
                .TrimEnd(Instances.TokenSeparators.ParameterListCloseTokenSeparator)
                ;

            return output;
        }

        (
            string declaringTypeNamespacedTypeName,
            string modifiedMethodName,
            string methodGenericTypesListValue,
            string parameterListValue)
            Decompose_MethodSignatureSegments(string parameterListedMethodSignatureValue)
        {
            // All method signatures have open-close parentheses (for the parameter list).
            var indexOfParameterListOpenTokenSeparator = Instances.StringOperator.Get_LastIndexOf(
                Instances.TokenSeparators.ParameterListOpenTokenSeparator,
                parameterListedMethodSignatureValue);

            var (nonParameterListedMethodSignatureValue, parameterList) = Instances.StringOperator.Partition_Inclusive_OnSecondPart(
                indexOfParameterListOpenTokenSeparator,
                parameterListedMethodSignatureValue);

            var parameterListValue = this.Get_ParameterListValue(parameterList);

            // Work backwards, starting from the parameter list open token separator, until we have reached a namespace token separator that is not inside a generic type parameter list.
            // If the signature string contains a '#' (hash) character, then the method name is that of an explicitly implemented member.

            var isExplicitlyImplementedMember = Instances.StringOperator.Contains(
                nonParameterListedMethodSignatureValue,
                Instances.TokenSeparators.ExplicitImplementationNamespaceTokenSeparator);

            var indexOfMethodNameNamespaceTokenSeparator = isExplicitlyImplementedMember
                ? Instances.StringOperator.Get_IndexOf(
                    nonParameterListedMethodSignatureValue,
                    Instances.TokenSeparators.ExplicitImplementationNamespaceTokenSeparator)
                : indexOfParameterListOpenTokenSeparator - 1
                ;

            int countOfGenericTypeListOpenTokens = 0;
            int countOfGenericTypeListCloseTokens = 0;
            bool inGenericTypeParameterList = false;
            var uptoIndex = Instances.StringOperator.Get_Substring_Upto_Inclusive(
                indexOfMethodNameNamespaceTokenSeparator,
                nonParameterListedMethodSignatureValue);
            foreach (var character in uptoIndex.Reverse())
            {
                if (character == Instances.TokenSeparators.GenericTypeListCloseTokenSeparator)
                {
                    countOfGenericTypeListCloseTokens++;

                    if (!inGenericTypeParameterList)
                    {
                        // We are going in reverse, so close will come before open.
                        inGenericTypeParameterList = true;
                    }
                }

                if (character == Instances.TokenSeparators.GenericTypeListOpenTokenSeparator)
                {
                    countOfGenericTypeListOpenTokens++;

                    if (countOfGenericTypeListOpenTokens == countOfGenericTypeListCloseTokens)
                    {
                        // Reset.
                        inGenericTypeParameterList = false;
                        countOfGenericTypeListCloseTokens = 0;
                        countOfGenericTypeListOpenTokens = 0;
                    }
                }

                if (character == Instances.TokenSeparators.NamespaceNameTokenSeparator
                    && !inGenericTypeParameterList)
                {
                    break;
                }

                indexOfMethodNameNamespaceTokenSeparator--;
            }

            var (declaringTypeNamespacedTypeName, methodNameToken) = Instances.StringOperator.Partition(
                indexOfMethodNameNamespaceTokenSeparator,
                nonParameterListedMethodSignatureValue);

            var indexOfGenericTypeListOpenTokenSeparatorOrNotFound = Instances.StringOperator.Get_IndexOf_OrNotFound(
                methodNameToken,
                Instances.TokenSeparators.GenericTypeListOpenTokenSeparator);

            var (modifiedMethodName, methodGenericTypesList) = Instances.StringOperator.Partition_Inclusive_OnSecondPart_OrFirstPartIfNotFound(
                indexOfGenericTypeListOpenTokenSeparatorOrNotFound,
                methodNameToken);

            var methodGenericTypesListValue = this.Get_GenericTypeListValue(methodGenericTypesList);

            return (
                declaringTypeNamespacedTypeName,
                modifiedMethodName,
                methodGenericTypesListValue,
                parameterListValue);
        }

        /// <summary>
        /// Note: can handle null and empty generic type lists (return empty).
        /// </summary>
        string Get_GenericTypeListValue(string genericTypeList)
        {
            var nonNullGenericTypeList = Instances.StringOperator.Empty_IfNull(genericTypeList);

            var output = nonNullGenericTypeList
                .TrimStart(Instances.TokenSeparators.GenericTypeListOpenTokenSeparator)
                .TrimEnd(Instances.TokenSeparators.GenericTypeListCloseTokenSeparator)
                ;

            return output;
        }

        MethodSignature Get_MethodSignature_Internal(string methodSignatureStringValue)
        {
            (string signatureStringPart, string outputTypeName) = this.Get_OutputTypeParts(methodSignatureStringValue);

            // Does the method have a return type (constructor methods do not).
            var hasReturnType = outputTypeName != null;

            var returnType = hasReturnType
                ? this.Get_TypeSignature_Internal(outputTypeName)
                : null
                ;

            var (declaringTypeSignature, modifiedMethodName, methodGenericTypesListValue, parameterListValue) = this.Decompose_MethodSignatureSegments(signatureStringPart);

            var declaringType = this.Get_TypeSignature_Internal(declaringTypeSignature);

            // If the method is a contructor, the return type is the declaring type.
            var isConstructor = modifiedMethodName == "#ctor";
            if (isConstructor)
            {
                returnType = declaringType;
            }

            var parameters = this.Parse_MethodParameters(parameterListValue);

            var methodGenericTypeInputs = this.Parse_TypesListValue(methodGenericTypesListValue);

            var methodName = this.Modify_MemberName_ForMemberName(modifiedMethodName);

            var output = new MethodSignature
            {
                DeclaringType = declaringType,
                MethodName = methodName,
                GenericTypeInputs = methodGenericTypeInputs,
                Parameters = parameters,
                ReturnType = returnType,
                //IsObsolete = , // Handled in top-level caller.
            };

            return output;
        }

        MethodParameterSignature[] Parse_MethodParameters(string methodParametersListValue)
        {
            var listItems = this.Get_ListItems(methodParametersListValue);

            var output = listItems
                .Select(listItem =>
                {
                    // The parameter name token separator (' ', space) might also be present in the generic types list separator (', ', comma-space).
                    // The parameter name will never have spaces in it, so the last parameter name token separator.
                    var lastIndexOfParameterNameTokenSeparator = Instances.StringOperator.Get_LastIndexOf(
                        Instances.TokenSeparators.ParameterNameTokenSeparator,
                        listItem);

                    var (parameterTypeNamespacedTypeName, parameterName) = Instances.StringOperator.Partition(
                        lastIndexOfParameterNameTokenSeparator,
                        listItem);

                    var parameterType = this.Get_TypeSignature_Internal(parameterTypeNamespacedTypeName);

                    var output = new MethodParameterSignature
                    {
                        ParameterName = parameterName,
                        ParameterType = parameterType,
                    };

                    return output;
                })
                .Now();

            return output;
        }

        FieldSignature Get_FieldSignature_Internal(string fieldSignatureStringValue)
        {
            (string signatureStringPart, string outputTypeName) = this.Get_OutputTypeParts(fieldSignatureStringValue);

            var (namespacedTypeName, modifiedFieldName) = this.Get_LastNamespaceParts(signatureStringPart);

            var declaringType = this.Get_TypeSignature_Internal(namespacedTypeName);
            var fieldType = this.Get_TypeSignature_Internal(outputTypeName);

            var fieldName = this.Modify_MemberName_ForMemberName(modifiedFieldName);

            var output = new FieldSignature
            {
                DeclaringType = declaringType,
                FieldType = fieldType,
                FieldName = fieldName,
                //IsObsolete = , // Handled in top-level caller.
            };

            return output;
        }

        bool Is_Array(string typeSignatureStringValue)
        {
            var output = Instances.StringOperator.Ends_With(
                typeSignatureStringValue,
                Instances.TypeNameAffixes.Array_Suffix);

            return output;
        }

        bool Is_Reference(string typeSignatureStringValue)
        {
            var output = Instances.StringOperator.Ends_With(
                typeSignatureStringValue,
                Instances.TypeNameAffixes.ByReference_Suffix_String);

            return output;
        }

        bool Is_Pointer(string typeSignatureStringValue)
        {
            var output = Instances.StringOperator.Ends_With(
                typeSignatureStringValue,
                Instances.TypeNameAffixes.Pointer_Suffix_String);

            return output;
        }

        string Get_ArrayElementType(string arrayTypeSignatureString)
        {
            var output = Instances.StringOperator.Except_Ending(
                arrayTypeSignatureString,
                Instances.TypeNameAffixes.Array_Suffix);

            return output;
        }

        string Get_ReferenceElementType(string referenceTypeSignatureString)
        {
            var output = Instances.StringOperator.Except_Ending(
                referenceTypeSignatureString,
                Instances.TypeNameAffixes.ByReference_Suffix_String);

            return output;
        }

        string Get_PointerElementType(string referenceTypeSignatureString)
        {
            var output = Instances.StringOperator.Except_Ending(
                referenceTypeSignatureString,
                Instances.TypeNameAffixes.Pointer_Suffix_String);

            return output;
        }

        bool If_HasElementType_SetElementTypeValues(
            string typeSignatureStringValue,
            TypeSignature output)
        {
            bool If_Array_SetArrayElementType(
                string typeSignatureStringValue,
                TypeSignature output)
            {
                var isArray = this.Is_Array(typeSignatureStringValue);
                if (isArray)
                {
                    output.ElementTypeRelationships |= ElementTypeRelationships.Array;

                    var elementTypeValue = this.Get_ArrayElementType(typeSignatureStringValue);

                    output.ElementType = this.Get_TypeSignature_Internal(elementTypeValue);
                }

                return isArray;
            }

            void If_Reference_SetReferenceElementType(
                string typeSignatureStringValue,
                TypeSignature output)
            {
                var isReference = this.Is_Reference(typeSignatureStringValue);
                if (isReference)
                {
                    output.ElementTypeRelationships |= ElementTypeRelationships.Reference;

                    var elementTypeValue = this.Get_ReferenceElementType(typeSignatureStringValue);

                    output.ElementType = this.Get_TypeSignature_Internal(elementTypeValue);
                }
            }

            void If_Pointer_SetPointerElementType(
                string typeSignatureStringValue,
                TypeSignature output)
            {
                var isPointer = this.Is_Pointer(typeSignatureStringValue);
                if (isPointer)
                {
                    output.ElementTypeRelationships |= ElementTypeRelationships.Pointer;

                    var elementTypeValue = this.Get_PointerElementType(typeSignatureStringValue);

                    output.ElementType = this.Get_TypeSignature_Internal(elementTypeValue);
                }
            }

            var hasElementType = this.Has_ElementType(typeSignatureStringValue);
            if (hasElementType)
            {
                output.Has_ElementType = true;

                // TODO: which comes first if you have a reference array (or array reference)???
                If_Array_SetArrayElementType(
                    typeSignatureStringValue,
                    output);

                If_Reference_SetReferenceElementType(
                    typeSignatureStringValue,
                    output);

                If_Pointer_SetPointerElementType(
                    typeSignatureStringValue,
                    output);
            }

            return hasElementType;
        }

        /// <summary>
        /// Does the type signature end with any of the element type name affixes (<see cref="ITypeNameAffixes.Array_Suffix"/>, <see cref="ITypeNameAffixes.ByReference_Suffix"/>, or <see cref="ITypeNameAffixes.Pointer_Suffix"/>).
        /// </summary>
        bool Has_ElementType(string typedSignatureString)
        {
            var output = Instances.StringOperator.Ends_WithAny(
                typedSignatureString,
                Instances.TypeNameAffixSets.All_Suffixes);

            return output;
        }

        /// <summary>
        /// Does the type signature start with the type parameter prefix, <inheritdoc cref="ITypeNameAffixes.TypeParameterMarker_Prefix" path="descendant::name"/>.
        /// </summary>
        bool Is_GenericTypeParameterTypeName(string typeSignatureStringValue)
        {
            var length = typeSignatureStringValue.Length;

            var lengthIsAtLeastTwo = length > 1;
            if (!lengthIsAtLeastTwo)
            {
                return false;
            }

            var firstCharacter = typeSignatureStringValue.First();
            var secondCharacter = Instances.EnumerableOperator.Get_Second(typeSignatureStringValue);

            var output = true
                && firstCharacter == Instances.TypeNameAffixes.TypeParameterMarker_Prefix
                // Need to check the second character to make sure it's not the type parameter marker again (in which case, this type signature string value is a method type signature string value).
                && secondCharacter != Instances.TypeNameAffixes.TypeParameterMarker_Prefix
                ;

            return output;
        }

        string Get_GenericTypeParameterTypeName(string typeSignatureStringValue)
        {
            // Remove the first character, which is the generic type parameter prefix.
            var output = Instances.StringOperator.Except_First(typeSignatureStringValue);
            return output;
        }

        string Get_GenericMethodParameterTypeName(string typeSignatureStringValue)
        {
            // Remove the first two characters, which are the generic method type parameter prefix.
            var output = Instances.StringOperator.Except_FirstTwo(typeSignatureStringValue);
            return output;
        }

        /// <summary>
        /// Get the indices of nested type name tokens separators that are not in a generic type inputs list.
        /// </summary>
        int[] Get_IndicesOfNestedTypeNameTokenSeparators(string typeSignatureStringValue)
        {
            var genericTypeInputListRanges = this.Get_GenericInputListRanges(typeSignatureStringValue);

            var output = this.Get_IndicesOfNestedTypeNameTokenSeparators(
                typeSignatureStringValue,
                genericTypeInputListRanges);

            return output;
        }

        /// <inheritdoc cref="Get_IndicesOfNestedTypeNameTokenSeparators(string)"/>
        int[] Get_IndicesOfNestedTypeNameTokenSeparators(
            string typeSignatureStringValue,
            Range[] genericTypeInputListRanges)
        {
            var indicesOfNestedTypeNameTokenSeparators = Instances.StringOperator.Get_IndicesOf_OrEmpty(
                typeSignatureStringValue,
                Instances.TokenSeparators.NestedTypeNameTokenSeparator);

            var output = indicesOfNestedTypeNameTokenSeparators
                .Where(index =>
                {
                    var anyRange = genericTypeInputListRanges
                        .Where(range => Instances.RangeOperator.Contains(
                            range,
                            index))
                        .Any();

                    var output = !anyRange;
                    return output;
                })
                .Now();

            return output;
        }

        /// <summary>
        /// Does the type signature start with the method type parameter prefix, <inheritdoc cref="ITypeNameAffixes.MethodTypeParameterMarker_Prefix" path="descendant::name"/>.
        /// </summary>
        bool Is_GenericMethodParameterTypeName(string typeSignatureStringValue)
        {
            var output = Instances.StringOperator.Starts_With(
                typeSignatureStringValue,
                Instances.TypeNameAffixes.TypeParameterMarker_Prefix);

            return output;
        }

        ///// <summary>
        ///// Does the type have a generic inputs list? (Does it contain the <see cref="ITokenSeparators.TypeArgumentListOpenTokenSeparator"/>.)
        ///// </summary>
        ///// <remarks>
        ///// This function will fail for compiler-generated names like "T:D8S.C0002.Deploy.IOperations+&lt;&gt;c__DisplayClass0_0".
        ///// </remarks>
        //bool Has_GenericInputsList_Simple(string typedSignatureString)
        //{
        //    var output = Instances.StringOperator.Contains(
        //        typedSignatureString,
        //        Instances.TokenSeparators.GenericTypeListOpenTokenSeparator);

        //    return output;
        //}

        /// <summary>
        /// Does the type have a generic inputs list? (Does it contain the <see cref="L0000.ITokenSeparators.TypeArgumentListOpenTokenSeparator"/>,
        /// following text, instead of following a namespace token separator like for compiler generated names?)
        /// </summary>
        /// <remarks>
        /// This function can handle compiler-generated names like "T:D8S.C0002.Deploy.IOperations+&lt;&gt;c__DisplayClass0_0".
        /// </remarks>
        bool Has_GenericInputsList(string typedSignatureString)
        {
            var genericInputListRanges = this.Get_GenericInputListRanges(typedSignatureString);

            var output = genericInputListRanges.Any();
            return output;
        }

        string Get_TypesListValue_FromTypesList(string typesList)
        {
            // Remove the first and last characters.
            var output = typesList[1..^1];
            return output;
        }

        TypeSignature[] Parse_TypesList(string typesList)
        {
            var typesListValue = this.Get_TypesListValue_FromTypesList(typesList);

            var output = this.Parse_TypesListValue(typesListValue);
            return output;
        }

        public string[] Get_ListItems(string listValue)
        {
            var isNullOrEmpty = Instances.StringOperator.Is_NullOrEmpty(listValue);
            if (isNullOrEmpty)
            {
                return Instances.ArrayOperator.Empty<string>();
            }

            int genericListOpenTokenSeparatorCount = 0;
            int genericListCloseTokenSeparatorCount = 0;

            int lastListItemSeparatorIndex = -1;

            bool inGenericList = false;

            var output = new List<string>();

            var index = 0;
            foreach (var character in listValue)
            {
                if (character == Instances.TokenSeparators.GenericTypeListOpenTokenSeparator)
                {
                    genericListOpenTokenSeparatorCount++;

                    if (!inGenericList)
                    {
                        inGenericList = true;
                    }
                }

                if (character == Instances.TokenSeparators.GenericTypeListCloseTokenSeparator)
                {
                    genericListCloseTokenSeparatorCount++;

                    if (genericListOpenTokenSeparatorCount == genericListCloseTokenSeparatorCount)
                    {
                        // Reset.
                        genericListOpenTokenSeparatorCount = 0;
                        genericListCloseTokenSeparatorCount = 0;

                        inGenericList = false;
                    }
                }

                if (character == Instances.TokenSeparators.ListItemSeparator
                    && !inGenericList)
                {
                    var item = Instances.StringOperator.Get_Substring_FromExclusive_ToExclusive(
                        lastListItemSeparatorIndex,
                        index,
                        listValue)
                        .Trim();

                    output.Add(item);

                    lastListItemSeparatorIndex = index;
                }

                index++;
            }

            var lastItem = Instances.StringOperator.Get_Substring_FromExclusive_ToExclusive(
                lastListItemSeparatorIndex,
                index,
                listValue)
                .Trim();

            output.Add(lastItem);

            return output.ToArray();
        }

        TypeSignature[] Parse_TypesListValue(string typesListValue)
        {
            var listItems = this.Get_ListItems(typesListValue);

            var output = listItems
                .Select(listItem =>
                {
                    var typeSignature = this.Get_TypeSignature_Internal(listItem);
                    return typeSignature;
                })
                .Now();

            return output;
        }

        public TypeSignature Get_TypeSignature(string typeSignatureString)
        {
            var kindMarker = this.Get_KindMarker(typeSignatureString);

            if (kindMarker != IKindMarkers.Type_Constant)
            {
                throw new Exception($"Type signature string did not begin with the type signature string marker.");
            }

            var typeSignatureStringValue = this.Get_SignatureStringValue(typeSignatureString);

            var output = this.Get_TypeSignature_Internal(typeSignatureStringValue);

            return output;
        }

        TypeSignature Get_TypeSignature_Internal(string typeSignatureStringValue)
        {
            var output = new TypeSignature();

            // Has element type.
            var hasElementType = this.If_HasElementType_SetElementTypeValues(
                typeSignatureStringValue,
                output);
            if (hasElementType)
            {
                // We are done.
                return output;
            }

            // Is this a type parameter of a type definition?
            var isGenericTypeParameterType = this.Is_GenericTypeParameterTypeName(typeSignatureStringValue);
            if (isGenericTypeParameterType)
            {
                var genericTypeParameterTypeName = this.Get_GenericTypeParameterTypeName(typeSignatureStringValue);

                output.Is_GenericTypeParameter = true;
                output.TypeName = genericTypeParameterTypeName;

                // Done.
                return output;
            }

            // Is this is type parameter of a method definition?
            var isGenericMethodParameterType = this.Is_GenericMethodParameterTypeName(typeSignatureStringValue);
            if (isGenericMethodParameterType)
            {
                var genericMethodParameterTypeName = this.Get_GenericMethodParameterTypeName(typeSignatureStringValue);

                output.Is_GenericMethodParameter = true;
                output.TypeName = genericMethodParameterTypeName;

                return output;
            }

            // Get namespace and type name.
            // Is the type nested?
            var isNested = this.Is_Nested(typeSignatureStringValue);
            if (isNested)
            {
                output.Is_Nested = true;

                var indicesOfNestedTypeNameTokenSeparators = this.Get_IndicesOfNestedTypeNameTokenSeparators(
                    typeSignatureStringValue);

                var indexOfLastNestedTypeTokenSeparator = indicesOfNestedTypeNameTokenSeparators.Max();

                var (nestedTypeParentTypeSignatureStringValue, nestedTypeName) = Instances.StringOperator.Partition(
                    indexOfLastNestedTypeTokenSeparator,
                    typeSignatureStringValue);

                output.NestedTypeParent = this.Get_TypeSignature_Internal(nestedTypeParentTypeSignatureStringValue);

                var hasGenericInputsList = this.Has_GenericInputsList(nestedTypeName);
                if (hasGenericInputsList)
                {
                    var genericInputsListRanges = this.Get_GenericInputListRanges(nestedTypeName);

                    // There should only be one for types!
                    var genericInputsListRange = genericInputsListRanges.Single();

                    var typeName = Instances.StringOperator.Get_Substring_Upto_Exclusive(
                        genericInputsListRange.Start.Value,
                        nestedTypeName);

                    var genericInputsList = nestedTypeName[genericInputsListRange];

                    output.GenericTypeInputs = this.Parse_TypesList(genericInputsList);
                    output.TypeName = typeName;
                }
                else
                {
                    output.TypeName = nestedTypeName;
                }

                // No namespace for nested types (their namespace is the namespace and type name of their parent type).

                // We are done.
                return output;
            }
            else
            {
                // Not nested.
                var hasGenericInputsList = this.Has_GenericInputsList(typeSignatureStringValue);
                if (hasGenericInputsList)
                {
                    var genericInputsListRanges = this.Get_GenericInputListRanges(typeSignatureStringValue);

                    // There should only be one for types!
                    var genericInputsListRange = genericInputsListRanges.Single();

                    var namespacedTypeName = Instances.StringOperator.Get_Substring_Upto_Exclusive(
                        genericInputsListRange.Start.Value,
                        typeSignatureStringValue);

                    var genericInputsList = typeSignatureStringValue[genericInputsListRange];

                    output.GenericTypeInputs = this.Parse_TypesList(genericInputsList);

                    var hasNamespace = this.Has_Namespace(namespacedTypeName);
                    if (hasNamespace)
                    {
                        var indexOfLastNamespaceTokenSeparator = Instances.StringOperator.Get_LastIndexOf(
                            Instances.TokenSeparators.NamespaceNameTokenSeparator,
                            namespacedTypeName);

                        var (namespaceName, typeName) = Instances.StringOperator.Partition(
                            indexOfLastNamespaceTokenSeparator,
                            namespacedTypeName);

                        output.NamespaceName = namespaceName;
                        output.TypeName = typeName;
                    }
                    else
                    {
                        output.TypeName = namespacedTypeName;
                    }
                }
                else
                {
                    var hasNamespace = this.Has_Namespace(typeSignatureStringValue);
                    if (hasNamespace)
                    {
                        var indexOfLastNamespaceTokenSeparator = Instances.StringOperator.Get_LastIndexOf(
                            Instances.TokenSeparators.NamespaceNameTokenSeparator,
                            typeSignatureStringValue);

                        var (namespaceName, typeName) = Instances.StringOperator.Partition(
                            indexOfLastNamespaceTokenSeparator,
                            typeSignatureStringValue);

                        output.NamespaceName = namespaceName;
                        output.TypeName = typeName;
                    }
                    else
                    {
                        output.TypeName = typeSignatureStringValue;
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// If the typed signature string part contains a namespace token separator (and it is before a generic type list open token separator, if present), then it contains a namespace name.
        /// </summary>
        public bool Has_Namespace(string typedSignatureStringPart)
        {
            var indexOfNamespaceTokenSeparatorOrNotFound = Instances.StringOperator.Get_IndexOf_OrNotFound(
                typedSignatureStringPart,
                Instances.TokenSeparators.NamespaceNameTokenSeparator);

            var namespaceTokenIsFound = Instances.StringOperator.Is_Found(indexOfNamespaceTokenSeparatorOrNotFound);
            if (namespaceTokenIsFound)
            {
                var indexOfGenericTypeListOpenTokenSeparatorOrNotFound = Instances.StringOperator.Get_IndexOf_OrNotFound(
                    typedSignatureStringPart,
                    Instances.TokenSeparators.GenericTypeListOpenTokenSeparator);

                var genericTypeListOpenTokenIsFound = Instances.StringOperator.Is_Found(indexOfGenericTypeListOpenTokenSeparatorOrNotFound);
                if (genericTypeListOpenTokenIsFound)
                {
                    var namespaceTokenSeparatorIsBeforeGenericTypeListOpen = indexOfNamespaceTokenSeparatorOrNotFound < indexOfGenericTypeListOpenTokenSeparatorOrNotFound;
                    if (namespaceTokenSeparatorIsBeforeGenericTypeListOpen)
                    {
                        // If the namespace token separator is before the generic type list open token, then the type signature string part has a namespace.
                        return true;
                    }
                    else
                    {
                        // If the generic type list open token is before the namespace token separator, then the type name itself does not have a namespace, but is generic,
                        // and one of its type arguments is a namespaced type name.
                        return false;
                    }
                }
                else
                {
                    // Since there is no generic type parameter list open token, but there is a namespace token separator, then the typed signature string part has a namespace.
                    return true;
                }
            }
            else
            {
                // There is no namespace token, so the typed signature string part does not have a namespace (it is either the type name, or the name of a generic type parameter).
                return false;
            }
        }

        /// <summary>
        /// Does the type signature string value contain the nested type name token separator?
        /// (Outside of a generic type input list, since otherwise one of the generic type inputs would be a nested type.)
        /// </summary>
        bool Is_Nested(string typeSignatureStringValue)
        {
            var genericTypeInputListRanges = this.Get_GenericInputListRanges(typeSignatureStringValue);

            var output = this.Is_Nested(
                typeSignatureStringValue,
                genericTypeInputListRanges);

            return output;
        }

        bool Is_Nested(
            string typeSignatureStringValue,
            Range[] genericTypeInputListRanges)
        {
            var indicesOfNestedTypeNameTokenSeparators = Instances.StringOperator.Get_IndicesOf_OrEmpty(
                typeSignatureStringValue,
                Instances.TokenSeparators.NestedTypeNameTokenSeparator);

            var output = indicesOfNestedTypeNameTokenSeparators
                .Where(index =>
                {
                    var anyIndicesInsideRange = genericTypeInputListRanges
                        .Where(range => Instances.RangeOperator.Contains(
                            range,
                            index))
                        .Any();

                    var output = !anyIndicesInsideRange;
                    return output;
                })
                .Any();

            return output;
        }

        /// <summary>
        /// For a signature string, get the (inclusive) start and end index ranges of generic type lists in the string.
        /// (Inclusive of the beginning and ending open and close token separators.)
        /// Note: generic type lists can have nested generic lists within. This method just returns the top-level list.
        /// </summary>
        Range[] Get_GenericInputListRanges(string signatureString)
        {
            static IEnumerable<Range> Internal(string signatureString)
            {
                var openTokenSeparatorCount = 0;
                var closeTokenSeparatorCount = 0;

                var indexOfStart = Instances.Indices.NotFound;

                bool isInGenericList = false;

                var index = 0;
                foreach (var character in signatureString)
                {
                    if (character == Instances.TokenSeparators.GenericTypeListOpenTokenSeparator)
                    {
                        openTokenSeparatorCount++;

                        if (!isInGenericList)
                        {
                            indexOfStart = index;

                            isInGenericList = true;
                        }
                    }

                    if (character == Instances.TokenSeparators.GenericTypeListCloseTokenSeparator)
                    {
                        closeTokenSeparatorCount++;

                        if (openTokenSeparatorCount == closeTokenSeparatorCount)
                        {
                            var indexOfEnd = index;

                            yield return new Range(
                                indexOfStart,
                                indexOfEnd + 1);

                            isInGenericList = false;
                        }
                    }

                    index++;
                }
            }

            var ranges = Internal(signatureString)
                .ToArray();

            // Post-processing.
            var output = ranges
                .Where(range =>
                {
                    // Even if the generic type list open token is found, if it found at the start of the typed signature string,
                    // or immediately following a namespace token separator or nested type token separator, then it is part of a compiler generated name.
                    var isFirst = Instances.IndexOperator.Is_First(range.Start.Value);
                    if (isFirst)
                    {
                        // We have a compiler-generated type name like <>c__DisplayClass0_0.
                        return false;
                    }
                    else
                    {
                        var precedingCharacter = Instances.StringOperator.Get_Character(
                            signatureString,
                            range.Start.Value - 1);

                        var precedingCharacterIsNameTokenSeparator = false
                            || Instances.TokenSeparators.NestedTypeNameTokenSeparator == precedingCharacter
                            || Instances.TokenSeparators.NamespaceNameTokenSeparator == precedingCharacter
                            ;

                        if (precedingCharacterIsNameTokenSeparator)
                        {
                            // If the preceding character is a name token separator, then we have one of the compiler generated type names like "D8S.C0002.Deploy.IOperations+<>c__DisplayClass0_0".
                            return false;
                        }

                        // Else, true.
                        return true;
                    }
                })
                .Now();

            return output;
        }

        EventSignature Get_EventSignature_Internal(string eventSignatureStringValue)
        {
            (string signatureStringPart, string outputTypeName) = this.Get_OutputTypeParts(eventSignatureStringValue);

            var (namespacedTypeName, modifiedEventName) = this.Get_LastNamespaceParts(signatureStringPart);

            var declaringType = this.Get_TypeSignature_Internal(namespacedTypeName);
            var eventHandlerType = this.Get_TypeSignature_Internal(outputTypeName);

            var eventName = this.Modify_MemberName_ForMemberName(modifiedEventName);

            var output = new EventSignature
            {
                DeclaringType = declaringType,
                EventHandlerType = eventHandlerType,
                EventName = eventName,
                //IsObsolete = , // Handled in top-level caller.
            };

            return output;
        }

        /// <inheritdoc cref="IMemberNameOperator.Modify_MemberName_ForMemberName(string)"/>
        string Modify_MemberName_ForMemberName(string name)
        {
            var output = Instances.MemberNameOperator.Modify_MemberName_ForMemberName(name);
            return output;
        }

        /// <summary>
        /// For many kinds of member (event, field), the last part of a namespace token separator separated string is the name of the member.
        /// </summary>
        (string firstPart, string lastPart) Get_LastNamespaceParts(string signatureStringPart)
        {
            var indexOfLastNamespaceTokenSeparator = Instances.StringOperator.Get_LastIndexOf(
                Instances.TokenSeparators.NamespaceNameTokenSeparator,
                signatureStringPart);

            var output = Instances.StringOperator.Split_OnIndex(
                indexOfLastNamespaceTokenSeparator,
                signatureStringPart);

            return output;
        }

        /// <summary>
        /// Can handle signatures that do not have output types (like constructor methods), in which case a null string is returned for the output type name.
        /// </summary>
        (string signatureStringPart, string outputTypeName) Get_OutputTypeParts(string signatureString)
        {
            var tokens = Instances.StringOperator.Split_OnCharacter(
                Instances.TokenSeparators.OutputTypeNameTokenSeparator,
                signatureString);

            var signatureStringPart = tokens[0];

            var hasOutputTypeName = tokens.Length > 1;

            var outputTypeName = hasOutputTypeName
                ? tokens[1]
                : null
                ;

            return (signatureStringPart, outputTypeName);
        }

        char Get_KindMarker(string signatureStringPart)
        {
            // The kind marker is always the first character.
            var output = signatureStringPart.First();
            return output;
        }

        string Get_SignatureStringValue(string signatureString)
        {
            var output = Instances.KindMarkerOperator.Remove_TypeKindMarker(signatureString);
            return output;
        }

        /// <summary>
        /// Handles all signature types.
        /// </summary>
        bool Are_Equal_ByValue(Signature signatureA, Signature signatureB)
        {
            var typeDeterminesEquality = Instances.TypeOperator.TypeCheckDeterminesEquality(signatureA, signatureB, out var typesAreEqual);
            if (typeDeterminesEquality)
            {
                return typesAreEqual;
            }
            // Now we know the derived types are the same.

            var output = this.SignatureTypeSwitch(
                signatureA,
                signatureB,
                Instances.EventSignatureOperator.Are_Equal_ByValue,
                Instances.FieldSignatureOperator.Are_Equal_ByValue,
                Instances.PropertySignatureOperator.Are_Equal_ByValue,
                Instances.MethodSignatureOperator.Are_Equal_ByValue,
                Instances.TypeSignatureOperator.Are_Equal_ByValue);

            return output;
        }

        /// <summary>
        /// Checks only the properties of the signature (does not handle all types like <see cref="Are_Equal_ByValue(Signature, Signature)"/>).
        /// Note: performs null check.
        /// </summary>
        bool Are_Equal_ByValue_SignatureOnly<TSignature>(TSignature a, TSignature b,
            Func<TSignature, TSignature, bool> equality)
            where TSignature : Signature
        {
            var output = Instances.NullOperator.NullCheckDeterminesEquality_Else(a, b,
                (a, b) =>
                {
                    var output = true;

                    output &= a.KindMarker == b.KindMarker;
                    output &= Instances.ArrayOperator.Are_Equal_OrderDependent(
                        a.Attributes,
                        b.Attributes,
                        Instances.AttributeSignatureOperator.Are_Equal_HanldeNulls);
                    output &= equality(a, b);

                    return output;
                });

            return output;
        }

        string Get_FullTypeName(TypeSignature typeSignature)
            => Instances.TypeNameOperator.Get_FullTypeName(typeSignature);

        /// <summary>
        /// If there are no generic type inputs, the empty string is returned.
        /// </summary>
        string Get_GenericTypeInputsList(TypeSignature[] genericTypeInputs)
        {
            var isNullOrEmpty = Instances.ArrayOperator.Is_NullOrEmpty(genericTypeInputs);
            if (isNullOrEmpty)
            {
                return String.Empty;
            }

            var output = genericTypeInputs
                .Select(genericTypeInput => Instances.TypeNameOperator.Get_FullTypeName(genericTypeInput))
                .Join(Instances.TokenSeparators.ArgumentListSeparator)
                .Wrap(
                    Instances.TokenSeparators.TypeArgumentListOpenTokenSeparator,
                    Instances.TokenSeparators.TypeArgumentListCloseTokenSeparator);

            return output;
        }

        Dictionary<string, int> Get_GenericTypeParameterPositionsByName(TypeSignature typeSignature)
        {
            var typeGenericTypeInputs = Instances.TypeSignatureOperator.Get_NestedTypeGenericTypeInputs(typeSignature);

            var typeIndex = 0;
            var output = typeGenericTypeInputs
                .ToDictionary(
                    x => x.TypeName,
                    x => typeIndex++);

            return output;
        }

        string Get_IdentityString(Signature signature)
        {
            var output = signature switch
            {
                EventSignature eventSignature => this.Get_IdentityString_ForEvent(eventSignature),
                FieldSignature fieldSignature => this.Get_IdentityString_ForField(fieldSignature),
                PropertySignature propertySignature => this.Get_IdentityString_ForProperty(propertySignature),
                MethodSignature methodSignature => this.Get_IdentityString_ForMethod(methodSignature),
                TypeSignature typeSignature => this.Get_IdentityString_ForType(typeSignature),
                _ => throw Instances.ExceptionOperator.Get_UnrecognizedSignatureType(signature)
            };

            return output;
        }

        string Get_IdentityString_ForEvent(EventSignature eventSignature)
        {
            var declaringTypeIdentityStringValue = Instances.NamespacedTypeNameOperator.Get_NamespacedTypeName(
                eventSignature.DeclaringType,
                false);

            var eventName = Instances.IdentityStringOperator.Modify_MemberName(eventSignature.EventName);

            var eventIdentityString = Instances.SignatureStringOperator.Combine(
                declaringTypeIdentityStringValue,
                eventName);

            var output = Instances.IdentityStringOperator.Get_EventIdentityString(eventIdentityString);
            return output;
        }

        string Get_IdentityString_ForField(FieldSignature fieldSignature)
        {
            var declaringTypeIdentityStringValue = Instances.NamespacedTypeNameOperator.Get_NamespacedTypeName(
                fieldSignature.DeclaringType,
                false);

            var fieldName = Instances.IdentityStringOperator.Modify_MemberName(fieldSignature.FieldName);

            var fieldIdentityString = Instances.SignatureStringOperator.Combine(
                declaringTypeIdentityStringValue,
                fieldName);

            var output = Instances.IdentityStringOperator.Get_FieldIdentityString(fieldIdentityString);
            return output;
        }

        string Get_IdentityString_ForMethod(MethodSignature methodSignature)
        {
            var declaringTypeIdentityStringValue = Instances.NamespacedTypeNameOperator.Get_NamespacedTypeName(
                methodSignature.DeclaringType,
                false);

            var methodName = Instances.IdentityStringOperator.Modify_MemberName(methodSignature.MethodName);

            var methodNameIdentityString = Instances.SignatureStringOperator.Combine(
                declaringTypeIdentityStringValue,
                methodName);

            var genericTypeInputsList = Instances.SignatureStringOperator.Get_GenericMethodTypeParameterCountToken(methodSignature.GenericTypeInputs);

            methodNameIdentityString = Instances.SignatureStringOperator.Append_TypeParameterList(
                methodNameIdentityString,
                genericTypeInputsList);

            var isConversionOperator = Instances.MethodNameOperator.Is_ConversionOperator(methodName);

            var parametersList = this.Get_ParametersList(
                methodSignature,
                methodSignature.GenericTypeInputs,
                isConversionOperator);

            methodNameIdentityString = Instances.SignatureStringOperator.Append_ParameterList(
                methodNameIdentityString,
                parametersList);

            // Special handling for explicit and implicit conversion operators.
            if (isConversionOperator)
            {
                var genericTypeParameterPositionsByName = this.Get_GenericTypeParameterPositionsByName(methodSignature.DeclaringType);

                var methodGenericTypeParameterPositionsByName = this.Get_MethodGenericTypeParameterPositionsByName(methodSignature.GenericTypeInputs);

                var outputTypeName = this.Get_ParameterTokenTypeSignatureString(
                    methodSignature.ReturnType,
                    genericTypeParameterPositionsByName,
                    methodGenericTypeParameterPositionsByName,
                    true);

                methodNameIdentityString = Instances.SignatureStringOperator.Append_OutputType(
                    methodNameIdentityString,
                    outputTypeName);
            }

            var output = Instances.IdentityStringOperator.Get_MethodIdentityString(methodNameIdentityString);
            return output;
        }

        string Get_IdentityString_ForProperty(PropertySignature propertySignature)
        {
            var declaringTypeIdentityStringValue = Instances.NamespacedTypeNameOperator.Get_NamespacedTypeName(
                propertySignature.DeclaringType,
                false);

            var propertyName = Instances.IdentityStringOperator.Modify_MemberName(propertySignature.PropertyName);

            var propertyNameIdentityString = Instances.SignatureStringOperator.Combine(
                declaringTypeIdentityStringValue,
                propertyName);

            // A property can never have generic type inputs.
            var methodGenericTypeInputs = Instances.ArrayOperator.Empty<TypeSignature>();

            var parametersList = this.Get_ParametersList(
                propertySignature,
                methodGenericTypeInputs,
                // A property can never be a conversion operator.
                false);

            propertyNameIdentityString = Instances.SignatureStringOperator.Append_ParameterList(
                propertyNameIdentityString,
                parametersList);

            var output = Instances.IdentityStringOperator.Get_PropertyIdentityString(propertyNameIdentityString);
            return output;
        }

        string Get_IdentityString_ForType(TypeSignature typeSignature)
        {
            string typeIdentityStringValue = Instances.NamespacedTypeNameOperator.Get_NamespacedTypeName(
                typeSignature,
                false);

            var output = Instances.IdentityStringOperator.Get_TypeIdentityString(typeIdentityStringValue);
            return output;
        }

        Dictionary<string, int> Get_MethodGenericTypeParameterPositionsByName(TypeSignature[] genericTypeInputs)
        {
            var methodGenericTypeInputs = Instances.NullOperator.Is_Null(genericTypeInputs)
                ? Instances.ArrayOperator.Empty<TypeSignature>()
                : genericTypeInputs
                ;

            var methodIndex = 0;
            var output = methodGenericTypeInputs
                .ToDictionary(
                    x => x.TypeName,
                    x => methodIndex++);

            return output;
        }

        string Get_ParametersList<TMethodBase>(
            TMethodBase methodSignature,
            TypeSignature[] genericTypeInputs,
            // There is special parameter list generic type parameter type name logic for implicit and explicit conversion operators.
            bool isConversionOperator)
            where TMethodBase : IHas_DeclaringType, IHas_Parameters
        {
            var parameters = methodSignature.Parameters;

            var isNull = parameters == default;
            if (isNull)
            {
                return String.Empty;
            }

            var hasParameter = parameters.Any();
            if (!hasParameter)
            {
                return String.Empty;
            }

            var genericTypeParameterPositionsByName = this.Get_GenericTypeParameterPositionsByName(methodSignature.DeclaringType);

            var methodGenericTypeParameterPositionsByName = this.Get_MethodGenericTypeParameterPositionsByName(genericTypeInputs);

            var output = parameters
                .Select(parameter => this.Get_ParameterToken(
                    parameter,
                    genericTypeParameterPositionsByName,
                    methodGenericTypeParameterPositionsByName,
                    isConversionOperator))
                .Join(Instances.TokenSeparators.ArgumentListSeparator)
                .Wrap(
                    Instances.TokenSeparators.ParameterListOpenTokenSeparator,
                    Instances.TokenSeparators.ParameterListCloseTokenSeparator);

            return output;
        }

        string Get_ParametersList(
            MethodParameterSignature[] methodParameters,
            string resultIfNullOrEmpty)
        {
            var isNullOrEmpty = Instances.ArrayOperator.Is_NullOrEmpty(methodParameters);
            if (isNullOrEmpty)
            {
                return resultIfNullOrEmpty;
            }

            var parameterListValue = methodParameters
               .Select(this.Get_ParameterToken)
               .Join(Instances.TokenSeparators.ArgumentListSeparator);

            var output = parameterListValue.Wrap(
                Instances.TokenSeparators.ParameterListOpenTokenSeparator,
                Instances.TokenSeparators.ParameterListCloseTokenSeparator);

            return output;
        }

        /// <summary>
        /// For methods, if there are no parameters, there is still a parameter list open-close parenthesis pair.
        /// </summary>
        string Get_ParametersList_ForMethod(MethodSignature methodSignature)
        {
            var output = this.Get_ParametersList(
                methodSignature.Parameters,
                // For methods, if there are no parameters, there is still a parameter list open-close parenthesis pair.
                Instances.Tokens.EmptyParameterListToken);

            return output;
        }

        /// <summary>
        /// For properties, if there are no parameters, there is no parameter list open-close parenthesis pair.
        /// </summary>
        string Get_ParametersList_ForProperty(PropertySignature propertySignature)
        {
            var output = this.Get_ParametersList(
                propertySignature.Parameters,
                // For properties, if there are no parameters, there is no parameter list open-close parenthesis pair.
                Instances.Strings.Empty);

            return output;
        }

        string Get_ParameterToken(MethodParameterSignature parameter)
        {
            var parameterTypeIdentityString = this.Get_FullTypeName(parameter.ParameterType);

            var output = Instances.SignatureStringOperator.Append_ParameterName(
                parameterTypeIdentityString,
                parameter.ParameterName);

            return output;
        }

        string Get_ParameterToken(
            MethodParameterSignature parameter,
            IDictionary<string, int> genericTypeParameterPositionsByName,
            IDictionary<string, int> methodGenericTypeParameterPositionsByName,
            bool isConversionOperator)
        {
            // Only the type matters for identity strings.
            var output = this.Get_ParameterTokenTypeSignatureString(
                parameter.ParameterType,
                genericTypeParameterPositionsByName,
                methodGenericTypeParameterPositionsByName,
                isConversionOperator);

            return output;
        }

        string Get_ParameterTokenTypeSignatureString(
            TypeSignature parameterTypeSignature,
            IDictionary<string, int> genericTypeParameterPositionsByName,
            IDictionary<string, int> methodGenericTypeParameterPositionsByName,
            bool isConversionOperator)
        {
            if (parameterTypeSignature.Has_ElementType)
            {
                var output = this.Get_ParameterTokenTypeSignatureString(
                    parameterTypeSignature.ElementType,
                    genericTypeParameterPositionsByName,
                    methodGenericTypeParameterPositionsByName,
                    isConversionOperator);

                output = Instances.IdentityStringOperator.Append_ElementTypeRelationshipMarkers(
                    output,
                    parameterTypeSignature.ElementTypeRelationships);

                return output;
            }

            if (parameterTypeSignature.Is_GenericTypeParameter)
            {
                // If the generic type parameter is in a conversion operator, special type name rules apply.
                if (isConversionOperator)
                {
                    var output = parameterTypeSignature.TypeName;
                    return output;
                }
                else
                {
                    var index = genericTypeParameterPositionsByName[parameterTypeSignature.TypeName];

                    var output = Instances.TypeNameOperator.Get_PositionalTypeName_ForGenericTypeParameter(index);
                    return output;
                }
            }
            else if (parameterTypeSignature.Is_GenericMethodParameter)
            {
                // No need to consider whether the method generic type parameter is in a conversion operator, since that can never happen.

                var index = methodGenericTypeParameterPositionsByName[parameterTypeSignature.TypeName];

                var output = Instances.TypeNameOperator.Get_PositionalTypeName_ForGenericMethodParameter(index);
                return output;
            }
            else
            {
                var parameterTypeIdentityStringValue = Instances.NamespacedTypeNameOperator.Get_NamespacedTypeName(
                    parameterTypeSignature,
                    genericTypeParameterPositionsByName,
                    methodGenericTypeParameterPositionsByName,
                    true);

                // For identity names, the parameter name is not included.
                var output = parameterTypeIdentityStringValue;
                return output;
            }
        }

        Signature Get_Signature(MemberInfo memberInfo)
            => Instances.MemberInfoOperator.Get_Signature(memberInfo);

        TypeSignature Get_Signature_ForType(Type type)
            => Instances.MemberInfoOperator.Get_TypeSignature(type);

        TypeSignature Get_Signature_ForType(
            Type type,
            bool includeAttributes)
            => Instances.MemberInfoOperator.Get_TypeSignature(
                type,
                includeAttributes);

        string Get_SignatureString(Signature signature)
        {
            var signatureString = signature switch
            {
                EventSignature eventSignature => this.Get_SignatureString_ForEvent(eventSignature),
                FieldSignature fieldSignature => this.Get_SignatureString_ForField(fieldSignature),
                PropertySignature propertySignature => this.Get_SignatureString_ForProperty(propertySignature),
                MethodSignature methodSignature => this.Get_SignatureString_ForMethod(methodSignature),
                TypeSignature typeSignature => this.Get_SignatureString_ForType(typeSignature),
                _ => throw Instances.ExceptionOperator.Get_UnrecognizedSignatureType(signature)
            };

            // Append attribute signtures.
            var attributeSignatureStrings = signature.Attributes
                ?.Select(Instances.SignatureStringOperator.Get_SignatureString)
                .Now()
                ?? Instances.EnumerableOperator.Empty<string>();

            var anyAttributes = attributeSignatureStrings.Any();

            var attributesToken = anyAttributes
                ? Instances.StringOperator.Concatenate(attributeSignatureStrings)
                    .Prefix_With(Instances.Characters.Space)
                : Instances.Strings.Empty
                ;

            var output = Instances.StringOperator.Concatenate(
                signatureString,
                attributesToken);

            return output;
        }

        AttributeSignature Get_Signature_ForAttribute(CustomAttributeData attribute)
            => Instances.AttributeOperator.Get_AttributeSignature(attribute);

        string Get_SignatureString_ForEvent(EventSignature eventSignature)
        {
            var declaringTypeName = this.Get_FullTypeName(eventSignature.DeclaringType);
            var eventHandlerTypeName = this.Get_FullTypeName(eventSignature.EventHandlerType);

            var eventName = Instances.SignatureStringOperator.Modify_MemberName_ForSignatureString(eventSignature.EventName);

            var eventSignatureString = Instances.SignatureStringOperator.Combine(
                declaringTypeName,
                eventName);

            eventSignatureString = Instances.SignatureStringOperator.Append_OutputType(
                eventSignatureString,
                eventHandlerTypeName);

            var output = Instances.SignatureStringOperator.Get_EventSignatureString(eventSignatureString);
            return output;
        }

        string Get_SignatureString_ForField(FieldSignature fieldSignature)
        {
            var declaringTypeName = this.Get_FullTypeName(fieldSignature.DeclaringType);
            var fieldTypeName = this.Get_FullTypeName(fieldSignature.FieldType);

            var fieldName = Instances.SignatureStringOperator.Modify_MemberName_ForSignatureString(fieldSignature.FieldName);

            var fieldSignatureString = Instances.SignatureStringOperator.Combine(
                declaringTypeName,
                fieldName);

            fieldSignatureString = Instances.SignatureStringOperator.Append_OutputType(
                fieldSignatureString,
                fieldTypeName);

            var output = Instances.SignatureStringOperator.Get_FieldSignatureString(fieldSignatureString);
            return output;
        }

        string Get_SignatureString_ForMethod(MethodSignature methodSignature)
        {
            var declaringTypeName = this.Get_FullTypeName(methodSignature.DeclaringType);
            var returnTypeName = this.Get_FullTypeName(methodSignature.ReturnType);

            var methodName = Instances.SignatureStringOperator.Modify_MemberName_ForSignatureString(methodSignature.MethodName);

            var methodNameSignatureString = Instances.SignatureStringOperator.Combine(
                declaringTypeName,
                methodName);

            var genericTypeInputsList = this.Get_GenericTypeInputsList(methodSignature.GenericTypeInputs);

            methodNameSignatureString = Instances.SignatureStringOperator.Append_TypeParameterList(
                methodNameSignatureString,
                genericTypeInputsList);

            var parametersList = this.Get_ParametersList_ForMethod(methodSignature);

            methodNameSignatureString = Instances.SignatureStringOperator.Append_ParameterList(
                methodNameSignatureString,
                parametersList);

            methodNameSignatureString = Instances.SignatureStringOperator.Append_OutputType(
                methodNameSignatureString,
                returnTypeName);

            var output = Instances.SignatureStringOperator.Get_MethodSignatureString(methodNameSignatureString);
            return output;
        }

        string Get_SignatureString_ForProperty(PropertySignature propertySignature)
        {
            var declaringTypeName = this.Get_FullTypeName(propertySignature.DeclaringType);
            var propertyTypeName = this.Get_FullTypeName(propertySignature.PropertyType);

            var propertyName = Instances.SignatureStringOperator.Modify_MemberName_ForSignatureString(propertySignature.PropertyName);

            var propertyNameSignatureString = Instances.SignatureStringOperator.Combine(
                declaringTypeName,
                propertyName);

            var parametersList = this.Get_ParametersList_ForProperty(propertySignature);

            propertyNameSignatureString = Instances.SignatureStringOperator.Append_ParameterList(
                propertyNameSignatureString,
                parametersList);

            propertyNameSignatureString = Instances.SignatureStringOperator.Append_OutputType(
                propertyNameSignatureString,
                propertyTypeName);

            var output = Instances.SignatureStringOperator.Get_PropertySignatureString(propertyNameSignatureString);
            return output;
        }

        string Get_SignatureString_ForType(TypeSignature typeSignature)
        {
            var typeName = this.Get_FullTypeName(typeSignature);

            // No adjustment necessary.
            var typeSignatureStringValue = typeName;

            var output = Instances.SignatureStringOperator.Get_TypeSignatureString(typeSignatureStringValue);
            return output;
        }

        void SignatureTypeSwitch(
            Signature signature,
            Action<EventSignature> eventSignatureAction,
            Action<FieldSignature> fieldSignatureAction,
            Action<PropertySignature> propertySignatureAction,
            Action<MethodSignature> methodSignatureAction,
            Action<TypeSignature> typeSignatureAction)
        {
            switch (signature)
            {
                case EventSignature eventSignature:
                    eventSignatureAction(eventSignature);
                    break;
                case FieldSignature fieldSignature:
                    fieldSignatureAction(fieldSignature);
                    break;
                case PropertySignature propertySignature:
                    propertySignatureAction(propertySignature);
                    break;
                case MethodSignature methodSignature:
                    methodSignatureAction(methodSignature);
                    break;
                case TypeSignature typeSignature:
                    typeSignatureAction(typeSignature);
                    break;
                default:
                    throw Instances.ExceptionOperator.Get_UnrecognizedSignatureType(signature);
            }
            ;
        }

        TOutput SignatureTypeSwitch<TOutput>(
            Signature signatureA,
            Signature signatureB,
            Func<EventSignature, EventSignature, TOutput> eventSignatureFunction,
            Func<FieldSignature, FieldSignature, TOutput> fieldSignatureFunction,
            Func<PropertySignature, PropertySignature, TOutput> propertySignatureFunction,
            Func<MethodSignature, MethodSignature, TOutput> methodSignatureFunction,
            Func<TypeSignature, TypeSignature, TOutput> typeSignatureFunction)
        {
            var output = this.SignatureTypeSwitch(
                signatureA,
                eventSignature => eventSignatureFunction(eventSignature, signatureB as EventSignature),
                fieldSignature => fieldSignatureFunction(fieldSignature, signatureB as FieldSignature),
                propertySignature => propertySignatureFunction(propertySignature, signatureB as PropertySignature),
                methodSignature => methodSignatureFunction(methodSignature, signatureB as MethodSignature),
                typeSignature => typeSignatureFunction(typeSignature, signatureB as TypeSignature));

            return output;
        }

        TOutput SignatureTypeSwitch<TOutput>(
            Signature signature,
            Func<EventSignature, TOutput> eventSignatureFunction,
            Func<FieldSignature, TOutput> fieldSignatureFunction,
            Func<PropertySignature, TOutput> propertySignatureFunction,
            Func<MethodSignature, TOutput> methodSignatureFunction,
            Func<TypeSignature, TOutput> typeSignatureFunction)
        {
            var output = signature switch
            {
                EventSignature eventSignature => eventSignatureFunction(eventSignature),
                FieldSignature fieldSignature => fieldSignatureFunction(fieldSignature),
                PropertySignature propertySignature => propertySignatureFunction(propertySignature),
                MethodSignature methodSignature => methodSignatureFunction(methodSignature),
                TypeSignature typeSignature => typeSignatureFunction(typeSignature),
                _ => throw Instances.ExceptionOperator.Get_UnrecognizedSignatureType(signature)
            };

            return output;
        }
    }
}
