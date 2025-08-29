using System;
using System.Linq;
using System.Reflection;

using F10Y.L0001.L000;
using F10Y.T0002;
using F10Y.T0011;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IMethodBaseOperator :
        L0000.IMethodBaseOperator,
        L0001.L000.IMethodBaseOperator
    {
#pragma warning disable IDE1006 // Naming Styles

        [Ignore]
        public new L0000.IMethodBaseOperator _L0000 => L0000.MethodBaseOperator.Instance;

        [Ignore]
        public L0001.L000.IMethodBaseOperator _L0001_L000 => L0001.L000.MethodBaseOperator.Instance;

#pragma warning restore IDE1006 // Naming Styles
    }
}
