using System;
using System.Linq;

using F10Y.T0002;


namespace F10Y.L0030
{
    public partial interface ISignatureStringOperator
    {
        AttributeSignature[] Parse_AttributeSignatureList(string attributeSignatureListSegment)
        {
            var attributeSignature_Values = Instances.StringOperator.Split(
                "][",
                attributeSignatureListSegment)
                .Trim(
                    Instances.TokenSeparators.AttributeCloseSeparator,
                    Instances.TokenSeparators.AttributeOpenSeparator
                )
                .Now();

            var output = attributeSignature_Values
                .Select(this.Parse_AttributeSignature_Value)
                .Now();

            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Example: [T:D8S.S0015.ExampleWithNamedArgumentAttribute((T:System.Int32)10, StringValue = (T:System.String)test, BooleanValue = (T:System.Boolean)True)]
        /// </remarks>
        AttributeSignature Parse_AttributeSignature(string attributeSignature)
        {
            var attributeSignature_Value = this.Get_AttributeSignature_Value(attributeSignature);

            var output = this.Parse_AttributeSignature_Value(attributeSignature_Value);
            return output;
        }

        AttributeSignature Parse_AttributeSignature_Value(string attributeSignature_Value)
        {
            var (typeSignatureString, argumentsListSegment_OrEmpty) = this.Decompose_AttributeSignature_Value(attributeSignature_Value);

            var typeSignature = Instances.SignatureOperator.Get_TypeSignature(typeSignatureString);

            var output = new AttributeSignature
            {
                Type = typeSignature,
            };

            var constructorArguments = Instances.ListOperator.New<TypedArgumentSignature>();
            var namedArguments = Instances.ListOperator.New<NamedArgumentSignature>();

            var hasArguments = Instances.StringOperator.Is_NotEmpty(argumentsListSegment_OrEmpty);
            if(hasArguments)
            {
                var arguments = Instances.StringOperator.Split(
                    Instances.TokenSeparators.ArgumentListSeparator,
                    argumentsListSegment_OrEmpty)
                    .Trim()
                    .Now();

                foreach (var argument in arguments)
                {
                    var isNamedArgument = this.Is_NamedArgumentSignatureString(argument);
                    if(isNamedArgument)
                    {
                        var namedArgumentSignature = this.Parse_NamedArgumentSignatureString(argument);

                        namedArguments.Add(namedArgumentSignature);
                    }
                    else
                    {
                        var typedArgumentSignature = this.Parse_TypedArgmentSignatureString(argument);

                        constructorArguments.Add(typedArgumentSignature);
                    }
                }
            }

            output.ConstructorArguments = constructorArguments.ToArray();
            output.NamedArguments = namedArguments.ToArray();

            return output;
        }

        NamedArgumentSignature Parse_NamedArgumentSignatureString(string namedArgumentSignatureString)
        {
            var (argumentName, typedArgumentSignatureString) = this.Decompose_NamedArgumentSignatureString(namedArgumentSignatureString);

            var typedArgumentSignature = this.Parse_TypedArgmentSignatureString(typedArgumentSignatureString);

            var output = new NamedArgumentSignature
            {
                Name = argumentName,
                TypedValue = typedArgumentSignature,
            };

            return output;
        }

        (string argumentName, string typedArgumentSignatureString) Decompose_NamedArgumentSignatureString(string namedArgumentSignatureString)
        {
            var hasEquals = Instances.StringOperator.Has_IndexOf_First(
                namedArgumentSignatureString,
                out var index_OrNotFound,
                Instances.TokenSeparators.NamedArgumentTokenSeparator);

            if(!hasEquals)
            {
                throw Instances.ExceptionOperator.From("Named argument signature strings must contain an equals ('=') sign.");
            }

            var (argumentName_NeedingTrim, typedArgumentSignatureString_NeedingTrim) = Instances.StringOperator.Split_OnIndex(
                index_OrNotFound,
                namedArgumentSignatureString);

            var argumentName = Instances.StringOperator.Trim(argumentName_NeedingTrim);
            var typedArgumentSignatureString = Instances.StringOperator.Trim(typedArgumentSignatureString_NeedingTrim);

            return (argumentName, typedArgumentSignatureString);
        }

        TypedArgumentSignature Parse_TypedArgmentSignatureString(string typedArgumentSignatureString)
        {
            var hasCloseParenthesis = Instances.StringOperator.Has_IndexOf_First(
                typedArgumentSignatureString,
                out var index_OrNotFound,
                Instances.TokenSeparators.ParameterListCloseTokenSeparator);

            if(!hasCloseParenthesis)
            {
                throw Instances.ExceptionOperator.From("Typed argument signature string must contain ')'.");
            }

            var (typeSignatureString_NeedingTrim, valueString) = Instances.StringOperator.Split_OnIndex(
                index_OrNotFound,
                typedArgumentSignatureString);

            var typeSignatureString = typeSignatureString_NeedingTrim.TrimStart(
                Instances.TokenSeparators.ParameterListOpenTokenSeparator);

            var typeSignature = Instances.SignatureOperator.Get_TypeSignature(typeSignatureString);

            var output = new TypedArgumentSignature
            {
                Type = typeSignature,
                // TODO: convert value into object-typed actual value based on type siganture string.
                // For now, just use the string.
                Value = valueString,
            };

            return output;
        }

        bool Is_NamedArgumentSignatureString(string argumentSignatureString)
        {
            // Does it contain an equals sign?
            var output = Instances.StringOperator.Contains(
                argumentSignatureString,
                Instances.TokenSeparators.NamedArgumentTokenSeparator);

            return output;
        }

        (string typeSignatureString, string argumentsListSegment_OrEmpty) Decompose_AttributeSignature_Value(string attributeSignature_Value)
        {
            // Find the open parenthesis, and split on it.
            var hasOpenParenthesis = Instances.StringOperator.Has_IndexOf_First(
                attributeSignature_Value,
                out var index_OrNotFound,
                Instances.TokenSeparators.ParameterListOpenTokenSeparator);

            if(!hasOpenParenthesis)
            {
                throw Instances.ExceptionOperator.From("Attribute signature must have parameter list parentheses.");
            }

            var (typeSignatureString, argumentsListSegment_NeedingTrim) = Instances.StringOperator.Split_OnIndex(
                index_OrNotFound,
                attributeSignature_Value);

            var argumentsListSegment = argumentsListSegment_NeedingTrim.TrimEnd(
                Instances.TokenSeparators.ParameterListCloseTokenSeparator);

            return (typeSignatureString, argumentsListSegment);
        }

        /// <summary>
        /// Get the contents of the attribute braces (the content between the '[' and ']' of an attribute signature string).
        /// </summary>
        string Get_AttributeSignature_Value(string attributeSignature)
        {
            var output = Instances.StringOperator.Trim(
                attributeSignature,
                Instances.TokenSeparators.ArrayCloseSeparator,
                Instances.TokenSeparators.ArrayOpenSeparator);

            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        (string nonAttributedSegment, string attributeListSegment_OrDefault) Decompose_AttributeSegment(string signatureString)
        {
            this.Has_Attributes(
                signatureString,
                out var nonAttributedSegment,
                out var attributeListSegment_OrDefault);

            return (nonAttributedSegment, attributeListSegment_OrDefault);
        }

        bool Has_Attributes(
            string signatureString,
            out string nonAttributedSegment,
            out string attributeListSegment_OrDefault)
        {
            // Example signture string:
            // M:System.Console.{get_IsInputRedirected}g__EnsureInitialized|34_0()~System.Runtime.CompilerServices.StrongBox`1<System.Boolean> [T:System.Runtime.CompilerServices.CompilerGeneratedAttribute()][T:System.Runtime.Versioning.UnsupportedOSPlatformAttribute((T:System.String)tvos)]

            // Algorithm: find the last "[ " ('[' after a space).
            // Use that to split the string.

            var output = Instances.StringOperator.Has_IndexOf_Last(
                signatureString,
                out var index_OrNotFound,
                Instances.TokenSeparators.AttributeListIndicationSeparator);

            if(output)
            {
                nonAttributedSegment = Instances.StringOperator.Get_Substring_Upto_Exclusive(
                    index_OrNotFound,
                    signatureString);

                // Skip the space in the attribute list indicator.
                attributeListSegment_OrDefault = Instances.StringOperator.Get_Substring_FromExclusive(
                    index_OrNotFound,
                    signatureString);
            }
            else
            {
                nonAttributedSegment = signatureString;

                attributeListSegment_OrDefault = default;
            }

            return output;
        }
    }
}
