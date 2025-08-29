using System;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IFieldSignatureOperator
    {
        public bool Are_Equal_ByValue(FieldSignature a, FieldSignature b)
        {
            var output = Instances.SignatureOperator.Are_Equal_ByValue_SignatureOnly(a, b,
                (a, b) =>
                {
                    var output = true;

                    output &= a.FieldName == b.FieldName;
                    output &= Instances.TypeSignatureOperator.Are_Equal_ByValue(
                        a.DeclaringType,
                        b.DeclaringType);
                    output &= Instances.TypeSignatureOperator.Are_Equal_ByValue(
                        a.FieldType,
                        b.FieldType);

                    return output;
                });

            return output;
        }

        public string ToString(FieldSignature fieldSignature)
        {
            var declaringTypeName = fieldSignature.DeclaringType.ToString();

            var typeNamedEventName = Instances.NamespacedTypeNameOperator.Combine(
                declaringTypeName,
                fieldSignature.FieldName);

            var eventHandlerTypeName = fieldSignature.FieldType.ToString();

            var output = Instances.TypeNameOperator.Append_OutputTypeName(
                typeNamedEventName,
                eventHandlerTypeName);

            return output;
        }
    }
}
