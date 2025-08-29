using System;
using System.Linq;

using F10Y.T0002;
using F10Y.T0011;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface ITypeOperator :
        L0000.ITypeOperator
    {
#pragma warning disable IDE1006 // Naming Styles

        [Ignore]
        public L0000.ITypeOperator _L0000 => L0000.TypeOperator.Instance;

#pragma warning restore IDE1006 // Naming Styles


        public Type Get_ElementType(Type type)
        {
            var output = type.GetElementType();
            return output;
        }

        /// <summary>
        /// Chooses <see cref="Get_GenericTypeInputs_NotInParents(Type)"/> as the default, since in general all we really want are the *new* generic inputs of the type.
        /// If you want all the raw generic inputs of the type, use <see cref="Get_GenericTypeInputs_OfType(Type)"/>.
        /// </summary>
        public Type[] Get_GenericTypeInputs(Type type)
        {
            var output = this.Get_GenericTypeInputs_NotInParents(type);
            return output;
        }

        /// <summary>
        /// For a generic type parameter, get the actual name of the type parameter (example: "T").
        /// </summary>
        public string Get_GenericTypeParameterTypeName_ActualName(Type type)
        {
            var output = type.Name;
            return output;
        }

        /// <summary>
        /// Get generic type inputs (either arguments, which are specified types like System.String, or parameters, which are unspecified like TKey).
        /// Note: handles any complications due to nesting, such as where the type might share generic inputs from it's nested parent type, by only returning generic types inputs that are not generic type inputs of any nested parents.
        /// </summary>
        public Type[] Get_GenericTypeInputs_NotInParents(Type type)
        {
            var genericTypeInputsOfType = this.Get_GenericTypeInputs_OfType(type);

            var isNested = this.Is_NestedType(type);
            if (isNested)
            {
                var nestedParentType = this.Get_NestedTypeParentType(type);

                // Get the generic type inputs of the parent, including any that are of the parent's parent.
                var nestedParentGenericTypeInputs = this.Get_GenericTypeInputs_OfType(nestedParentType);

                // New generic types inputs in this type that are not in the parent, which are assumed to be just those types after the generic types of the parent.
                var output = genericTypeInputsOfType.Skip(nestedParentGenericTypeInputs.Length)
                    .ToArray();

                return output;
            }
            else
            {
                // If not nested, just return all generic type inputs of the type.
                return genericTypeInputsOfType;
            }
        }

        /// <summary>
        /// Get generic type inputs (either arguments, which are specified types like System.String, or parameters, which are unspecified like TKey).
        /// Note: gets the generic type inputs of the type (without handling any complications due to nesting, where the type might share generic inputs from it's nested parent type).
        /// </summary>
        public Type[] Get_GenericTypeInputs_OfType(Type type)
        {
            // The GetGenericArguments() method returns both type parameters of a generic type definition,
            // and the generic type arguments of a closed generic type.
            var output = type.GetGenericArguments();
            return output;
        }

        /// <summary>
        /// Gets the simple name of a type (removing the generic parameter count).
        /// </summary>
        public string Get_Name_Simple(Type type)
        {
            var namePossiblyWithTypeParameterCount = this.Get_Name(type);

            var hasParameterCount = Instances.StringOperator.Contains(
                namePossiblyWithTypeParameterCount,
                Instances.TokenSeparators.TypeParameterCountSeparator);

            if (hasParameterCount)
            {
                var indexOfTypeParameterCountTokenSeparator = Instances.StringOperator.Get_IndexOf(
                    namePossiblyWithTypeParameterCount,
                    Instances.TokenSeparators.TypeParameterCountSeparator);

                var (output, _) = Instances.StringOperator.Partition_Exclusive(
                    indexOfTypeParameterCountTokenSeparator,
                    namePossiblyWithTypeParameterCount);

                return output;
            }
            else
            {
                return namePossiblyWithTypeParameterCount;
            }
        }

        /// <summary>
        /// The parent of a nested type is the type's <see cref="Type.DeclaringType"/>.
        /// </summary>
        public Type Get_NestedTypeParentType(Type type)
        {
            var output = type.DeclaringType;
            return output;
        }

        /// <summary>
        /// <para>Returns the value of <see cref="Type.HasElementType"/>.</para>
        /// Note: not just arrays, but by-reference and pointer types also have element types.
        /// </summary>
        public bool Has_ElementType(Type type)
        {
            var output = type.HasElementType;
            return output;
        }

        public bool Is_GenericTypeDefinition(Type type)
        {
            var output = type.IsGenericTypeDefinition;
            return output;
        }

        public bool Is_GenericMethodParamter(Type type)
        {
            var output = type.IsGenericMethodParameter;
            return output;
        }

        /// <summary>
        /// Is the type a type parameter of a generic type or generic method?
        /// Note: this tests for whether the type is a <em>parameter</em>, not an argument.
        /// As the clearest explanation of the difference,
        /// generic type parameters have names like "T", while generic type arguments have names like "System.Int32".
        /// </summary>
        public bool Is_GenericTypeParameter(Type type)
        {
            var output = type.IsGenericTypeParameter;
            return output;
        }
    }
}
