using System;

using F10Y.T0003;


namespace F10Y.L0030
{
    [ValuesMarker]
    public partial interface ISpecialMethodNames
    {
        public string ImplicitConversionOperator => "op_Implicit";
        public string ExplicitConversionOperator => "op_Explicit";
    }
}
