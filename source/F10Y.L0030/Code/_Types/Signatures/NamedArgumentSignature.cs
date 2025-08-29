using System;

using F10Y.T0004;


namespace F10Y.L0030
{
    /// <summary>
    /// Models an argument, as a typed argument with a name.
    /// </summary>
    /// <remarks>
    /// Useful for the <see cref="AttributeSignature"/> type.
    /// </remarks>
    [DataTypeMarker]
    public class NamedArgumentSignature : IEquatable<NamedArgumentSignature>
    {
        public string Name { get; set; }

        public TypedArgumentSignature TypedValue { get; set; }

        public bool Equals(NamedArgumentSignature other)
            => Instances.NamedArgumentSignatureOperator.Are_Equal(
                this,
                other);

        public override bool Equals(object obj)
            => this.Equals(obj as NamedArgumentSignature);

        public override int GetHashCode()
            => Instances.NamedArgumentSignatureOperator.Get_HashCode(this);
    }
}
