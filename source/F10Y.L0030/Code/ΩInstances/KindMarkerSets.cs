using System;


namespace F10Y.L0030
{
    public class KindMarkerSets : IKindMarkerSets
    {
        #region Infrastructure

        public static IKindMarkerSets Instance { get; } = new KindMarkerSets();


        private KindMarkerSets()
        {
        }

        #endregion
    }
}
