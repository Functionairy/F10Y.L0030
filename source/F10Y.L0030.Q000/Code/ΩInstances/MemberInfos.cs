using System;


namespace F10Y.L0030.Q000
{
    public class MemberInfos : IMemberInfos
    {
        #region Infrastructure

        public static IMemberInfos Instance { get; } = new MemberInfos();


        private MemberInfos()
        {
        }

        #endregion
    }
}
