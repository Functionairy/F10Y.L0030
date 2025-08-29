using System;
using System.Collections.Generic;


namespace F10Y.L0030
{
    /// <summary>
    /// Compares two type signature instances on the basis of their (simple) type names alone.
    /// </summary>
    public class TypeNameBasedTypeSignatureEqualityComparer : IEqualityComparer<TypeSignature>
    {
        #region Static

        public static TypeNameBasedTypeSignatureEqualityComparer Instance { get; } = new TypeNameBasedTypeSignatureEqualityComparer();

        #endregion


        public bool Equals(TypeSignature x, TypeSignature y)
        {
            var output = x.TypeName == y.TypeName;
            return output;
        }

        public int GetHashCode(TypeSignature obj)
        {
            var output = obj.TypeName.GetHashCode();
            return output;
        }
    }
}
