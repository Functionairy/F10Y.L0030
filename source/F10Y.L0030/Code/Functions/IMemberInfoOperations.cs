using System;
using System.Reflection;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IMemberInfoOperations
    {
        public Func<TMemberInfo, bool> Name_Is<TMemberInfo>(string memberName)
            where TMemberInfo : MemberInfo
        {
            bool Internal(TMemberInfo member)
            {
                var output = Instances.MemberInfoOperator.Is_Name(
                    member,
                    memberName);

                return output;
            }

            return Internal;
        }
    }
}
