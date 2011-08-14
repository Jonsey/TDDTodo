using System;

namespace TDDToDo.Models
{
    [Serializable]
    public class ScenarioEvent
    {
        #region Constructors

        public ScenarioEvent(ScenarioStatus status)
        {
            TimeStamp = DateTime.Now;
            Status = status;
        } 

        #endregion

        #region Properties

        public DateTime TimeStamp { get; private set; }

        public ScenarioStatus Status { get; private set; } 

        #endregion
    }
}