using System;

using F10Y.T0004;


namespace F10Y.L0030
{
    /// <summary>
    /// Represents the parameter to a method (or property).
    /// </summary>
    [DataTypeMarker]
    public class MethodParameterSignature
    {
        public TypeSignature ParameterType { get; set; }
        public string ParameterName { get; set; }


        public override string ToString()
        {
            var output = Instances.MethodParameterOperator.To_String(this);
            return output;
        }
    }
}
