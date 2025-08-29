using System;
using System.Reflection;


namespace F10Y.L0030.Extensions
{
    public static class SignatureExtensions
    {
        public static TSignature Set_AttributeSignatures<TSignature>(this TSignature signature,
            MemberInfo memberInfo)
            where TSignature : Signature
        {
            Instances.MemberInfoOperator.Set_AttributeSignatures(
                signature,
                memberInfo);

            return signature;
        }
    }
}
