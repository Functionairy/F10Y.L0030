using System;
using System.Linq;
using System.Reflection;

using F10Y.T0002;
using F10Y.T0011;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IMethodInfoOperator :
        L0000.IMethodInfoOperator
    {
#pragma warning disable IDE1006 // Naming Styles

        [Ignore]
        public L0000.IMethodInfoOperator _L0000 => L0000.MethodInfoOperator.Instance;

#pragma warning restore IDE1006 // Naming Styles


        /// <inheritdoc cref="L0001.L000.IMethodBaseOperator.Get_GenericTypeParameters(MethodBase)"/>
        public Type[] Get_GenericTypeParameters(MethodInfo methodInfo)
        {
            var output = Instances.MethodBaseOperator.Get_GenericTypeParameters(methodInfo);
            return output;
        }

        /// <inheritdoc cref="L0001.L000.IMethodBaseOperator.Get_InputParameters(MethodBase)"/>
        public ParameterInfo[] Get_InputParameters(MethodInfo methodInfo)
        {
            var output = Instances.MethodBaseOperator.Get_InputParameters(methodInfo);
            return output;
        }

        public Type Get_ReturnType(MethodInfo methodInfo)
        {
            // After upgrade to MetadataLoadContext 8.0, parsing of function pointer types is supported.
            var output = methodInfo.ReturnType;
            return output;
        }

        public int Get_TypeParameterCount(MethodInfo methodInfo)
        {
            var output = methodInfo.GetGenericArguments().Length;
            return output;
        }

        /// <summary>
        /// Determines if the method has any input parameters.
        /// </summary>
        public bool Has_InputParameters(MethodInfo methodInfo)
        {
            var inputParameters = this.Get_InputParameters(methodInfo);

            var output = inputParameters.Any();
            return output;
        }

        /// <summary>
        /// Determines if the method is an explicit, or implicit, conversion operator.
        /// </summary>
        public bool Is_ConversionOperator(MethodInfo methodInfo)
        {
            var output = Instances.MethodNameOperator.Is_ConversionOperator(methodInfo.Name);
            return output;
        }

        /// <inheritdoc cref="L0001.L000.IMethodBaseOperator.Is_Generic(MethodBase)"/>
        public bool Is_Generic(MethodInfo methodInfo)
        {
            var output = Instances.MethodBaseOperator.Is_Generic(methodInfo);
            return output;
        }

        public MethodInfo Get_MethodOf(
            Type type,
            string methodName)
        {
            var method = type.GetMethods()
                .Where(Instances.MethodInfoOperations.Name_Is(methodName))
                .Single();

            return method;
        }

        public MethodInfo Get_MethodOf<T>(string methodName)
        {
            var type = Instances.TypeOperator.Get_TypeOf<T>();

            var output = this.Get_MethodOf(
                type,
                methodName);

            return output;
        }

        public MethodInfo Get_MethodOf(
            Type type,
            string methodName,
            int genericTypeInputCount)
        {
            var method = type.GetMethods()
                .Where(Instances.MethodInfoOperations.Name_Is(
                    methodName,
                    genericTypeInputCount))
                .Single();

            return method;
        }

        public MethodInfo Get_MethodOf<T>(
            string methodName,
            int genericTypeInputCount)
        {
            var type = Instances.TypeOperator.Get_TypeOf<T>();

            var output = this.Get_MethodOf(
                type,
                methodName,
                genericTypeInputCount);

            return output;
        }

        public int Get_GenericTypeInputCount(MethodInfo methodInfo)
        {
            var output = methodInfo.GetGenericArguments().Length;
            return output;
        }

        /// <summary>
        /// Gets the standard name for the method.
        /// </summary>
        public string Get_Name(MethodInfo method)
        {
            var output = method.Name;
            return output;
        }

        public bool Is_GenericTypeInputCount(
            MethodInfo methodInfo,
            int genericTypeInputCount)
        {
            var count = this.Get_GenericTypeInputCount(methodInfo);

            var output = count == genericTypeInputCount;
            return output;
        }

        public bool Is_Name(
            MethodInfo method,
            string methodName)
        {
            var name = this.Get_Name(method);

            var output = name == methodName;
            return output;
        }

        public bool Is_Name(
            MethodInfo method,
            string methodName,
            int genericTypeInputCount)
        {
            var output = true
                && this.Is_Name(method, methodName)
                && this.Is_GenericTypeInputCount(method, genericTypeInputCount)
                ;

            return output;
        }
    }
}
