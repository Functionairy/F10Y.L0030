using System;

using F10Y.T0002;


namespace F10Y.L0030
{
    /// <summary>
    /// 
    /// </summary>
    // Prior work: R5T.T0221.
    [FunctionsMarker]
    public partial interface IElementTypeRelationshipOperator
    {
        /// <inheritdoc cref="Append_ElementTypeRelationshipMarkers(string, ElementTypeRelationships, string, string, string)"/>
        public string Append_ElementTypeRelationshipMarkers(
            string typeName,
            ElementTypeRelationships elementTypeRelationships)
            => Instances.ElementTypeRelationshipOperator.Append_ElementTypeRelationshipMarkers(
                typeName,
                elementTypeRelationships,
                Instances.Tokens.ArrayTypeToken,
                Instances.Tokens.ReferenceTypeToken,
                Instances.Tokens.PointerTypeToken);

        /// <summary>
        /// Appends element type marker tokens to the type name.
        /// <para>Note: multiple type markers may be appended if the type has multiple relationships to its element type.</para>
        /// </summary>
        public string Append_ElementTypeRelationshipMarkers(
            string typeName,
            ElementTypeRelationships elementTypeRelationships,
            string arrayMarker,
            string referenceMarker,
            string pointerMarker)
        {
            var isArray = Instances.FlagsOperator.Has_Flag(elementTypeRelationships, ElementTypeRelationships.Array);
            if (isArray)
            {
                typeName += arrayMarker;
            }

            var isReference = Instances.FlagsOperator.Has_Flag(elementTypeRelationships, ElementTypeRelationships.Reference);
            if (isReference)
            {
                typeName += referenceMarker;
            }

            var isPointer = Instances.FlagsOperator.Has_Flag(elementTypeRelationships, ElementTypeRelationships.Pointer);
            if (isPointer)
            {
                typeName += pointerMarker;
            }

            return typeName;
        }

        public ElementTypeRelationships Get_ElementTypeRelationship(Type type)
        {
            var output = ElementTypeRelationships.None;

            if (type.IsArray)
            {
                output |= ElementTypeRelationships.Array;
            }

            if (type.IsByRef)
            {
                output |= ElementTypeRelationships.Reference;
            }

            if (type.IsPointer)
            {
                output |= ElementTypeRelationships.Pointer;
            }

            return output;
        }
    }
}
