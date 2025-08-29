using System;


namespace F10Y.L0030
{
    public class EventSignatureOperator : IEventSignatureOperator
    {
        #region Infrastructure

        public static IEventSignatureOperator Instance { get; } = new EventSignatureOperator();


        private EventSignatureOperator()
        {
        }

        #endregion
    }
}
