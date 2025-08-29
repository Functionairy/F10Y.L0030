using System;

using F10Y.T0009;


namespace F10Y.L0030
{
    [HasXMarker]
    public interface IHas_DeclaringType
    {
        TypeSignature DeclaringType { get; }
    }
}
