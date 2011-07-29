using System;
using System.Collections.Generic;

namespace TDDToDo.Models
{
    [Serializable]
    public class TodoItem
    {
        #region Properties

        public bool InProgress { get; set; }
        public bool Completed { get; set; }
        public DateTime CreatedAt { get; private set; }
        public string Title { get; private set; }

        public List<TodoEvent> Events { get; private set; }

        #endregion

        #region Constructors

        public TodoItem(string title)
        {
            Events = new List<TodoEvent>();
            RecordEvent(TodoEventType.Created);

            Title = title;
            CreatedAt = Events[0].TimeStamp; // TODO don't like it
        }

        #endregion

        #region Public Methods

        public void SetInProgress()
        {
            InProgress = true;
            Completed = false;

            RecordEvent(TodoEventType.SetInProgress);
        }

        public void SetCompleted()
        {
            Completed = true;
            InProgress = false;

            RecordEvent(TodoEventType.Completed);
        }

        #endregion

        #region Private Methods

        void RecordEvent(TodoEventType eventType)
        {
            Events.Add(new TodoEvent(eventType));
        }

        #endregion
    }
}
