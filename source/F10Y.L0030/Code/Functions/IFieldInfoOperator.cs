using System;
using System.Linq;
using System.Reflection;

using F10Y.T0002;
using F10Y.T0011;


namespace F10Y.L0030
{
    /// <inheritdoc cref="L0000.IFieldInfoOperator" path="/summary"/>
    /// <remarks>
    /// <inheritdoc cref="Documentation.Project_SelfDescription" path="/summary"/>
    /// </remarks>
    [FunctionsMarker]
    public partial interface IFieldInfoOperator :
        L0000.IFieldInfoOperator
    {
#pragma warning disable IDE1006 // Naming Styles

        [Ignore]
        L0000.IFieldInfoOperator _L0000 => L0000.FieldInfoOperator.Instance;

#pragma warning restore IDE1006 // Naming Styles


        /// <summary>
        /// Quality-of-life overload for <see cref="Get_Name(FieldInfo)"/>.
        /// </summary>
        string Get_FieldName(FieldInfo fieldInfo)
            => this.Get_Name(fieldInfo);

        FieldInfo Get_FieldOf(
            Type type,
            string fieldName)
        {
            var method = type.GetFields()
                .Where(Instances.FieldInfoOperations.Name_Is(fieldName))
                .Single();

            return method;
        }

        FieldInfo Get_FieldOf<T>(string fieldName)
        {
            var type = Instances.TypeOperator.Get_TypeOf<T>();

            var output = this.Get_FieldOf(
                type,
                fieldName);

            return output;
        }

        string Get_Name(FieldInfo field)
            => Instances.MemberInfoOperator.Get_Name(field);

        bool Is_Name(
            FieldInfo field,
            string fieldName)
            => Instances.MemberInfoOperator.Is_Name(
                field,
                fieldName);
    }
}
