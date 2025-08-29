using System;

using F10Y.T0003;


namespace F10Y.L0030
{
    [ValuesMarker]
    public partial interface ITypeNameAffixSets
    {
        public string[] All =>
        [
            Instances.TypeNameAffixes.Array_Suffix,
            Instances.TypeNameAffixes.ByReference_Suffix_String,
            Instances.TypeNameAffixes.Pointer_Suffix_String
        ];
    }
}
