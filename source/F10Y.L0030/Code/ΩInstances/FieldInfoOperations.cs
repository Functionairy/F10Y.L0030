using System;


namespace F10Y.L0030
{
    public class FieldInfoOperations : IFieldInfoOperations
    {
        #region Infrastructure

        public static IFieldInfoOperations Instance { get; } = new FieldInfoOperations();


        private FieldInfoOperations()
        {
        }

        #endregion
    }
}
