using System;


namespace F10Y.L0030
{
    public class MemberInfoOperations : IMemberInfoOperations
    {
        #region Infrastructure

        public static IMemberInfoOperations Instance { get; } = new MemberInfoOperations();


        private MemberInfoOperations()
        {
        }

        #endregion
    }
}
