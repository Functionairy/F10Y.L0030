using System;


namespace F10Y.L0030
{
    public class TypeNameAffixSets : ITypeNameAffixSets
    {
        #region Infrastructure

        public static ITypeNameAffixSets Instance { get; } = new TypeNameAffixSets();


        private TypeNameAffixSets()
        {
        }

        #endregion
    }
}
