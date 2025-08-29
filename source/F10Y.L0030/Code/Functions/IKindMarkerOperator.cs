using System;
using System.Linq;

using F10Y.T0002;
using F10Y.T0011;


namespace F10Y.L0030
{
    [FunctionsMarker]
    public partial interface IKindMarkerOperator
    {
#pragma warning disable IDE1006 // Naming Styles

        [Ignore]
        public Internal.IKindMarkerOperator _Internal => Internal.KindMarkerOperator.Instance;

#pragma warning restore IDE1006 // Naming Styles


        public char Get_KindMarker_Character(string kindMarked)
        {
            var output = kindMarked.First();
            return output;
        }

        /// <summary>
        /// Chooses <see cref="Get_KindMarker_Character(string)"/> as the default.
        /// </summary>
        public char Get_KindMarker(string kindMarked)
            => this.Get_KindMarker_Character(kindMarked);

        /// <summary>
        /// Determines if the member name is marked with a member kind.
        /// </summary>
        /// <remarks>
        ///  Returns true if:
        /// <list type="number">
        /// <item><inheritdoc cref="Internal.IKindMarkerOperator.Is_KindMarked_HasMinimumLength(string)" path="/summary"/></item>
        /// <item><inheritdoc cref="Internal.IKindMarkerOperator.Is_KindMarked_HasTokenSeparator(string)" path="/summary"/></item>
        /// <item><inheritdoc cref="Internal.IKindMarkerOperator.Is_KindMarked_HasRecognizedFirstCharacter(string)" path="/summary"/></item>
        /// </list>
        /// </remarks>
        public bool Is_KindMarked(string memberName)
        {
            var hasMinimumLength = _Internal.Is_KindMarked_HasMinimumLength(memberName);
            if (!hasMinimumLength)
            {
                return false;
            }

            var hasTokenSeparator = _Internal.Is_KindMarked_HasTokenSeparator(memberName);
            if (!hasTokenSeparator)
            {
                return false;
            }

            var hasRecognizedFirstCharacter = _Internal.Is_KindMarked_HasRecognizedFirstCharacter(memberName);
            if (!hasRecognizedFirstCharacter)
            {
                return false;
            }

            return true;
        }

        public bool Is_KindMarked(
            string kindMarked,
            char memberKindMarker)
        {
            var isMemberKindMarked = this.Is_KindMarked(kindMarked);
            if (!isMemberKindMarked)
            {
                return false;
            }

            var markerKindCharacter = this.Get_KindMarker_Character(kindMarked);

            var output = markerKindCharacter == memberKindMarker;
            return output;
        }

        public bool Is_TypeKindMarked(string kindMarked)
        {
            var output = this.Is_KindMarked(
                kindMarked,
                Instances.KindMarkers.Type);

            return output;
        }

        public bool Is_NamespaceKindMarked(string kindMarked)
        {
            var output = this.Is_KindMarked(
                kindMarked,
                Instances.KindMarkers.Namespace);

            return output;
        }

        public bool Is_FieldKindMarked(string kindMarked)
        {
            var output = this.Is_KindMarked(
                kindMarked,
                Instances.KindMarkers.Field);

            return output;
        }

        public bool Is_PropertyKindMarked(string kindMarked)
        {
            var output = this.Is_KindMarked(
                kindMarked,
                Instances.KindMarkers.Property);

            return output;
        }

        public bool Is_MethodKindMarked(string kindMarked)
        {
            var output = this.Is_KindMarked(
                kindMarked,
                Instances.KindMarkers.Method);

            return output;
        }

        public bool Is_EventKindMarked(string kindMarked)
        {
            var output = this.Is_KindMarked(
                kindMarked,
                Instances.KindMarkers.Event);

            return output;
        }

        public bool Is_ErrorKindMarked(string kindMarked)
        {
            var output = this.Is_KindMarked(
                kindMarked,
                Instances.KindMarkers.Error);

            return output;
        }

        public string Make_KindMarked(
            string identityStringValue,
            char kindMarker)
        {
            var output = $"{kindMarker}{Instances.TokenSeparators.KindMarkerTokenSeparator}{identityStringValue}";
            return output;
        }

        public string Make_ErrorKindMarked(string identityStringValue)
        {
            var output = this.Make_KindMarked(
                identityStringValue,
                Instances.KindMarkers.Error);

            return output;
        }

        public string Make_EventKindMarked(string identityStringValue)
        {
            var output = this.Make_KindMarked(
                identityStringValue,
                Instances.KindMarkers.Event);

            return output;
        }

        public string Make_FieldKindMarked(string identityStringValue)
        {
            var output = this.Make_KindMarked(
                identityStringValue,
                Instances.KindMarkers.Field);

            return output;
        }

        public string Make_MethodKindMarked(string identityStringValue)
        {
            var output = this.Make_KindMarked(
                identityStringValue,
                Instances.KindMarkers.Method);

            return output;
        }

        public string Make_PropertyKindMarked(string identityStringValue)
        {
            var output = this.Make_KindMarked(
                identityStringValue,
                Instances.KindMarkers.Property);

            return output;
        }

        /// <summary>
        /// Prefixes the identity string with the type kind-marker value.
        /// </summary>
        public string Make_TypeKindMarked(string identityStringValue)
        {
            var output = this.Make_KindMarked(
                identityStringValue,
                Instances.KindMarkers.Type);

            return output;
        }

        public string Remove_KindMark(string kindMarked)
        {
            var output = kindMarked[2..];
            return output;
        }

        public string Remove_TypeKindMarker(string kindMarked)
        {
            var output = Instances.StringOperator.Get_Substring_From_Exclusive(
                Instances.TokenSeparators.KindMarkerTokenSeparator,
                kindMarked);

            return output;
        }

        public T Switch_OnKindMarker<T>(
            string kindMarked,
            T for_Event,
            T for_Field,
            T for_Method,
            T for_Namespace,
            T for_Property,
            T for_Type,
            T for_Error)
        {
            var kindMarker = this.Get_KindMarker_Character(kindMarked);

            T output = kindMarker switch
            {
                IKindMarkers.Error_Constant => for_Error,
                IKindMarkers.Event_Constant => for_Event,
                IKindMarkers.Field_Constant => for_Field,
                IKindMarkers.Method_Constant => for_Method,
                IKindMarkers.Namespace_Constant => for_Namespace,
                IKindMarkers.Property_Constant => for_Property,
                IKindMarkers.Type_Constant => for_Type,
                _ => throw Instances.SwitchOperator.Get_DefaultCaseException(kindMarker)
            };

            return output;
        }

        public void Switch_OnKindMarker(
            string kindMarked,
            Action for_Event,
            Action for_Field,
            Action for_Method,
            Action for_Namespace,
            Action for_Property,
            Action for_Type,
            Action for_Error)
        {
            var action = this.Switch_OnKindMarker<Action>(
                kindMarked,
                for_Event,
                for_Field,
                for_Method,
                for_Namespace,
                for_Property,
                for_Type,
                for_Error);

            action();
        }
    }
}
