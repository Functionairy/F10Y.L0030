using System;
using System.Reflection;

using F10Y.T0002;
using F10Y.T0011;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IExceptionOperator :
        L0000.IExceptionOperator
    {
#pragma warning disable IDE1006 // Naming Styles

        [Ignore]
        public L0000.IExceptionOperator _L0000 => L0000.ExceptionOperator.Instance;

#pragma warning restore IDE1006 // Naming Styles


        public Exception Get_UnrecognizedSignatureType(Signature signature)
        {
            var signatureTypeName = Instances.TypeNameOperator.Get_TypeNameOf(signature);

            var output = new Exception($"{signatureTypeName}: unrecognized signature type.");
            return output;
        }

        public Exception Get_UnknownElementTypeRelationshipException()
        {
            var output = new Exception("Unknown element type relationship.");
            return output;
        }

        public Exception Get_UnrecognizedMemberTypeException(MemberInfo memberInfo)
        {
            var message = Instances.ExceptionMessageOperator.Get_UnrecognizedMemberTypeExceptionMessage(memberInfo);

            var output = this.Get_Exception(message);
            return output;
        }

    }
}
