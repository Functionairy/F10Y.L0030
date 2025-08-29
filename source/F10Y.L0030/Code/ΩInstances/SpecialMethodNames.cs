using System;


namespace F10Y.L0030
{
    public class SpecialMethodNames : ISpecialMethodNames
    {
        #region Infrastructure

        public static ISpecialMethodNames Instance { get; } = new SpecialMethodNames();


        private SpecialMethodNames()
        {
        }

        #endregion
    }
}
