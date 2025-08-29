using System;


namespace F10Y.L0030
{
    public class PropertyInfoOperations : IPropertyInfoOperations
    {
        #region Infrastructure

        public static IPropertyInfoOperations Instance { get; } = new PropertyInfoOperations();


        private PropertyInfoOperations()
        {
        }

        #endregion
    }
}
