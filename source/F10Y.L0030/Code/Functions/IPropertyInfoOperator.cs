using System;
using System.Linq;
using System.Reflection;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IPropertyInfoOperator
    {
        public Type Get_DeclaringType(PropertyInfo propertyInfo)
            => Instances.MemberInfoOperator.Get_DeclaringType(propertyInfo);

        public MethodInfo Get_GetMethod(PropertyInfo propertyInfo)
        {
            var output = propertyInfo.GetMethod;
            return output;
        }

        /// <summary>
        /// Returns an empty array if there are no parameters.
        /// </summary>
        public ParameterInfo[] Get_IndexerParameters(PropertyInfo propertyInfo)
        {
            var output = propertyInfo.GetIndexParameters();
            return output;
        }

        public string Get_PropertyName(PropertyInfo propertyInfo)
            => Instances.MemberInfoOperator.Get_Name(propertyInfo);

        public PropertyInfo Get_PropertyOf(
            Type type,
            string propertyName)
        {
            var method = type.GetProperties()
                .Where(Instances.PropertyInfoOperations.Name_Is(propertyName))
                .Single();

            return method;
        }

        public PropertyInfo Get_PropertyOf<T>(string propertyName)
        {
            var type = Instances.TypeOperator.Get_TypeOf<T>();

            var output = this.Get_PropertyOf(
                type,
                propertyName);

            return output;
        }

        public Type Get_PropertyType(PropertyInfo propertyInfo)
        {
            var output = propertyInfo.PropertyType;
            return output;
        }

        /// <summary>
        /// Is the method an indexer method?
        /// (I.e. does the get-method for the property have input parameters?
        /// </summary>
        public bool Is_Indexer(PropertyInfo propertyInfo)
        {
            //var getMethod = this.Get_GetMethod(propertyInfo);

            //var anyInputParameters = Instances.MethodInfoOperator.Has_InputParameters(getMethod);

            var indexerParameters = this.Get_IndexerParameters(propertyInfo);

            var output = indexerParameters.Any();
            return output;
        }

        public bool Is_Name(
            PropertyInfo property,
            string propertyName)
            => Instances.MemberInfoOperator.Is_Name(
                property,
                propertyName);
    }
}
