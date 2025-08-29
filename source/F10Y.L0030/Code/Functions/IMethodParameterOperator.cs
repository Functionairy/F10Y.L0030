using System;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IMethodParameterOperator
    {
        public bool Are_Equal_ByValue(MethodParameterSignature a, MethodParameterSignature b)
        {
            var output = true;

            output &= a.ParameterName == b.ParameterName;
            output &= Instances.TypeSignatureOperator.Are_Equal_ByValue(
                a.ParameterType,
                b.ParameterType);

            return output;
        }

        public string To_String(MethodParameterSignature methodParameter)
        {
            var parameterTypeName = Instances.TypeSignatureOperator.To_String(methodParameter.ParameterType);

            var output = $"{methodParameter.ParameterName}: {parameterTypeName}";
            return output;
        }
    }
}
