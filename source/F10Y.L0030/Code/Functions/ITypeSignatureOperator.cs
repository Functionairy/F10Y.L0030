using System;
using System.Collections.Generic;
using System.Linq;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface ITypeSignatureOperator
    {
        public void Add_NestedTypeGenericTypeInputs(
            TypeSignature typeSignature,
            List<TypeSignature> genericTypeInputs)
        {
            if (typeSignature.Is_Nested)
            {
                this.Add_NestedTypeGenericTypeInputs(
                    typeSignature.NestedTypeParent,
                    genericTypeInputs);
            }

            var hasTypeGenericTypeInputs = Instances.NullOperator.Is_NotNull(typeSignature.GenericTypeInputs);
            if (hasTypeGenericTypeInputs)
            {
                // All generic type parameter names are unique within the context of a signature.
                var newGenericTypeInputs = typeSignature.GenericTypeInputs
                    .Except(
                        genericTypeInputs,
                        TypeNameBasedTypeSignatureEqualityComparer.Instance);

                genericTypeInputs.AddRange(newGenericTypeInputs);
            }
        }

        /// <summary>
        /// Uses the rules of C# type identity to compare two types.
        /// Those rules consider only:
        /// 1. Namespace (including parent nested types, if present)
        /// 2. Type name (the simple type name)
        /// 3. The number of generic type parameters (if the type is generic)
        /// 4. Its element type (if the type is an array, or by-reference type).
        /// </summary>
        public bool Are_Equal_ByIdentity(TypeSignature a, TypeSignature b)
        {
            var output = Instances.NullOperator.NullCheckDeterminesEquality_Else(a, b,
                (a, b) =>
                {
                    var output = true;

                    output &= a.TypeName == b.TypeName;
                    output &= a.NamespaceName == b.NamespaceName;
                    output &= a.Is_Nested == b.Is_Nested;
                    output &= this.Are_Equal_ByValue(
                        a.NestedTypeParent,
                        b.NestedTypeParent);
                    output &= a.Is_GenericMethodParameter == b.Is_GenericMethodParameter;
                    output &= a.Is_GenericTypeParameter == b.Is_GenericTypeParameter;

                    var genericTypeInputsCountA = this.Get_GenericTypeInputsCount(a);
                    var genericTypeInputsCountB = this.Get_GenericTypeInputsCount(b);

                    output &= genericTypeInputsCountA == genericTypeInputsCountB;

                    output &= a.Has_ElementType == b.Has_ElementType;
                    output &= a.ElementTypeRelationships == b.ElementTypeRelationships;
                    output &= this.Are_Equal_ByValue(
                        a.ElementType,
                        b.ElementType);

                    return output;
                });

            return output;
        }

        public bool Are_Equal_ByValue(TypeSignature a, TypeSignature b)
        {
            var output = Instances.SignatureOperator.Are_Equal_ByValue_SignatureOnly(
                a,
                b,
                (a, b) =>
                {
                    var output = true;

                    output &= a.TypeName == b.TypeName;
                    output &= a.NamespaceName == b.NamespaceName;
                    output &= a.Is_Nested == b.Is_Nested;
                    output &= this.Are_Equal_ByValue(
                        a.NestedTypeParent,
                        b.NestedTypeParent);
                    output &= a.Is_GenericMethodParameter == b.Is_GenericMethodParameter;
                    output &= a.Is_GenericTypeParameter == b.Is_GenericTypeParameter;
                    output &= Instances.ArrayOperator.Are_Equal(
                        a.GenericTypeInputs,
                        b.GenericTypeInputs,
                        this.Are_Equal_ByValue);
                    output &= a.Has_ElementType == b.Has_ElementType;
                    output &= a.ElementTypeRelationships == b.ElementTypeRelationships;
                    output &= this.Are_Equal_ByValue(
                        a.ElementType,
                        b.ElementType);

                    return output;
                });

            return output;
        }

        public int Get_GenericTypeInputsCount(TypeSignature typeSignature)
        {
            var isNullOrEmpty = Instances.ArrayOperator.Is_NullOrEmpty(typeSignature.GenericTypeInputs);

            var output = isNullOrEmpty
                ? 0
                : typeSignature.GenericTypeInputs.Length;
            ;

            return output;
        }

        /// <inheritdoc cref="Are_Equal_ByIdentity(TypeSignature, TypeSignature)"/>
        public int Get_HashCode_ForIdentity(TypeSignature obj)
        {
            // For generic type parameters, only the type name matter.
            var isGenericTypeParameter = obj.Is_GenericMethodParameter || obj.Is_GenericTypeParameter;
            if (isGenericTypeParameter)
            {
                var output = obj.TypeName.GetHashCode();
                return output;
            }
            else
            {
                var forNestedParent = obj.Is_Nested
                    ? this.Get_HashCode_ForIdentity(obj.NestedTypeParent)
                    : Instances.HashCodes.For_Null_Fixed
                    ;

                var forElementType = obj.Has_ElementType
                    ? HashCode.Combine(
                        this.Get_HashCode_ForIdentity(obj.ElementType),
                        // Don't forget the element type relationship itself!
                        obj.ElementTypeRelationships)
                    : Instances.HashCodes.For_Null_Fixed
                    ;

                var genericTypeInputCount = this.Get_GenericTypeInputsCount(obj);

                var output = HashCode.Combine(
                    obj.NamespaceName,
                    obj.TypeName,
                    genericTypeInputCount,
                    forNestedParent,
                    forElementType);

                return output;
            }
        }

        public TypeSignature[] Get_NestedTypeGenericTypeInputs(TypeSignature typeSignature)
        {
            var genericTypeInputs = Instances.ListOperator.New<TypeSignature>();

            this.Add_NestedTypeGenericTypeInputs(
                typeSignature,
                genericTypeInputs);

            var output = genericTypeInputs.ToArray();
            return output;
        }

        public TypeSignature[] Get_NestedTypeGenericTypeParameters(TypeSignature typeSignature)
        {
            var nestedTypeGenericTypeInputs = this.Get_NestedTypeGenericTypeInputs(typeSignature);

            var output = nestedTypeGenericTypeInputs
                .Where(x => x.Is_GenericTypeParameter)
                .Now();

            return output;
        }

        public string To_String(TypeSignature typeSignature)
        {
            string output;

            if (typeSignature.Is_Nested)
            {
                var nestedParentTypeName = typeSignature.NestedTypeParent.ToString();

                output = Instances.TypeNameOperator.Append_NestedTypeName(
                    nestedParentTypeName,
                    typeSignature.TypeName);

                return output;
            }

            if (typeSignature.Has_ElementType)
            {
                var elementTypeName = typeSignature.ElementType.ToString();

                output = Instances.ElementTypeRelationshipOperator.Append_ElementTypeRelationshipMarkers(
                    elementTypeName,
                    typeSignature.ElementTypeRelationships,
                    Instances.TypeNameAffixes.Array_Suffix,
                    Instances.TypeNameAffixes.ByReference_Suffix_String,
                    Instances.TypeNameAffixes.Pointer_Suffix_String);

                return output;
            }

            if (typeSignature.Is_GenericTypeParameter || typeSignature.Is_GenericMethodParameter)
            {
                output = typeSignature.TypeName;

                return output;
            }

            output = Instances.NamespacedTypeNameOperator.Get_NamespacedTypeName(
                typeSignature.NamespaceName,
                typeSignature.TypeName);

            return output;
        }
    }
}
