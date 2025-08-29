using System;

using F10Y.T0004;


namespace F10Y.L0030
{
    /// <summary>
    /// Models a attribute <see cref="System.Reflection.CustomAttributeData"/>.
    /// </summary>
    [DataTypeMarker]
    public class AttributeSignature : IEquatable<AttributeSignature>
    {
        public TypeSignature Type { get; set; }

        /// <summary>
        /// Arguments given to the constructor method as a value.
        /// </summary>
        /// <remarks>
        /// Should not be null.
        /// </remarks>
        public TypedArgumentSignature[] ConstructorArguments { get; set; }

        /// <summary>
        /// Arguments specified by name (optional).
        /// </summary>
        public NamedArgumentSignature[] NamedArguments { get; set; }

        public bool Equals(AttributeSignature other)
            => Instances.AttributeSignatureOperator.Are_Equal_HanldeNulls(
                this,
                other);

        public override bool Equals(object obj)
            => this.Equals(obj as AttributeSignature);

        public override int GetHashCode()
            => Instances.AttributeSignatureOperator.Get_HashCode(this);
    }
}
