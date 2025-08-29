using System;
using System.Reflection;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IMethodInfoOperations
    {
        public Func<MethodInfo, bool> Name_Is(string methodName)
            => Instances.MemberInfoOperations.Name_Is<MethodInfo>(methodName);

        public Func<MethodInfo, bool> Name_Is(
            string methodName,
            int genericTypeInputCount)
        {
            bool Internal(MethodInfo method)
            {
                var output = Instances.MethodInfoOperator.Is_Name(
                    method,
                    methodName,
                    genericTypeInputCount);

                return output;
            }

            return Internal;
        }
    }
}
