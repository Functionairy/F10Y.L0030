using System;
using System.Reflection;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IPropertyInfoOperations
    {
        public Func<PropertyInfo, bool> Name_Is(string propertyName)
            => Instances.MemberInfoOperations.Name_Is<PropertyInfo>(propertyName);
    }
}
