using System;

using F10Y.T0004;


namespace F10Y.L0030
{
    /// <summary>
    /// Signature structure representing methods.
    /// </summary>
    [DataTypeMarker]
    public class MethodSignature : Signature,
        IHas_DeclaringType,
        IHas_Parameters
    {
        public TypeSignature DeclaringType { get; set; }
        public string MethodName { get; set; }
        public TypeSignature ReturnType { get; set; }

        /// <summary>
        /// If the property is an indexer, it will have input parameters.
        /// </summary>
        public MethodParameterSignature[] Parameters { get; set; }

        /// <summary>
        /// Generic type inputs are either 1) parameters or 2) arguments.
        /// If the method is a generic method definition (open generic method without specified generic type arguments), then it will have generic type parameter names.
        /// If the method is a constructed generic method (closed generic method with specified generic type arguments), then it will have generic type arguments.
        /// </summary>
        public TypeSignature[] GenericTypeInputs { get; set; }


        public MethodSignature()
        {
            this.KindMarker = Instances.KindMarkers.Method;
        }

        public override string ToString()
        {
            var output = Instances.MethodSignatureOperator.ToString(this);
            return output;
        }
    }
}
