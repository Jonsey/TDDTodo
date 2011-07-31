using System;
using System.Collections.Generic;

namespace TDDToDo.Models
{
    [Serializable]
    public class Specification
    {
        #region Properties

        public bool InProgress { get; set; }
        public bool Completed { get; set; }
        public DateTime CreatedAt { get; private set; }
        public string Title { get; private set; }

        public List<SpecificationEvent> Events { get; private set; }

        #endregion

        #region Constructors

        public Specification(string title)
        {
            Events = new List<SpecificationEvent>();
            RecordEvent(SpecificationEventType.Created);

            Title = title;
            CreatedAt = Events[0].TimeStamp; // TODO don't like it
        }

        #endregion

        #region Public Methods

        public void SetInProgress()
        {
            InProgress = true;
            Completed = false;

            RecordEvent(SpecificationEventType.SetInProgress);
        }

        public void SetCompleted()
        {
            Completed = true;
            InProgress = false;

            RecordEvent(SpecificationEventType.Completed);
        }

        #endregion

        #region Private Methods

        void RecordEvent(SpecificationEventType eventType)
        {
            Events.Add(new SpecificationEvent(eventType));
        }

        #endregion
    }
}
