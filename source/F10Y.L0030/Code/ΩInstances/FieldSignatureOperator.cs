using System;


namespace F10Y.L0030
{
    public class FieldSignatureOperator : IFieldSignatureOperator
    {
        #region Infrastructure

        public static IFieldSignatureOperator Instance { get; } = new FieldSignatureOperator();


        private FieldSignatureOperator()
        {
        }

        #endregion
    }
}
