using System;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IMethodSignatureOperator
    {
        public bool Are_Equal_ByValue(MethodSignature a, MethodSignature b)
        {
            var output = Instances.SignatureOperator.Are_Equal_ByValue_SignatureOnly(a, b,
                (a, b) =>
                {
                    var output = true;

                    output &= a.MethodName == b.MethodName;
                    output &= Instances.TypeSignatureOperator.Are_Equal_ByValue(
                        a.DeclaringType,
                        b.DeclaringType);
                    output &= Instances.TypeSignatureOperator.Are_Equal_ByValue(
                        a.ReturnType,
                        b.ReturnType);
                    output &= Instances.ArrayOperator.Are_Equal(
                        a.Parameters,
                        b.Parameters,
                        Instances.MethodParameterOperator.Are_Equal_ByValue);
                    output &= Instances.ArrayOperator.Are_Equal(
                        a.GenericTypeInputs,
                        b.GenericTypeInputs,
                        Instances.TypeSignatureOperator.Are_Equal_ByValue);

                    return output;
                });

            return output;
        }

        public string ToString(MethodSignature methodSignature)
        {
            var returnTypeName = Instances.TypeSignatureOperator.To_String(methodSignature.ReturnType);

            var output = $"{methodSignature.MethodName}: {returnTypeName}";
            return output;
        }
    }
}
