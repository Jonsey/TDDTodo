using System;

namespace TDDToDo.Models
{
    [Serializable]
    public class SpecificationEvent
    {
        #region Constructors

        public SpecificationEvent(SpecificationEventType eventType)
        {
            TimeStamp = DateTime.Now;
            EventType = eventType;
        } 

        #endregion

        #region Properties

        public DateTime TimeStamp { get; private set; }

        public SpecificationEventType EventType { get; private set; } 

        #endregion
    }
}