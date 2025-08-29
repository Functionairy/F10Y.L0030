using System;
using System.Threading.Tasks;


namespace F10Y.L0030.Construction
{
    class Program
    {
        static async Task Main()
        {
            await Program.Demonstrations_IdentityString();
            //await Program.Demonstrations_Signature();
        }

        #region Demonstrations

        public static async Task Demonstrations_IdentityString()
        {
            await IdentityStringDemonstrations.Instance
                .Get_IdentityString_OfMember()
                ;
        }

        public static async Task Demonstrations_Signature()
        {
            //await SignatureDemonstrations.Instance
                
            //    ;
        }

        #endregion
    }
}