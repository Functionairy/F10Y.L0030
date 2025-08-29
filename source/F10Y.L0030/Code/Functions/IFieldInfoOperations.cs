using System;
using System.Reflection;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IFieldInfoOperations
    {
        public Func<FieldInfo, bool> Name_Is(string fieldName)
            => Instances.MemberInfoOperations.Name_Is<FieldInfo>(fieldName);
    }
}
