using System;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface ITypedArgumentSignatureOperator
    {
        public bool Are_Equal_HandleNulls(
            TypedArgumentSignature a,
            TypedArgumentSignature b)
        {
            var output = Instances.NullOperator.NullCheckDeterminesEquality_Else(
                a,
                b,
                this.Are_Equal);

            return output;
        }

        public bool Are_Equal(
            TypedArgumentSignature a,
            TypedArgumentSignature b)
        {
            var output = true
                && a.Type.Equals(b.Type)
                && a.Value.Equals(b.Value)
                ;

            return output;
        }

        public int Get_HashCode(TypedArgumentSignature typedArgumentSignature)
            => Instances.HashCodeOperator.Combine(
                typedArgumentSignature.Type,
                typedArgumentSignature.Value);
    }
}
