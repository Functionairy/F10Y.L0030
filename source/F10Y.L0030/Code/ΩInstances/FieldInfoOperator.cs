using System;


namespace F10Y.L0030
{
    public class FieldInfoOperator : IFieldInfoOperator
    {
        #region Infrastructure

        public static IFieldInfoOperator Instance { get; } = new FieldInfoOperator();


        private FieldInfoOperator()
        {
        }

        #endregion
    }
}
