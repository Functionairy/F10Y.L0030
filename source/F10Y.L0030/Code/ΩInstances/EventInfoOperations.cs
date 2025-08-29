using System;


namespace F10Y.L0030
{
    public class EventInfoOperations : IEventInfoOperations
    {
        #region Infrastructure

        public static IEventInfoOperations Instance { get; } = new EventInfoOperations();


        private EventInfoOperations()
        {
        }

        #endregion
    }
}
