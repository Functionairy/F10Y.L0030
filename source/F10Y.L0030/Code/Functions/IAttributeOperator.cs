using System;
using System.Linq;
using System.Reflection;

using F10Y.T0002;
using F10Y.T0011;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IAttributeOperator :
        L0000.IAttributeOperator
    {
#pragma warning disable IDE1006 // Naming Styles

        [Ignore]
        public L0000.IAttributeOperator _L0000 => L0000.AttributeOperator.Instance;

#pragma warning restore IDE1006 // Naming Styles


        public AttributeSignature Get_AttributeSignature(CustomAttributeData attribute)
        {
            var type = this.Get_AttributeType(attribute);

            var typeSignature = Instances.SignatureOperator.Get_Signature_ForType(
                type,
                // Do not include attributes on type.
                false);

            var constructorArguments = this.Get_ConstructorArguments(attribute)
                .Select(this.Get_TypedArgumentSignature)
                .Now();

            var namedArguments = this.Get_NamedArguments(attribute)
                .Select(this.Get_NamedArgumentSignature)
                .Now();

            var output = new AttributeSignature
            {
                Type = typeSignature,
                ConstructorArguments = constructorArguments,
                NamedArguments = namedArguments,
            };

            return output;
        }

        public NamedArgumentSignature Get_NamedArgumentSignature(CustomAttributeNamedArgument namedArgument)
        {
            var name = namedArgument.MemberName;

            var typedArgument = this.Get_TypedArgumentSignature(namedArgument.TypedValue);

            var output = new NamedArgumentSignature
            {
                Name = name,
                TypedValue = typedArgument,
            };

            return output;
        }

        public TypedArgumentSignature Get_TypedArgumentSignature(CustomAttributeTypedArgument typedArgument)
        {
            var typeSignature = Instances.SignatureOperator.Get_Signature_ForType(
                typedArgument.ArgumentType,
                // Do not get attributes of attribute.
                false);

            var value = typedArgument.Value;

            var output = new TypedArgumentSignature
            {
                Type = typeSignature,
                Value = value
            };

            return output;
        }
    }
}
