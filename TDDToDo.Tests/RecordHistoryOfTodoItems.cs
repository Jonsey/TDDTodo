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
            var item = new Specification("Title");

            Assert.AreEqual(timestamp, item.CreatedAt);
        }

        [Test]
        public void TodoItemShouldBeCreatedWithATimeStamp()
        {
            var timestamp = DateTime.Now;
            var item = new Specification("Title");

            Assert.AreEqual(timestamp, item.Events[0].TimeStamp);
        }

        [Test]
        public void TodoEventShouldHaveAType()
        {
            var item = new Specification("Title");

            Assert.AreEqual(SpecificationEventType.Created, item.Events[0].EventType);
        }

        [Test]
        public void ShouldRecordInProgressEvent()
        {
            var item = new Specification("Title");
            item.SetInProgress();

            Assert.AreEqual(SpecificationEventType.SetInProgress, item.Events[1].EventType); 
        }

        [Test]
        public void ShouldRecordCompletedEvent()
        {
            var item = new Specification("Title");
            item.SetCompleted();

            Assert.AreEqual(SpecificationEventType.Completed, item.Events[1].EventType);
        }

        [Test]
        public void ShouldRecordAllEvents()
        {
            var item = new Specification("Title");
            item.SetInProgress();
            item.SetCompleted();

            Assert.AreEqual(SpecificationEventType.Created, item.Events[0].EventType);
            Assert.AreEqual(SpecificationEventType.SetInProgress, item.Events[1].EventType);
            Assert.AreEqual(SpecificationEventType.Completed, item.Events[2].EventType);
        }


    }
}
