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
            var item = new Scenario("Title");

            Assert.AreEqual(timestamp, item.CreatedAt);
        }

        [Test]
        public void TodoItemShouldBeCreatedWithATimeStamp()
        {
            var timestamp = DateTime.Now;
            var item = new Scenario("Title");

            Assert.AreEqual(timestamp, item.Events[0].TimeStamp);
        }

        [Test]
        public void TodoEventShouldHaveAType()
        {
            var item = new Scenario("Title");

            Assert.AreEqual(ScenarioStatus.Created, item.Events[0].Status);
        }

        [Test]
        public void ShouldRecordInProgressEvent()
        {
            var item = new Scenario("Title");
            item.SetInProgress();

            Assert.AreEqual(ScenarioStatus.InProgress, item.Events[1].Status); 
        }

        [Test]
        public void ShouldRecordCompletedEvent()
        {
            var item = new Scenario("Title");
            item.SetCompleted();

            Assert.AreEqual(ScenarioStatus.Completed, item.Events[1].Status);
        }

        [Test]
        public void ShouldRecordAllEvents()
        {
            var item = new Scenario("Title");
            item.SetInProgress();
            item.SetCompleted();

            Assert.AreEqual(ScenarioStatus.Created, item.Events[0].Status);
            Assert.AreEqual(ScenarioStatus.InProgress, item.Events[1].Status);
            Assert.AreEqual(ScenarioStatus.Completed, item.Events[2].Status);
        }


    }
}
