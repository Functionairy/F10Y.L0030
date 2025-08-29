using System;


namespace F10Y.L0030
{
    public class MemberInfoOperator : IMemberInfoOperator
    {
        #region Infrastructure

        public static IMemberInfoOperator Instance { get; } = new MemberInfoOperator();


        private MemberInfoOperator()
        {
        }

        #endregion
    }
}
