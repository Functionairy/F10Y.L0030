using System;
using System.Reflection;

using F10Y.T0002;
using F10Y.T0011;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IExceptionMessageOperator :
        L0000.IExceptionMessageOperator
    {
#pragma warning disable IDE1006 // Naming Styles

        [Ignore]
        public L0000.IExceptionMessageOperator _L0000 => L0000.ExceptionMessageOperator.Instance;

#pragma warning restore IDE1006 // Naming Styles


        public string Get_UnrecognizedMemberTypeExceptionMessage(MemberInfo memberInfo)
        {
            var output = $"Unrecognzed member info type: {memberInfo}";
            return output;
        }
    }
}
