using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using F10Y.L0030.Extensions;
using F10Y.T0002;
using F10Y.T0011;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IMemberInfoOperator :
        L0000.IMemberInfoOperator
    {
#pragma warning disable IDE1006 // Naming Styles

        [Ignore]
        public L0000.IMemberInfoOperator _L0000 => L0000.MemberInfoOperator.Instance;

#pragma warning restore IDE1006 // Naming Styles


        public void Set_AttributeSignatures(
            Signature signature,
            MemberInfo memberInfo)
        {
            var attributes = this.Get_Attributes(memberInfo);

            signature.Attributes = attributes
                .Select(Instances.AttributeOperator.Get_AttributeSignature)
                .Now();
        }

        public Signature Get_Signature(MemberInfo memberInfo)
        {
            Signature output = memberInfo switch
            {
                ConstructorInfo constructorInfo => this.Get_MethodSignature_ForConstructor(constructorInfo),
                EventInfo eventInfo => this.Get_EventSignature(eventInfo),
                FieldInfo fieldInfo => this.Get_FieldSignature(fieldInfo),
                MethodInfo methodInfo => this.Get_MethodSignature(methodInfo),
                PropertyInfo propertyInfo => this.Get_PropertySignature(propertyInfo),
                TypeInfo typeInfo => this.Get_TypeSignature(typeInfo),
                _ => throw Instances.ExceptionOperator.Get_UnrecognizedMemberTypeException(memberInfo),
            };

            // Handle at the root.

            return output;
        }

        public EventSignature Get_EventSignature(EventInfo eventInfo)
        {
            var declaringType = Instances.EventInfoOperator.Get_DeclaringType(eventInfo);
            var eventHandlerType = Instances.EventInfoOperator.Get_EventHandlerType(eventInfo);
            var eventName = Instances.EventInfoOperator.Get_Name(eventInfo);

            var declaringTypeSignature = this.Get_TypeSignature(declaringType);
            var eventHandlerTypeSignature = this.Get_TypeSignature(eventHandlerType);

            var output = new EventSignature
            {
                DeclaringType = declaringTypeSignature,
                EventHandlerType = eventHandlerTypeSignature,
                EventName = eventName,
            }
            .Set_AttributeSignatures(eventInfo);

            return output;
        }

        public FieldSignature Get_FieldSignature(FieldInfo fieldInfo)
        {
            var declaringType = Instances.FieldInfoOperator.Get_DeclaringType(fieldInfo);
            var fieldType = Instances.FieldInfoOperator.Get_FieldType(fieldInfo);
            var fieldName = Instances.FieldInfoOperator.Get_FieldName(fieldInfo);

            var declaringTypeSignature = this.Get_TypeSignature(declaringType);
            var fieldTypeSignature = this.Get_TypeSignature(fieldType);

            var output = new FieldSignature
            {
                DeclaringType = declaringTypeSignature,
                FieldName = fieldName,
                FieldType = fieldTypeSignature,
            }
            .Set_AttributeSignatures(fieldInfo);

            return output;
        }

        public TypeSignature[] Get_GenericTypeInputTypeSignatures(
            IEnumerable<Type> genericTypeInputs)
        {
            var output = genericTypeInputs
                .Select(this.Get_TypeSignature)
                .Now();

            return output;
        }

        public TypeSignature Get_GenericTypeInputSignature(Type type)
        {
            var genericTypeName = Instances.TypeOperator.Get_GenericTypeParameterTypeName_ActualName(type);

            var isGenericTypeParameter = Instances.TypeOperator.Is_GenericTypeParameter(type);
            var isGenericMethodParameter = Instances.TypeOperator.Is_GenericMethodParamter(type);

            var output = new TypeSignature
            {
                Is_GenericMethodParameter = isGenericMethodParameter,
                Is_GenericTypeParameter = isGenericTypeParameter,
                TypeName = genericTypeName,
            };

            return output;
        }

        public MethodParameterSignature Get_MethodParameter(
            ParameterInfo parameterInfo)
        {
            var parameterType = Instances.ParameterInfoOperator.Get_ParameterType(parameterInfo);
            var parameterName = Instances.ParameterInfoOperator.Get_ParameterName(parameterInfo);

            var parameterTypeSignature = this.Get_TypeSignature(
                parameterType);

            var output = new MethodParameterSignature
            {
                ParameterName = parameterName,
                ParameterType = parameterTypeSignature,
            };

            return output;
        }

        public MethodParameterSignature[] Get_MethodParameters(
            IEnumerable<ParameterInfo> parameters)
        {
            var output = parameters
                .Select(this.Get_MethodParameter)
                .Now();

            return output;
        }

        public MethodSignature Get_MethodSignature(MethodInfo methodInfo)
        {
            // Constructors do not have method generic type parameters, and asking for them causes an exception.
            // So ask for them here so the method for method bases does not have to.
            var methodGenericTypeInputs = Instances.MethodBaseOperator.Get_GenericTypeInputs_OfMethodOnly(methodInfo);

            var output = this.Get_MethodSignature_ForMethodBase(
                methodInfo,
                methodGenericTypeInputs);

            var returnType = Instances.MethodInfoOperator.Get_ReturnType(methodInfo);

            var returnTypeSignature = this.Get_TypeSignature(
                returnType);

            output.ReturnType = returnTypeSignature;

            return output;
        }

        public MethodSignature Get_MethodSignature_ForConstructor(ConstructorInfo constructorInfo)
        {
            // Constructors cannot have generic type inputs.
            var genericTypeInputs = Instances.ArrayOperator.Empty<Type>();

            var output = this.Get_MethodSignature_ForMethodBase(
                constructorInfo,
                genericTypeInputs);

            // For constructors, the return type is the declaring type.
            output.ReturnType = output.DeclaringType;

            return output;
        }

        public MethodSignature Get_MethodSignature_ForMethodBase(
            MethodBase methodBase,
            // Will be empty for constructors, parameter must be here since constructors throw an exception when asked for generic type inputs.
            Type[] methodGenericTypeInputs)
        {
            var declaringType = Instances.MethodBaseOperator.Get_DeclaringType(methodBase);
            var methodName = Instances.MethodBaseOperator.Get_MethodName(methodBase);

            var declaringTypeSignature = this.Get_TypeSignature(
                declaringType);

            var parameters = Instances.MethodBaseOperator.Get_Parameters(
                methodBase);

            var methodParameters = this.Get_MethodParameters(
                parameters);

            var genericTypeInputs = methodGenericTypeInputs
                .Select(this.Get_TypeSignature)
                .ToArray();

            var output = new MethodSignature
            {
                DeclaringType = declaringTypeSignature,
                GenericTypeInputs = genericTypeInputs,
                MethodName = methodName,
                Parameters = methodParameters,
                //ReturnType // Handled in caller since method signatures for constructors should be different than for regular methods.
            }
            .Set_AttributeSignatures(methodBase);

            return output;
        }

        public TypeSignature Get_TypeSignature(Type type)
            => this.Get_TypeSignature(
                type,
                true);

        public TypeSignature Get_TypeSignature(
            Type type,
            bool includeAttributes)
        {
            // If the type is a generic type parameter, it must exist within the current generic type parameter context.
            var isGenericParameter = Instances.TypeOperator.Is_GenericParameter(type);
            if (isGenericParameter)
            {
                var genericTypeParameterTypeSignature = this.Get_GenericTypeInputSignature(type);
                return genericTypeParameterTypeSignature;
            }

            var output = new TypeSignature();

            // Is the type a nested type? If so, we consider the parent type of the nested type first.
            var isNested = Instances.TypeOperator.Is_NestedType(type);
            if (isNested)
            {
                output.Is_Nested = true;

                var nestedTypeParentType = Instances.TypeOperator.Get_NestedTypeParentType(type);

                // Ordinarily, generic type signature values (like whether a generic type parameter is a method parameter or type parameter)
                // can just be read from member objects, but for generic parent types of nested types, the generic type parameters don't flow through.
                // Instead you can just construct the generic type with the type parameters from the nested type.
                var nestedTypeParentIsGenericTypeDefintion = Instances.TypeOperator.Is_GenericTypeDefinition(nestedTypeParentType);
                if (nestedTypeParentIsGenericTypeDefintion)
                {
                    var nestedTypeGenericTypeInputs = Instances.TypeOperator.Get_GenericTypeInputs_OfType(type);

                    var nestedTypeParentTypeGenericTypeInputs = Instances.TypeOperator.Get_GenericTypeInputs_OfType(nestedTypeParentType);

                    // Make a type.
                    var nestedTypeParentTypeGenericInputCount = nestedTypeParentTypeGenericTypeInputs.Length;

                    var nestedTypeGenericTypeArgumentsForParent = nestedTypeGenericTypeInputs.Take(nestedTypeParentTypeGenericInputCount).ToArray();

                    var constructedNestedTypeParentType = nestedTypeParentType.MakeGenericType(nestedTypeGenericTypeArgumentsForParent);

                    var nestedTypeParentTypeSignature = this.Get_TypeSignature(
                        constructedNestedTypeParentType);

                    output.NestedTypeParent = nestedTypeParentTypeSignature;
                }
                else
                {
                    var nestedTypeParentTypeSignature = this.Get_TypeSignature(
                        nestedTypeParentType);

                    output.NestedTypeParent = nestedTypeParentTypeSignature;
                }
            }
            else
            {
                // Else, not a nested type.
                // Handle types with element types.
                var hasElementType = Instances.TypeOperator.Has_ElementType(type);
                if (hasElementType)
                {
                    output.Has_ElementType = hasElementType;

                    output.ElementTypeRelationships = Instances.ElementTypeRelationshipOperator.Get_ElementTypeRelationship(type);

                    var elementType = Instances.TypeOperator.Get_ElementType(type);

                    output.ElementType = this.Get_TypeSignature(
                        elementType);

                    return output;
                }

                output.NamespaceName = Instances.TypeOperator.Get_NamespaceName(type);
            }

            output.TypeName = Instances.TypeOperator.Get_Name(type);

            var genericTypeInputs = Instances.TypeOperator.Get_GenericTypeInputs_NotInParents(type);

            var genericTypeInputSignatures = genericTypeInputs
                .Select(this.Get_TypeSignature)
                .Now()
                .Null_IfEmpty();

            output.GenericTypeInputs = genericTypeInputSignatures;

            if(includeAttributes)
            {
                output.Set_AttributeSignatures(type);
            }

            return output;
        }

        public PropertySignature Get_PropertySignature(PropertyInfo propertyInfo)
        {
            var declaringType = Instances.PropertyInfoOperator.Get_DeclaringType(propertyInfo);
            var propertyType = Instances.PropertyInfoOperator.Get_PropertyType(propertyInfo);
            var propertyName = Instances.PropertyInfoOperator.Get_PropertyName(propertyInfo);

            var declaringTypeSignature = this.Get_TypeSignature(declaringType);
            var propertyTypeSignature = this.Get_TypeSignature(propertyType);

            // If the property is an indexer, it will have method parameters.
            var parameters = Instances.PropertyInfoOperator.Get_IndexerParameters(propertyInfo);

            var declaringTypeGenericTypeInputSignaturesByName = declaringTypeSignature.GenericTypeInputs
                .Empty_IfNull()
                .ToDictionary(
                    x => x.TypeName);

            // Properties will never have method-level generic types.
            var methodGenericTypeParameterContext = Instances.DictionaryOperator.Empty<string, TypeSignature>();

            var methodParameters = this.Get_MethodParameters(
                parameters);

            var output = new PropertySignature
            {
                DeclaringType = declaringTypeSignature,
                Parameters = methodParameters,
                PropertyName = propertyName,
                PropertyType = propertyTypeSignature,
            }
            .Set_AttributeSignatures(propertyInfo);

            return output;
        }

        public bool Has_DeclaringType(
            MemberInfo memberInfo,
            out Type declaringType)
        {
            declaringType = this.Get_DeclaringType(memberInfo);

            var output = Instances.DefaultOperator.Is_NotDefault(declaringType);
            return output;
        }

        public bool Has_DeclaringType(MemberInfo memberInfo)
            => this.Has_DeclaringType(
                memberInfo,
                out _);

        /// <summary>
        /// Chooses <see cref="Is_Obsolete_Simple(MemberInfo)"/> as the default.
        /// <para><inheritdoc cref="Is_Obsolete_Simple(MemberInfo)" path="/summary"/></para>
        /// </summary>
        public bool Is_Obsolete(MemberInfo memberInfo)
            => this.Is_Obsolete_Simple(memberInfo);

        /// <summary>
        /// Simply evaluates the given member for whether it has the obsolete attribute.
        /// <para>Does <strong>not</strong> evaluate whether the member is within an obsolete type (for non-type members), or nested within an obsolete type (for type members).</para>
        /// </summary>
        public bool Is_Obsolete_Simple(MemberInfo memberInfo)
        {
            var hasObsoleteAttribute = this.Has_AttributeOfType(
                memberInfo,
                Instances.NamespacedTypeNames.System_ObsoleteAttribute,
                out _);

            return hasObsoleteAttribute;
        }

        /// <summary>
        /// Evaluates whether the given member is obsolete, or is declared in a type that is obsolete.
        /// <para>Does <strong>not</strong> evaluate whether the member is within an obsolete type (for non-type members), or nested within an obsolete type (for type members).</para>
        /// </summary>
        public bool Is_Obsolete_OrInObsoleteDeclaringType(MemberInfo memberInfo)
        {
            var isObsolete_Simple = this.Is_Obsolete_Simple(memberInfo);
            if (isObsolete_Simple)
            {
                return true;
            }

            // Else, if the 
            var hasDeclaringType = this.Has_DeclaringType(
                memberInfo,
                out var declaringType);

            if (hasDeclaringType)
            {
                var isObsolete_DeclaringType = this.Is_Obsolete_Simple(declaringType);
                if (isObsolete_DeclaringType)
                {
                    return true;
                }
            }

            // Else, not obsolete.
            return false;
        }
    }
}
