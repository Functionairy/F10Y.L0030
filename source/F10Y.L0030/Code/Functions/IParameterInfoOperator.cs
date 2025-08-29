using System;
using System.Reflection;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IParameterInfoOperator
    {
        /// <summary>
        /// Chooses <see cref="Get_ParameterName_OrDefault(ParameterInfo)"/> as the default.
        /// All parameters should have names, so by default this should be so.
        /// </summary>
        public string Get_ParameterName(ParameterInfo parameterInfo)
        {
            var output = this.Get_ParameterName_OrDefault(parameterInfo);
            return output;
        }

        /// <summary>
        /// <inheritdoc cref="Documentation.ParametersShouldHaveParameterNames" path="/summary"/>
        /// In that case, use the default parameter name (<see cref="IValues.Default_ParameterName"/>).
        /// </summary>
        public string Get_ParameterName_OrDefault(ParameterInfo parameterInfo)
        {
            var output = this.Get_ParameterName_OrDefault(
                parameterInfo,
                Instances.Values.Default_ParameterName);

            return output;
        }

        /// <summary>
        /// <inheritdoc cref="Documentation.ParametersShouldHaveParameterNames" path="/summary"/>
        /// In that case, use the specified default parameter name.
        /// </summary>
        public string Get_ParameterName_OrDefault(
            ParameterInfo parameterInfo,
            string defaultParameterName)
        {
            var parameterNameOrNullOrEmpty = this.Get_ParameterName_Robust(parameterInfo);

            var isNullOrEmpty = Instances.StringOperator.Is_NullOrEmpty(parameterNameOrNullOrEmpty);

            var output = isNullOrEmpty
                ? defaultParameterName
                : parameterNameOrNullOrEmpty
                ;

            return output;
        }

        /// <summary>
        /// Robust in the sense that the result of <see cref="ParameterInfo.Name"/> is returned without any check that it is non-null and non-empty.
        /// </summary>
        public string Get_ParameterName_Robust(ParameterInfo parameterInfo)
        {
            var output = parameterInfo.Name;
            return output;
        }

        /// <summary>
        /// Strict in the sense that an exception is thrown if the parameter name is null or empty.
        /// </summary>
        public string Get_ParameterName_Strict(ParameterInfo parameterInfo)
        {
            var parameterNameOrNullOrEmpty = this.Get_ParameterName_Robust(parameterInfo);

            var isNullOrEmpty = Instances.StringOperator.Is_NullOrEmpty(parameterNameOrNullOrEmpty);
            if (isNullOrEmpty)
            {
                throw new Exception("Parameter name was null or empty.");
            }

            // Else, success.
            var output = parameterNameOrNullOrEmpty;
            return output;
        }

        /// <summary>
        /// Returns <see cref="ParameterInfo.ParameterType"/>.
        /// </summary>
        public Type Get_ParameterType(ParameterInfo parameterInfo)
        {
            var output = parameterInfo.ParameterType;
            return output;
        }

        /// <summary>
        /// Determines if the parameter is a C# "ref" parameter.
        /// This is determined by whether the type of the parameters is a by-reference type.
        /// </summary>
        public bool Is_ByReference(ParameterInfo parameterInfo)
        {
            var output = parameterInfo.ParameterType.IsByRef;
            return output;
        }
    }
}
