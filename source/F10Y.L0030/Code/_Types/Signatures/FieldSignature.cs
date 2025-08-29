using System;

using F10Y.T0004;


namespace F10Y.L0030
{
    /// <summary>
    /// A signature structure for representing field members.
    /// </summary>
    [DataTypeMarker]
    public class FieldSignature : Signature, IEquatable<FieldSignature>
    {
        public TypeSignature DeclaringType { get; set; }
        public string FieldName { get; set; }
        public TypeSignature FieldType { get; set; }


        public FieldSignature()
        {
            this.KindMarker = Instances.KindMarkers.Field;
        }

        public override string ToString()
        {
            var output = Instances.FieldSignatureOperator.ToString(this);
            return output;
        }

        public bool Equals(FieldSignature other)
        {
            var output = Instances.FieldSignatureOperator.Are_Equal_ByValue(
                this,
                other);

            return output;
        }
    }
}
