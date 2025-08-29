using System;
using System.Linq;

using F10Y.T0002;


namespace F10Y.L0030.Internal
{
    [FunctionsMarker]
    public partial interface IKindMarkerOperator
    {
        /// <summary>
        /// Is the member name at least two characters long, to allow for the member kind marker character and member kind marker token separator character?
        /// </summary>
        public bool Is_KindMarked_HasMinimumLength(string memberName)
        {
            var length = memberName.Length;

            // Allow empty member names, so just test if length is 2 or greater.
            var isMinimumLength = length > 1;
            return isMinimumLength;
        }

        /// <summary>
        /// Is the second character the member kind marker token separtor (":")?
        /// </summary>
        public bool Is_KindMarked_HasTokenSeparator(string memberName)
        {
            var secondCharacter = memberName.Second();

            var output = Instances.TokenSeparators.KindMarkerTokenSeparator == secondCharacter;
            return output;
        }

        /// <summary>
        /// Is the first character one of the recognized member kind markers?
        /// </summary>
        public bool Is_KindMarked_HasRecognizedFirstCharacter(string memberName)
        {
            var possibleMarkerKindCharacter = Instances.KindMarkerOperator.Get_KindMarker_Character(memberName);

            var allMarkersHash = Instances.KindMarkerSets.All_AsHash;

            var output = allMarkersHash.Contains(possibleMarkerKindCharacter);
            return output;
        }
    }
}
