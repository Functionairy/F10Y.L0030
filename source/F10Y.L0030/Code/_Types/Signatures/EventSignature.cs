using System;

using F10Y.T0004;


namespace F10Y.L0030
{
    /// <summary>
    /// A signature structure for representing event members.
    /// </summary>
    [DataTypeMarker]
    public class EventSignature : Signature, IEquatable<EventSignature>
    {
        public TypeSignature DeclaringType { get; set; }
        public string EventName { get; set; }
        public TypeSignature EventHandlerType { get; set; }


        public EventSignature()
        {
            this.KindMarker = Instances.KindMarkers.Event;
        }

        public override string ToString()
        {
            var output = Instances.EventSignatureOperator.ToString(this);
            return output;
        }

        public bool Equals(EventSignature other)
        {
            var output = Instances.EventSignatureOperator.Are_Equal_ByValue(
                this,
                other);

            return output;
        }
    }
}
