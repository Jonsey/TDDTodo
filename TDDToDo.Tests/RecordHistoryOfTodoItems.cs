using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TDDToDo.Models;

namespace TDDToDo.Tests
{
    // Record history of todos
    // Log changes as events


    [TestFixture]
    public class RecordHistoryOfTodoItems
    {
        [Test]
        public void TodoItemShouldExposeCreatedAt()
        {
            var timestamp = DateTime.Now;
            var item = new TodoItem("Title");

            Assert.AreEqual(timestamp, item.CreatedAt);
        }

        [Test]
        public void TodoItemShouldBeCreatedWithATimeStamp()
        {
            var timestamp = DateTime.Now;
            var item = new TodoItem("Title");

            Assert.AreEqual(timestamp, item.Events[0].TimeStamp);
        }

        [Test]
        public void TodoEventShouldHaveAType()
        {
            var item = new TodoItem("Title");

            Assert.AreEqual(TodoEventType.Created, item.Events[0].EventType);
        }

        [Test]
        public void ShouldRecordInProgressEvent()
        {
            var item = new TodoItem("Title");
            item.SetInProgress();

            Assert.AreEqual(TodoEventType.SetInProgress, item.Events[1].EventType); 
        }

        [Test]
        public void ShouldRecordCompletedEvent()
        {
            var item = new TodoItem("Title");
            item.SetCompleted();

            Assert.AreEqual(TodoEventType.Completed, item.Events[1].EventType);
        }

        [Test]
        public void ShouldRecordAllEvents()
        {
            var item = new TodoItem("Title");
            item.SetInProgress();
            item.SetCompleted();

            Assert.AreEqual(TodoEventType.Created, item.Events[0].EventType);
            Assert.AreEqual(TodoEventType.SetInProgress, item.Events[1].EventType);
            Assert.AreEqual(TodoEventType.Completed, item.Events[2].EventType);
        }


    }
}
