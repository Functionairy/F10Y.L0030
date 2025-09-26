using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface ISignatureStringOperator
    {
        string Get_SignatureString(AttributeSignature attributeSignature)
        {
            var typeToken = Instances.SignatureOperator.Get_SignatureString_ForType(attributeSignature.Type);

            string Get_TypedArgumentToken(TypedArgumentSignature argument)
            {
                var typeToken = Instances.SignatureOperator.Get_SignatureString_ForType(argument.Type);
                var valueToken = argument.Value.ToString();

                var output = $"({typeToken}){valueToken}";
                return output;
            }

            string Get_NamedArgumentToken(NamedArgumentSignature argument)
            {
                var typedToken = Get_TypedArgumentToken(argument.TypedValue);
                var nameToken = argument.Name;

                var output = $"{nameToken} = {typedToken}";
                return output;
            }

            var constructorTokens = attributeSignature.ConstructorArguments
                .Select(Get_TypedArgumentToken)
                .Now();

            var namedTokens = attributeSignature.NamedArguments
                .Select(Get_NamedArgumentToken)
                .Now();

            var argumentTokens = Instances.EnumerableOperator.From_Enumerables(
                constructorTokens,
                namedTokens);

            var argumentsListToken = Instances.StringOperator.Join_AsList(
                argumentTokens);

            var output = $"[{typeToken}({argumentsListToken})]";
            return output;
        }

        /// <summary>
        /// Append does not insert a <see cref="L0000.ITokenSeparators.NamespaceNameTokenSeparator"/>.
        /// </summary>
        string Append(string part1, string part2)
        {
            var output = $"{part1}{part2}";
            return output;
        }

        string Append_NestedTypeTypeName(
            string parentTypeName,
            string nestedTypeName)
        {
            var output = this.Combine(
                parentTypeName,
                nestedTypeName,
                Instances.TokenSeparators.NestedTypeNameTokenSeparator_String);

            return output;
        }

        string Append_ObsoleteToken(string signatureStringValue)
        {
            var output = this.Append(
                signatureStringValue,
                Instances.Tokens.ObsoleteToken);

            return output;
        }

        string Append_OutputType(
            string signatureStringValue,
            string outputTypeName)
        {
            var output = this.Combine(
                signatureStringValue,
                outputTypeName,
                Instances.TokenSeparators._ForSignatureString.OutputTypeTokenSeparator_String);

            return output;
        }

        string Append_ParameterList(
            string signatureStringValue,
            string parameterListToken)
        {
            var output = this.Append(
                signatureStringValue,
                parameterListToken);

            return output;
        }

        string Append_ParameterName(
            string parameterTypeString,
            string parameterName)
        {
            var output = this.Combine(
                parameterTypeString,
                parameterName,
                Instances.TokenSeparators.ParameterNameTokenSeparator_String);

            return output;
        }

        string Append_TypeParameterList(
            string signatureStringValue,
            string typeParameterListToken)
        {
            var output = this.Append(
                signatureStringValue,
                typeParameterListToken);

            return output;
        }

        /// <summary>
        /// Combine inserts a <see cref="L0000.ITokenSeparators.NamespaceNameTokenSeparator"/>.
        /// </summary>
        string Combine(
            string part1,
            string part2)
        {
            var output = this.Combine(
                part1,
                part2,
                Instances.TokenSeparators.NamespaceNameTokenSeparator_String);

            return output;
        }

        string Combine(
            string part1,
            string part2,
            string separator)
        {
            var output = $"{part1}{separator}{part2}";
            return output;
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

        string Get_SignatureStringValue_WithoutObsolete_IfObsolete(
            string signatureStringValueMaybeObsolete,
            out bool isObsolete)
        {
            isObsolete = this.Is_Obsolete(signatureStringValueMaybeObsolete);

            var output = isObsolete
                ? this.Remove_ObsoleteToken(signatureStringValueMaybeObsolete)
                : signatureStringValueMaybeObsolete
                ;

            return output;
        }

        int Get_TokenIndexOf_MatchingGenericTypeListTokenSeparatorsCount(
            IEnumerable<string> tokens,
            int startTokenIndex_Inclusive)
        {
            // Include the initial token by excluding it from being skipped.
            var counter = Instances.IndexOperator.Get_Count_Exclusive(startTokenIndex_Inclusive);

            var tokensToCheck = tokens.Skip(counter);

            int openTokensCount = 0;
            int closeTokensCount = 0;

            foreach (var token in tokensToCheck)
            {
                openTokensCount += Instances.StringOperator.Get_CountOf(
                    Instances.TokenSeparators.GenericTypeListOpenTokenSeparator,
                    token);

                closeTokensCount += Instances.StringOperator.Get_CountOf(
                    Instances.TokenSeparators.GenericTypeListCloseTokenSeparator,
                    token);

                if (openTokensCount == closeTokensCount)
                {
                    return counter;
                }
                else
                {
                    counter++;
                }
            }

            // Will only get here if tokens are fundamentally unpaired.
            throw new Exception($"({openTokensCount}:{Instances.TokenSeparators.GenericTypeListOpenTokenSeparator}, {closeTokensCount}:{Instances.TokenSeparators.GenericTypeListCloseTokenSeparator}), unpaired counts.");
        }

        (int startTokenIndex, int endTokenIndex)
            Get_TokenIndicesOf_GenericTypeList(
            IEnumerable<string> tokens,
            int initialTokenIndex_Inclusive = 0)
        {
            // Include the initial token by excluding it from being skipped.
            var skipCount = Instances.IndexOperator.Get_Count_Exclusive(initialTokenIndex_Inclusive);

            var tokensToCheck = tokens.Skip(skipCount);

            var startTokenIndex = Instances.StringOperator.Get_TokenIndex_Containing(
                tokensToCheck,
                Instances.TokenSeparators.GenericTypeListOpenTokenSeparator);

            var endTokenIndex = this.Get_TokenIndexOf_MatchingGenericTypeListTokenSeparatorsCount(
                tokensToCheck,
                startTokenIndex);

            return (startTokenIndex, endTokenIndex);
        }

        bool Contains_GenericTypeListOpenToken(string signatureStringPart)
        {
            var output = Instances.StringOperator.Contains(
                signatureStringPart,
                Instances.TokenSeparators.GenericTypeListOpenTokenSeparator);

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

        /// <summary>
        /// For a signature string, get the generic type lists in the string.
        /// (Returns the list, inclusive of the beginning and ending open and close token separators.)
        /// Note: generic type lists can have nested generic lists within. This method just returns the top-level list.
        /// </summary>
        string[] Get_GenericInputListValues(string signatureString)
        {
            var genericListRanges = this.Get_GenericInputListRanges(signatureString);

            var output = genericListRanges
                .Select(range =>
                {
                    var indexOfStart = range.Start;
                    var indexOfEnd = range.End;

                    var genericList = Instances.StringOperator.Get_Substring_FromInclusive_ToInclusive(
                        indexOfStart.Value,
                        indexOfEnd.Value,
                        signatureString);

                    return genericList;
                })
                .ToArray();

            return output;
        }

        string Get_GenericInputList(string signatureString)
        {
            var output = this.Get_GenericInputListValues(signatureString)
                .First();

            return output;
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

        /// <summary>
        /// The value does not include the open and close tokens
        /// (<see cref="L0000.ITokenSeparators.GenericTypeListOpenTokenSeparator"/>, <see cref="L0000.ITokenSeparators.GenericTypeListCloseTokenSeparator"/>).
        /// </summary>
        string Get_GenericTypeListValue(string[] typedSignatureStringParts)
        {
            var indexOfFirstTypeNamePart = this.Get_IndexOfFirstTypeNamePart(typedSignatureStringParts);

            var firstTypeNamePart = typedSignatureStringParts[indexOfFirstTypeNamePart];

            var tokensForFirst = Instances.StringOperator.Split(
                Instances.TokenSeparators.GenericTypeListOpenTokenSeparator,
                firstTypeNamePart);

            var firstTypeNameListToken = tokensForFirst[1];

            var lastTypeNamePart = typedSignatureStringParts.Get_Last();

            var tokensForLast = Instances.StringOperator.Split(
                Instances.TokenSeparators.GenericTypeListCloseTokenSeparator,
                lastTypeNamePart);

            var lastTypeNameListToken = tokensForLast[0];

            var middleTokens = typedSignatureStringParts[(indexOfFirstTypeNamePart + 1)..^2];

            var tokens = Instances.EnumerableOperator.From(firstTypeNameListToken)
                .Append(middleTokens)
                .Append(lastTypeNameListToken)
                ;

            var output = Instances.StringOperator.Join(
                Instances.TokenSeparators.NamespaceNameTokenSeparator,
                tokens);

            return output;
        }

        string Get_GenericParameterCountToken(
            string parameterCountTokenSeparator,
            TypeSignature[] genericTypeInputs = default)
        {
            var isNull = genericTypeInputs == default;
            if (isNull)
            {
                return String.Empty;
            }

            var hasGenericTypeInputs = genericTypeInputs.Any();
            if (!hasGenericTypeInputs)
            {
                return String.Empty;
            }

            var genericTypeParameterCount = genericTypeInputs.Length;

            var output = Instances.IdentityStringOperator.Get_GenericParameterCountToken(
                genericTypeParameterCount,
                parameterCountTokenSeparator);

            return output;
        }

        /// <summary>
        /// Note: can handle the null and empty case (returns an empty string).
        /// </summary>
        string Get_GenericTypeParameterCountToken(TypeSignature[] genericTypeInputs = default)
        {
            var output = this.Get_GenericParameterCountToken(
                Instances.TokenSeparators.TypeParameterCountSeparator_String,
                genericTypeInputs);

            return output;
        }

        string Get_GenericTypeParameterTypeName(string typeSignatureStringValue)
        {
            // Remove the first character, which is the generic type parameter prefix.
            var output = typeSignatureStringValue.Except_First();
            return output;
        }

        string Get_GenericMethodParameterTypeName(string typeSignatureStringValue)
        {
            // Remove the first two characters, which are the generic method type parameter prefix.
            var output = typeSignatureStringValue.Except_FirstTwo();
            return output;
        }

        /// <summary>
        /// Note: can handle the null and empty case (returns an empty string).
        /// </summary>
        string Get_GenericMethodTypeParameterCountToken(TypeSignature[] genericTypeInputs = default)
        {
            var output = this.Get_GenericParameterCountToken(
                Instances.TokenSeparators.MethodTypeParameterCountSeparator,
                genericTypeInputs);

            return output;
        }

        /// <summary>
        /// The first type name part is either the last part of the array of typed signature string parts,
        /// or the first part that contains a generic type list open token.
        /// </summary>
        int Get_IndexOfFirstTypeNamePart(string[] typedSignatureStringParts)
        {
            for (int i = 0; i < typedSignatureStringParts.Length; i++)
            {
                var part = typedSignatureStringParts[i];

                var containsGenericTypeListOpenToken = this.Contains_GenericTypeListOpenToken(part);
                if (containsGenericTypeListOpenToken)
                {
                    return i;
                }
            }

            // Else, no generic type list open token was found.
            // Return the index of the last token.
            var output = typedSignatureStringParts.Get_IndexOfLast();
            return output;
        }

        char Get_KindMarker(string signatureStringPart)
        {
            // The kind marker is always the first character.
            var output = signatureStringPart.First();
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

            var output = Instances.StringOperator.Split_IntoParts(
                indexOfLastNamespaceTokenSeparator,
                signatureStringPart);

            return output;
        }

        string[] Get_ListItems(string listValue)
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

        string Get_NamespaceName(string[] typedSignatureStringParts)
        {
            // Given an array of typed signature string parts, the namespace name is the concatenation of all parts up to (but not including) the first type name part.
            // The first type name part is either the last part of the array of typed signature string parts, or the first part that contains a generic type list open token.

            var indexOfFirstTypeNamePart = this.Get_IndexOfFirstTypeNamePart(typedSignatureStringParts);

            var parts = typedSignatureStringParts[..indexOfFirstTypeNamePart];

            var output = Instances.StringOperator.Join(
                Instances.TokenSeparators.NamespaceNameTokenSeparator,
                parts);

            return output;
        }

        /// <summary>
        /// Can handle signatures that do not have output types (like constructor methods), in which case a null string is returned for the output type name.
        /// </summary>
        (string signatureStringPart, string outputTypeName) Get_OutputTypeParts(string signatureString)
        {
            var tokens = Instances.StringOperator.Split(
                Instances.TokenSeparators._ForSignatureString.OutputTypeTokenSeparator,
                signatureString);

            var signatureStringPart = tokens[0];

            var hasOutputTypeName = tokens.Length > 1;

            var outputTypeName = hasOutputTypeName
                ? tokens[1]
                : null
                ;

            return (signatureStringPart, outputTypeName);
        }

        string Get_SignatureString(MemberInfo member)
        {
            var signature = Instances.SignatureOperator.Get_Signature(member);

            var output = this.Get_SignatureString(signature);
            return output;
        }

        string Get_SignatureString(Signature signature)
        {
            var output = Instances.SignatureOperator.Get_SignatureString(signature);
            return output;
        }

        string Get_SignatureStringValue(string signatureString)
        {
            var output = Instances.KindMarkerOperator.Remove_TypeKindMarker(signatureString);
            return output;
        }

        string Get_EventSignatureString(string identityStringValue)
        {
            var output = Instances.KindMarkerOperator.Make_EventKindMarked(identityStringValue);
            return output;
        }

        string Get_FieldSignatureString(string identityStringValue)
        {
            var output = Instances.KindMarkerOperator.Make_FieldKindMarked(identityStringValue);
            return output;
        }

        string Get_MethodSignatureString(string identityStringValue)
        {
            var output = Instances.KindMarkerOperator.Make_MethodKindMarked(identityStringValue);
            return output;
        }

        string Get_PropertySignatureString(string identityStringValue)
        {
            var output = Instances.KindMarkerOperator.Make_PropertyKindMarked(identityStringValue);
            return output;
        }

        /// <summary>
        /// Prefixes the identity string with the type kind-marker value to get a type signature string.
        /// </summary>
        string Get_TypeSignatureString(string identityStringValue)
        {
            var output = Instances.KindMarkerOperator.Make_TypeKindMarked(identityStringValue);
            return output;
        }

        string Get_TypesListValue_FromTypesList(string typesList)
        {
            // Remove the first and last characters.
            var output = typesList[1..^1];
            return output;
        }

        /// <summary>
        /// Gets the simple type name.
        /// </summary>
        string Get_TypeName(string[] typedSignatureStringParts)
        {
            var indexOfFirstTypeNamePart = this.Get_IndexOfFirstTypeNamePart(typedSignatureStringParts);

            var firstTypeNamePart = typedSignatureStringParts[indexOfFirstTypeNamePart];

            var tokens = Instances.StringOperator.Split(
                Instances.TokenSeparators.GenericTypeListOpenTokenSeparator,
                firstTypeNamePart);

            var output = tokens[0];
            return output;
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

        /// <summary>
        /// Does the type have a generic inputs list? (Does it contain the <see cref="L0000.ITokenSeparators.TypeArgumentListOpenTokenSeparator"/>.)
        /// </summary>
        /// <remarks>
        /// This function will fail for compiler-generated names like "T:D8S.C0002.Deploy.IOperations+&lt;&gt;c__DisplayClass0_0".
        /// </remarks>
        bool Has_GenericInputsList_Simple(string typedSignatureString)
        {
            var output = Instances.StringOperator.Contains(
                typedSignatureString,
                Instances.TokenSeparators.GenericTypeListOpenTokenSeparator);

            return output;
        }

        /// <summary>
        /// If the typed signature string part contains a namespace token separator (and it is before a generic type list open token separator, if present), then it contains a namespace name.
        /// </summary>
        bool Has_Namespace(string typedSignatureStringPart)
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
            var secondCharacter = typeSignatureStringValue.Second();

            var output = true
                && firstCharacter == Instances.TypeNameAffixes.TypeParameterMarker_Prefix
                // Need to check the second character to make sure it's not the type parameter marker again (in which case, this type signature string value is a method type signature string value).
                && secondCharacter != Instances.TypeNameAffixes.TypeParameterMarker_Prefix
                ;

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

        /// <summary>
        /// Does the type signature string value contain the nested type name token separator?
        /// (Outside of a generic type input list, since otherwise one of the generic type inputs would be a nested type.)
        /// </summary>
        /// <remarks>
        /// Note: not appropriate for use, since types might have nested types inside their generic type input lists.
        /// </remarks>
        bool Is_Nested_Simple(string typeSignatureStringValue)
        {
            var output = Instances.StringOperator.Contains(
                typeSignatureStringValue,
                Instances.TokenSeparators.NestedTypeNameTokenSeparator);

            return output;
        }

        bool Is_Obsolete(string signatureStringValue)
        {
            var output = Instances.StringOperator.Ends_With(
                signatureStringValue,
                Instances.Tokens.ObsoleteToken);

            return output;
        }

        /// <inheritdoc cref="IMemberNameOperator.Modify_MemberName_ForSignatureString(string)"/>
        string Modify_MemberName_ForSignatureString(string name)
        {
            var output = Instances.MemberNameOperator.Modify_MemberName_ForSignatureString(name);
            return output;
        }

        /// <inheritdoc cref="IMemberNameOperator.Modify_MemberName_ForMemberName(string)"/>
        string Modify_MemberName_ForMemberName(string name)
        {
            var output = Instances.MemberNameOperator.Modify_MemberName_ForMemberName(name);
            return output;
        }

        string Remove_ObsoleteToken(string signatureStringWithObsoleteToken)
        {
            var output = Instances.StringOperator.Except_Ending_Strict(
                signatureStringWithObsoleteToken,
                Instances.Tokens.ObsoleteToken);

            return output;
        }
    }
}
