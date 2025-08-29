using System;


namespace F10Y.L0030
{
    public class EventInfoOperator : IEventInfoOperator
    {
        #region Infrastructure

        public static IEventInfoOperator Instance { get; } = new EventInfoOperator();


        private EventInfoOperator()
        {
        }

        #endregion
    }
}
