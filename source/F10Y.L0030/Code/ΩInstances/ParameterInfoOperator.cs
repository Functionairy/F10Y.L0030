using System;


namespace F10Y.L0030
{
    public class ParameterInfoOperator : IParameterInfoOperator
    {
        #region Infrastructure

        public static IParameterInfoOperator Instance { get; } = new ParameterInfoOperator();


        private ParameterInfoOperator()
        {
        }

        #endregion
    }
}
