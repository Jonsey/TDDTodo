using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TDDToDo.Models;

namespace TDDToDo.Tests
{
    
    // Feature API is not intuitive
    // Completed item set to inprogress should not be completed
    // Completed item should not be in progress
    // Flag item as completed
    // Flag item as in progress 
    // Add an item to the list
    // Give list a category
    // List of items 

    [TestFixture]
    public class CreateFeatureTests
    {
        const string Title = "Make excellent bit of software";
        const string Detail = "In order the do something As a user I want to be able to do something";

        Feature feature;
        Specification specification;

        void InitialiseTodoList()
        {
            feature = new Feature(Title, Detail);
        }

        void AddTodoItem()
        {
            specification = new Specification("Title");
            feature.AddItem(specification);
        }

        [Test]
        public void ShouldBeAbleToCreateANewTodoList()
        {
            InitialiseTodoList();
            Assert.IsNotNull(feature);
        }

        [Test]
        public void ShouldBeAbleToAddANewItem()
        {
            InitialiseTodoList();
            AddTodoItem();

            Assert.AreEqual(specification, feature.Specifications[0]);
        }

        [Test]
        public void ListShouldHaveATitle()
        {
            InitialiseTodoList();

            Assert.AreEqual(Title, feature.Title);
        }

        [Test]
        public void ListShouldHaveDetail()
        {
            InitialiseTodoList();

            Assert.AreEqual(Detail, feature.Detail);
        }

        [Test]
        public void ShouldMarkAsInProgress()
        {
            InitialiseTodoList();
            AddTodoItem();

            feature.Specifications[0].SetInProgress();

            Assert.IsTrue(feature.Specifications[0].InProgress);
        }

        [Test]
        public void ShouldMarkAsCompleted()
        {
            InitialiseTodoList();
            AddTodoItem();

            feature.Specifications[0].SetCompleted();

            Assert.IsTrue(feature.Specifications[0].Completed);
        }

        [Test]
        public void InProgressItemShouldNotBeInprogressAfterCompletion()
        {
            InitialiseTodoList();
            AddTodoItem();

            feature.Specifications[0].SetInProgress();
            feature.Specifications[0].SetCompleted();

            Assert.IsFalse(feature.Specifications[0].InProgress);
        }

        [Test]
        public void CompletedItemSetToInprogressShouldNotBeCompleted()
        {
            InitialiseTodoList();
            AddTodoItem();

            feature.Specifications[0].SetCompleted();
            feature.Specifications[0].SetInProgress();

            Assert.IsFalse(feature.Specifications[0].Completed);
        }
    }
}
