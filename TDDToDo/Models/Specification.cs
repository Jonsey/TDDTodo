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

        public List<Step> Steps { get; private set; }

        #endregion

        #region Constructors

        public Scenario(string title)
        {
            Events = new List<ScenarioEvent>();
            Steps = new List<Step>();

            SetStatus(ScenarioStatus.Created);

            Title = title;
            CreatedAt = Events[0].TimeStamp; // TODO don't like it
        }

        #endregion

        #region Public Methods

        public void SetInProgress()
        {
            InProgress = true;
            Completed = false;

            SetStatus(ScenarioStatus.InProgress);
        }

        public void SetCompleted()
        {
            Completed = true;
            InProgress = false;

            SetStatus(ScenarioStatus.Completed);
        }

        public void AddStep(string step)
        {
            Steps.Add(new Step(step));
        }

        #endregion

        #region Private Methods

        void SetStatus(ScenarioStatus status)
        {
            Events.Add(new ScenarioEvent(status));
        }

        #endregion

        
    }
}
