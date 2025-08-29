using System;

using F10Y.T0004;


namespace F10Y.L0030
{
    /// <summary>
    /// Models an argument, with type and object-based value.
    /// </summary>
    /// <remarks>
    /// Useful for the <see cref="AttributeSignature"/> type.
    /// </remarks>
    [DataTypeMarker]
    public class TypedArgumentSignature : IEquatable<TypedArgumentSignature>
    {
        public TypeSignature Type { get; set; }
        public object Value { get; set; }

        public bool Equals(TypedArgumentSignature other)
            => Instances.TypedArgumentSignatureOperator.Are_Equal_HandleNulls(
                this,
                other);

        public override bool Equals(object obj)
            => this.Equals(obj as TypedArgumentSignature);

        public override int GetHashCode()
            => Instances.TypedArgumentSignatureOperator.Get_HashCode(this);
    }
}
