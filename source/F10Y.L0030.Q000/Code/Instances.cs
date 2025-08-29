using System;


namespace F10Y.L0030.Q000
{
    public class Instances
    {
        public static L0000.IEnumerableOperator EnumerableOperator => L0000.EnumerableOperator.Instance;
        public static IIdentityStringOperator IdentityStringOperator => L0030.IdentityStringOperator.Instance;
        public static IMemberInfos MemberInfos => Q000.MemberInfos.Instance;
        public static ISignatureOperator SignatureOperator => L0030.SignatureOperator.Instance;
    }
}