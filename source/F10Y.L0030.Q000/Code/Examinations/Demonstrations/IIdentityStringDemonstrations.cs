using System;
using System.Linq;
using System.Threading.Tasks;

using F10Y.T0006;
using F10Y.T0014.T001;


namespace F10Y.L0030.Q000
{
    [DemonstrationsMarker]
    public partial interface IIdentityStringDemonstrations :
        IScriptTextOutputInfrastructure_Definition
    {
        [DemonstrationOfMarker("M:F10Y.L0030.IIdentityStringOperator.Get_IdentityString(System.Reflection.MemberInfo)")]
        [DemonstrationOfMarker( "T:F10Y.T0006.DemonstrationOfMarkerAttribute")]
        public async Task Get_IdentityString_OfMember()
        {
            /// Inputs.
            var memberInfo = Instances.MemberInfos
                .Example_Type
                ;


            /// Run.
            var identityString = Instances.IdentityStringOperator.Get_IdentityString(memberInfo);

            var lines_ForOutput = Instances.EnumerableOperator.From("Identity String:")
                .Append_BlankLine()
                .Append(identityString)
                ;

            await this.Write_Lines_AndOpen(lines_ForOutput);
        }
    }
}
