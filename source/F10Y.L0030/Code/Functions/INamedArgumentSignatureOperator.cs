using System;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface INamedArgumentSignatureOperator
    {
        public bool Are_Equal_HandleNulls(
            NamedArgumentSignature a,
            NamedArgumentSignature b)
        {
            var output = Instances.NullOperator.NullCheckDeterminesEquality_Else(
                a,
                b,
                this.Are_Equal);

            return output;
        }

        public bool Are_Equal(
            NamedArgumentSignature a,
            NamedArgumentSignature b)
        {
            var output = true
                && a.Name == b.Name
                && a.TypedValue.Equals(b.TypedValue)
                ;

            return output;
        }

        public int Get_HashCode(NamedArgumentSignature namedArgumentSignature)
            => Instances.HashCodeOperator.Combine(
                namedArgumentSignature.Name,
                namedArgumentSignature.TypedValue);
    }
}
