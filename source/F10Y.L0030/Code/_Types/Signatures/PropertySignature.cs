using System;

using F10Y.T0004;


namespace F10Y.L0030
{
    /// <summary>
    /// Signature structure for representing properties.
    /// </summary>
    [DataTypeMarker]
    public class PropertySignature : Signature,
        IHas_DeclaringType,
        IHas_Parameters
    {
        public TypeSignature DeclaringType { get; set; }
        public string PropertyName { get; set; }
        public TypeSignature PropertyType { get; set; }

        /// <summary>
        /// If the property is an indexer, it will have input parameters.
        /// </summary>
        public MethodParameterSignature[] Parameters { get; set; }


        public PropertySignature()
        {
            this.KindMarker = Instances.KindMarkers.Property;
        }

        public override string ToString()
        {
            var output = Instances.PropertySignatureOperator.ToString(this);
            return output;
        }
    }
}
