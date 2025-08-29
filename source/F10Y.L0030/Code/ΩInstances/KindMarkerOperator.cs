using System;


namespace F10Y.L0030
{
    public class KindMarkerOperator : IKindMarkerOperator
    {
        #region Infrastructure

        public static IKindMarkerOperator Instance { get; } = new KindMarkerOperator();


        private KindMarkerOperator()
        {
        }

        #endregion
    }
}


namespace F10Y.L0030.Internal
{
    public class KindMarkerOperator : IKindMarkerOperator
    {
        #region Infrastructure

        public static IKindMarkerOperator Instance { get; } = new KindMarkerOperator();


        private KindMarkerOperator()
        {
        }

        #endregion
    }
}