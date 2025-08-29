using System;

using F10Y.T0004;


namespace F10Y.L0030
{
    /// <summary>
    /// A signature structure base type for representing the various member types in C# (events, fields, methods, properties, types).
    /// </summary>
    /// <remarks>
    /// This base class is somewhat unsettled.
    /// Should it include the obsoletion property?
    /// </remarks>
    [DataTypeMarker]
    public abstract class Signature
    {
        public char KindMarker { get; set; }

        public AttributeSignature[] Attributes { get; set; }


        protected Signature(char kindMarker)
        {
            this.KindMarker = kindMarker;
        }

        protected Signature()
        {
        }
    }
}
