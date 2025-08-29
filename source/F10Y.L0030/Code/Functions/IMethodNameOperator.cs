using System;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IMethodNameOperator
    {
        /// <summary>
        /// Determines if the method is an explicit, or implicit, conversion operator.
        /// </summary>
        public bool Is_ConversionOperator(string methodName)
        {
            var output = false
                || methodName == Instances.SpecialMethodNames.ImplicitConversionOperator
                || methodName == Instances.SpecialMethodNames.ExplicitConversionOperator;

            return output;
        }
    }
}
