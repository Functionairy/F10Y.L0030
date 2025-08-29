using System;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IPropertySignatureOperator
    {
        public bool Are_Equal_ByValue(PropertySignature a, PropertySignature b)
        {
            var output = Instances.SignatureOperator.Are_Equal_ByValue_SignatureOnly(a, b,
                (a, b) =>
                {
                    var output = true;

                    output &= a.PropertyName == b.PropertyName;
                    output &= Instances.TypeSignatureOperator.Are_Equal_ByValue(
                        a.DeclaringType,
                        b.DeclaringType);
                    output &= Instances.TypeSignatureOperator.Are_Equal_ByValue(
                        a.PropertyType,
                        b.PropertyType);
                    output &= Instances.ArrayOperator.Are_Equal(
                        a.Parameters,
                        b.Parameters,
                        Instances.MethodParameterOperator.Are_Equal_ByValue);

                    return output;
                });

            return output;
        }

        public string ToString(PropertySignature propertySignature)
        {
            var propertyTypeName = Instances.TypeSignatureOperator.To_String(propertySignature.PropertyType);

            var output = $"{propertySignature.PropertyName}: {propertyTypeName}";
            return output;
        }
    }
}
