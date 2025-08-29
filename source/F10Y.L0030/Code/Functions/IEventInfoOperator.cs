using System;
using System.Linq;
using System.Reflection;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IEventInfoOperator
    {
        /// <summary>
        /// Returns the result of <see cref="MemberInfo.DeclaringType"/>.
        /// </summary>
        public Type Get_DeclaringType(EventInfo eventInfo)
            => Instances.MemberInfoOperator.Get_DeclaringType(eventInfo);

        /// <summary>
        /// Returns the result of <see cref="EventInfo.EventHandlerType"/>.
        /// </summary>
        public Type Get_EventHandlerType(EventInfo eventInfo)
        {
            var output = eventInfo.EventHandlerType;
            return output;
        }

        /// <summary>
        /// Quality-of-life overload for <see cref="Get_Name(EventInfo)"/>.
        /// </summary>
        public string Get_EventName(EventInfo eventInfo)
            => this.Get_Name(eventInfo);

        public EventInfo Get_EventOf(
            Type type,
            string eventName)
        {
            var method = type.GetEvents()
                .Where(Instances.EventInfoOperations.Name_Is(eventName))
                .Single();

            return method;
        }

        public EventInfo Get_EventOf<T>(string eventName)
        {
            var type = Instances.TypeOperator.Get_TypeOf<T>();

            var output = this.Get_EventOf(
                type,
                eventName);

            return output;
        }

        public string Get_Name(EventInfo eventInfo)
            => Instances.MemberInfoOperator.Get_Name(eventInfo);

        public bool Is_Name(
            EventInfo eventInfo,
            string eventName)
            => Instances.MemberInfoOperator.Is_Name(
                eventInfo,
                eventName);
    }
}
