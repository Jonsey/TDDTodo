using System;
using System.Collections.Generic;

namespace TDDToDo.Models
{
    [Serializable]
    public class Scenario
    {
        #region Properties

        public bool InProgress { get; set; }
        public bool Completed { get; set; }
        public DateTime CreatedAt { get; private set; }
        public string Title { get; private set; }

        public List<ScenarioEvent> Events { get; private set; }

        #endregion

        #region Constructors

        public Scenario(string title)
        {
            Events = new List<ScenarioEvent>();
            RecordEvent(ScenarioEventType.Created);

            Title = title;
            CreatedAt = Events[0].TimeStamp; // TODO don't like it
        }

        #endregion

        #region Public Methods

        public void SetInProgress()
        {
            InProgress = true;
            Completed = false;

            RecordEvent(ScenarioEventType.SetInProgress);
        }

        public void SetCompleted()
        {
            Completed = true;
            InProgress = false;

            RecordEvent(ScenarioEventType.Completed);
        }

        #endregion

        #region Private Methods

        void RecordEvent(ScenarioEventType eventType)
        {
            Events.Add(new ScenarioEvent(eventType));
        }

        #endregion
    }
}
