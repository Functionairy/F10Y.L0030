using F10Y.L0000.Extensions;
using F10Y.T0002;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface ISignatureOperator
    {
        /// <summary>
        /// Handles all signature types.
        /// </summary>
        public bool Are_Equal_ByValue(Signature signatureA, Signature signatureB)
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
        public bool Are_Equal_ByValue_SignatureOnly<TSignature>(TSignature a, TSignature b,
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

        public string Get_FullTypeName(TypeSignature typeSignature)
            => Instances.TypeNameOperator.Get_FullTypeName(typeSignature);

        /// <summary>
        /// If there are no generic type inputs, the empty string is returned.
        /// </summary>
        public string Get_GenericTypeInputsList(TypeSignature[] genericTypeInputs)
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

        public Dictionary<string, int> Get_GenericTypeParameterPositionsByName(TypeSignature typeSignature)
        {
            var typeGenericTypeInputs = Instances.TypeSignatureOperator.Get_NestedTypeGenericTypeInputs(typeSignature);

            var typeIndex = 0;
            var output = typeGenericTypeInputs
                .ToDictionary(
                    x => x.TypeName,
                    x => typeIndex++);

            return output;
        }

        public string Get_IdentityString(Signature signature)
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

        public string Get_IdentityString_ForEvent(EventSignature eventSignature)
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

        public string Get_IdentityString_ForField(FieldSignature fieldSignature)
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

        public string Get_IdentityString_ForMethod(MethodSignature methodSignature)
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

        public string Get_IdentityString_ForProperty(PropertySignature propertySignature)
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

        public string Get_IdentityString_ForType(TypeSignature typeSignature)
        {
            string typeIdentityStringValue = Instances.NamespacedTypeNameOperator.Get_NamespacedTypeName(
                typeSignature,
                false);

            var output = Instances.IdentityStringOperator.Get_TypeIdentityString(typeIdentityStringValue);
            return output;
        }

        public Dictionary<string, int> Get_MethodGenericTypeParameterPositionsByName(TypeSignature[] genericTypeInputs)
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

        public string Get_ParametersList<TMethodBase>(
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

        public string Get_ParametersList(
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
        public string Get_ParametersList_ForMethod(MethodSignature methodSignature)
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
        public string Get_ParametersList_ForProperty(PropertySignature propertySignature)
        {
            var output = this.Get_ParametersList(
                propertySignature.Parameters,
                // For properties, if there are no parameters, there is no parameter list open-close parenthesis pair.
                Instances.Strings.Empty);

            return output;
        }

        public string Get_ParameterToken(MethodParameterSignature parameter)
        {
            var parameterTypeIdentityString = this.Get_FullTypeName(parameter.ParameterType);

            var output = Instances.SignatureStringOperator.Append_ParameterName(
                parameterTypeIdentityString,
                parameter.ParameterName);

            return output;
        }

        public string Get_ParameterToken(
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

        public string Get_ParameterTokenTypeSignatureString(
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

        public Signature Get_Signature(MemberInfo memberInfo)
            => Instances.MemberInfoOperator.Get_Signature(memberInfo);

        public TypeSignature Get_Signature_ForType(Type type)
            => Instances.MemberInfoOperator.Get_TypeSignature(type);

        public TypeSignature Get_Signature_ForType(
            Type type,
            bool includeAttributes)
            => Instances.MemberInfoOperator.Get_TypeSignature(
                type,
                includeAttributes);

        public string Get_SignatureString(Signature signature)
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
                .Select(Instances.SignatureStringOperator.Get_SignatureString)
                .Now();

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

        public AttributeSignature Get_Signature_ForAttribute(CustomAttributeData attribute)
            => Instances.AttributeOperator.Get_AttributeSignature(attribute);

        public string Get_SignatureString_ForEvent(EventSignature eventSignature)
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

        public string Get_SignatureString_ForField(FieldSignature fieldSignature)
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

        public string Get_SignatureString_ForMethod(MethodSignature methodSignature)
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

        public string Get_SignatureString_ForProperty(PropertySignature propertySignature)
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

        public string Get_SignatureString_ForType(TypeSignature typeSignature)
        {
            var typeName = this.Get_FullTypeName(typeSignature);

            // No adjustment necessary.
            var typeSignatureStringValue = typeName;

            var output = Instances.SignatureStringOperator.Get_TypeSignatureString(typeSignatureStringValue);
            return output;
        }

        public void SignatureTypeSwitch(
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

        public TOutput SignatureTypeSwitch<TOutput>(
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

        public TOutput SignatureTypeSwitch<TOutput>(
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
