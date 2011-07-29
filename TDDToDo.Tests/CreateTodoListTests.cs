using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TDDToDo.Models;

namespace TDDToDo.Tests
{
    
    // TodoList API is not intuitive
    // Completed item set to inprogress should not be completed
    // Completed item should not be in progress
    // Flag item as completed
    // Flag item as in progress 
    // Add an item to the list
    // Give list a category
    // List of items 

    [TestFixture]
    public class CreateTodoListTests
    {
        const string ListTitle = "Make excellent bit of software";

        TodoList list;
        TodoItem todoItem;

        void InitialiseTodoList()
        {
            list = new TodoList(ListTitle);
        }

        void AddTodoItem()
        {
            todoItem = new TodoItem("Title");
            list.AddItem(todoItem);
        }

        [Test]
        public void ShouldBeAbleToCreateANewTodoList()
        {
            InitialiseTodoList();
            Assert.IsNotNull(list);
        }

        [Test]
        public void ShouldBeAbleToAddANewItem()
        {
            InitialiseTodoList();
            AddTodoItem();

            Assert.AreEqual(todoItem, list.Items[0]);
        }

        [Test]
        public void ListShouldHaveATitle()
        {
            InitialiseTodoList();

            Assert.AreEqual(ListTitle, list.Title);
        }

        [Test]
        public void ShouldMarkAsInProgress()
        {
            InitialiseTodoList();
            AddTodoItem();

            list.Items[0].SetInProgress();

            Assert.IsTrue(list.Items[0].InProgress);
        }

        [Test]
        public void ShouldMarkAsCompleted()
        {
            InitialiseTodoList();
            AddTodoItem();

            list.Items[0].SetCompleted();

            Assert.IsTrue(list.Items[0].Completed);
        }

        [Test]
        public void InProgressItemShouldNotBeInprogressAfterCompletion()
        {
            InitialiseTodoList();
            AddTodoItem();

            list.Items[0].SetInProgress();
            list.Items[0].SetCompleted();

            Assert.IsFalse(list.Items[0].InProgress);
        }

        [Test]
        public void CompletedItemSetToInprogressShouldNotBeCompleted()
        {
            InitialiseTodoList();
            AddTodoItem();

            list.Items[0].SetCompleted();
            list.Items[0].SetInProgress();

            Assert.IsFalse(list.Items[0].Completed);
        }
    }
}
