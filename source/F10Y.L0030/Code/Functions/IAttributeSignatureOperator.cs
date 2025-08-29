using System;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IAttributeSignatureOperator
    {
        public bool Are_Equal(
            AttributeSignature a,
            AttributeSignature b)
        {
            var output = true
                && a.Type.Equals(b.Type)
                && Instances.ArrayOperator.Are_Equal_OrderDependent(
                    a.ConstructorArguments,
                    b.ConstructorArguments,
                    Instances.TypedArgumentSignatureOperator.Are_Equal_HandleNulls)
                && Instances.ArrayOperator.Are_Equal_OrderDependent(
                    a.NamedArguments,
                    b.NamedArguments,
                    Instances.NamedArgumentSignatureOperator.Are_Equal_HandleNulls)
                ;

            return output;
        }

        public bool Are_Equal_HanldeNulls(
            AttributeSignature a,
            AttributeSignature b)
        {
            var output = Instances.NullOperator.NullCheckDeterminesEquality_Else(
                a,
                b,
                this.Are_Equal);

            return output;
        }

        public int Get_HashCode(AttributeSignature attributeSignature)
            => Instances.HashCodeOperator.Combine(
                attributeSignature.Type,
                attributeSignature.ConstructorArguments,
                attributeSignature.NamedArguments);
    }
}
