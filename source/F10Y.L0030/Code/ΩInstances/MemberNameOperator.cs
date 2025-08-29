using System;


namespace F10Y.L0030
{
    public class MemberNameOperator : IMemberNameOperator
    {
        #region Infrastructure

        public static IMemberNameOperator Instance { get; } = new MemberNameOperator();


        private MemberNameOperator()
        {
        }

        #endregion
    }
}
