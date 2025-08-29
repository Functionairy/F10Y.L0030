using System;


namespace F10Y.L0030
{
    public class NamespacedTypeNames : INamespacedTypeNames
    {
        #region Infrastructure

        public static INamespacedTypeNames Instance { get; } = new NamespacedTypeNames();


        private NamespacedTypeNames()
        {
        }

        #endregion
    }
}
