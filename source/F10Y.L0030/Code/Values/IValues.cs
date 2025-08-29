using System;

using F10Y.T0003;
using F10Y.T0011;


namespace F10Y.L0030
{
    [ValuesMarker]
    public partial interface IValues :
        L0000.IValues
    {
#pragma warning disable IDE1006 // Naming Styles

        [Ignore]
        public L0000.IValues _L0000 => L0000.Values.Instance;

#pragma warning restore IDE1006 // Naming Styles


        /// <summary>
        /// <para>
        /// <value>"parameter"</value>,
        /// <description>the default value for parameters whose names are strangely null or empty</description>.
        /// </para>
        /// </summary>
        public const string Default_ParameterName_Const = "parameter";

        /// <inheritdoc cref="Default_ParameterName_Const"/>
        public string Default_ParameterName => Default_ParameterName_Const;
    }
}
