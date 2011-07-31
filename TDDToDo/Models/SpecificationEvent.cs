using System;

namespace TDDToDo.Models
{
    [Serializable]
    public class ScenarioEvent
    {
        #region Constructors

        public ScenarioEvent(ScenarioEventType eventType)
        {
            TimeStamp = DateTime.Now;
            EventType = eventType;
        } 

        #endregion

        #region Properties

        public DateTime TimeStamp { get; private set; }

        public ScenarioEventType EventType { get; private set; } 

        #endregion
    }
}