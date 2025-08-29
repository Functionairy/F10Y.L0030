using System;
using System.Reflection;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IEventInfoOperations
    {
        public Func<EventInfo, bool> Name_Is(string methodName)
            => Instances.MemberInfoOperations.Name_Is<EventInfo>(methodName);
    }
}
