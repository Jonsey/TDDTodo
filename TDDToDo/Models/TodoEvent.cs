using System;

namespace TDDToDo.Models
{
    [Serializable]
    public class TodoEvent
    {
        #region Constructors

        public TodoEvent(TodoEventType eventType)
        {
            TimeStamp = DateTime.Now;
            EventType = eventType;
        } 

        #endregion

        #region Properties

        public DateTime TimeStamp { get; private set; }

        public TodoEventType EventType { get; private set; } 

        #endregion
    }
}