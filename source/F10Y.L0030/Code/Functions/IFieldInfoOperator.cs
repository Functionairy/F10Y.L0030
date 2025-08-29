using System;
using System.Linq;
using System.Reflection;

using F10Y.T0002;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IFieldInfoOperator
    {
        public Type Get_DeclaringType(FieldInfo fieldInfo)
            => Instances.MemberInfoOperator.Get_DeclaringType(fieldInfo);

        public Type Get_FieldType(FieldInfo fieldInfo)
        {
            var output = fieldInfo.FieldType;
            return output;
        }

        /// <summary>
        /// Quality-of-life overload for <see cref="Get_Name(FieldInfo)"/>.
        /// </summary>
        public string Get_FieldName(FieldInfo fieldInfo)
            => this.Get_Name(fieldInfo);

        public FieldInfo Get_FieldOf(
            Type type,
            string fieldName)
        {
            var method = type.GetFields()
                .Where(Instances.FieldInfoOperations.Name_Is(fieldName))
                .Single();

            return method;
        }

        public FieldInfo Get_FieldOf<T>(string fieldName)
        {
            var type = Instances.TypeOperator.Get_TypeOf<T>();

            var output = this.Get_FieldOf(
                type,
                fieldName);

            return output;
        }

        public string Get_Name(FieldInfo field)
            => Instances.MemberInfoOperator.Get_Name(field);

        public bool Is_Name(
            FieldInfo field,
            string fieldName)
            => Instances.MemberInfoOperator.Is_Name(
                field,
                fieldName);
    }
}
