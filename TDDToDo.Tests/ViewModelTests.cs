using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using NUnit.Framework;
using TDDToDo.Models;
using TDDToDo.ViewModels;

namespace TDDToDo.Tests
{
    // TODO Set status on data load
    // Set Item complete
    // Set Item in progress
    // TODO Add categories such as Idea, Must, Should, Candy
    // TODO Story points?
    // TODO Current development speed to predict completion time
    // TODO Dynamic and acurate estimated completion time of feature
    // TODO Create test stub from title (also allows quick navigation to test)
    // Should be able to add a new item when a list has been selected
    // System crashes when selected list is null
    // Selecting list should update can do add item
    // Display list item in human readable form
    // TODO implement ToString?
    // Expose SelectedListsItems as a property
    // Add items to the lists
    // Adding items when no list is selected
    // Select list for editing/viewing
    // Show list items when list is selected
    // Add item to selected list
    // create multiple lists
    // TODO duplicate list categories
    // Add new list

    [TestFixture]
    public class ViewModelTests
    {

        const string ItemTitle = "Get this test to pass";

        bool canExecuteChanged;
        string changedProperty;
        string DataFolder = System.Configuration.ConfigurationSettings.AppSettings["DataFolder"];
        string DataFileLocation = System.Configuration.ConfigurationSettings.AppSettings["PathToDataFile"];

        #region Setup Teardown
        [SetUp]
        public void BeforeEach()
        {
            if (Directory.Exists(DataFolder))
                Directory.Delete(DataFolder, true);
        }

        [TearDown]
        public void AfterEach()
        {
            if (Directory.Exists(DataFolder))
                Directory.Delete(DataFolder, true);
        } 
        #endregion

        [Test]
        public void CanCreateMultipleLists()
        {
            var viewModel = new TodoListViewModel();
            viewModel.NewListCommand.Execute("List 1");
            viewModel.NewListCommand.Execute("List 2"); 

            Assert.AreEqual(2, viewModel.TodoLists.Count);
        }

        [Test]
        public void ShouldNotBeAbleToAddItemsIfNoListSelected()
        {
            var viewModel = new TodoListViewModel();
            viewModel.NewListCommand.Execute("List 1");

            Assert.IsFalse(viewModel.AddItemCommand.CanExecute(ItemTitle));
        }

        [Test]
        public void ShouldBeAbleToAddANewItemIfAListHasBeenSelected()
        {
            var viewModel = CreateAndSelectOneList();

            try
            {
                viewModel.AddItemCommand.Execute(ItemTitle);
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void ShouldAddNewItemsToTheSelectedList()
        {
            var viewModel = new TodoListViewModel();
            viewModel.NewListCommand.Execute("List 1");
            viewModel.NewListCommand.Execute("List 2");

            viewModel.SelectedList = viewModel.TodoLists[1];

            Assert.IsTrue(viewModel.AddItemCommand.CanExecute(ItemTitle), "Cannot add item");

            viewModel.AddItemCommand.Execute(ItemTitle);

            Assert.AreEqual(ItemTitle, viewModel.TodoLists[1].Items[0].Title);
        }

        [Test]
        public void ShouldExposeSelectedListsItems()
        {
            var viewModel = new TodoListViewModel();
            viewModel.NewListCommand.Execute("List 1");
            viewModel.NewListCommand.Execute("List 2");

            viewModel.SelectedList = viewModel.TodoLists[1];
            viewModel.AddItemCommand.Execute(ItemTitle);

            Assert.AreEqual(ItemTitle, viewModel.SelectedListsItems[0].Title);
        }

        [Test]
        public void ItemsListShouldRelateToSelectedList()
        {
            var viewModel = new TodoListViewModel();
            viewModel.NewListCommand.Execute("List 1");
            viewModel.NewListCommand.Execute("List 2");

            viewModel.SelectedList = viewModel.TodoLists[1];
            viewModel.AddItemCommand.Execute("Item 1");

            viewModel.SelectedList = viewModel.TodoLists[0];
            viewModel.AddItemCommand.Execute("Item 0");

            viewModel.SelectedList = viewModel.TodoLists[1];

            Assert.AreEqual("Item 1", viewModel.SelectedList.Items[0].Title);
        }

        [Test] 
        public void ShouldNotFailWhenNoListHasBeenSelected()
        {
            var viewModel = new TodoListViewModel();

            Assert.IsNull(viewModel.SelectedListsItems);
        }

        [Test]
        public void ShouldAllowNewItemsToBeAddedWhenListHasBeenSelected()
        {
            CreateAndSelectOneList();
            Assert.IsTrue(canExecuteChanged);
        }

        [Test]
        public void ShouldRefreshItemsWhenAListIsSelected()
        {
            CreateAndSelectOneList();

            AssertTodoItemsRefreshed();
        }

        [Test]
        public void ShouldRefreshItemsWhenAnItemIsAdded()
        {
            var viewModel = CreateAndSelectOneList();
            changedProperty = string.Empty;

            viewModel.AddItemCommand.Execute(ItemTitle);

            AssertTodoItemsRefreshed();
        }

        [Test]
        public void ShouldBeAbleToSetItemsInProgress()
        {
            var viewModel = CreateAndSelectOneList();
            viewModel.AddItemCommand.Execute(ItemTitle);

            changedProperty = string.Empty;
            viewModel.SetItemInProgressCommand.Execute(viewModel.TodoLists[0].Items[0]);

            Assert.IsTrue(viewModel.TodoLists[0].Items[0].InProgress);
            AssertTodoItemsRefreshed();
        }

        [Test]
        public void ShouldBeAbleToSetItemsAsComplete()
        {
            var viewModel = CreateAndSelectOneList();
            viewModel.AddItemCommand.Execute(ItemTitle);

            changedProperty = string.Empty;
            
            viewModel.SetItemCompletedCommand.Execute(viewModel.TodoLists[0].Items[0]);

            Assert.IsTrue(viewModel.TodoLists[0].Items[0].Completed);
            AssertTodoItemsRefreshed();
        }
        
        [Test]
        public void CompletedItemShouldNotBeInProgress()
        {
            var viewModel = CreateAndSelectOneList();
            viewModel.AddItemCommand.Execute(ItemTitle);

            changedProperty = string.Empty;
            viewModel.SetItemInProgressCommand.Execute(viewModel.TodoLists[0].Items[0]);
            viewModel.SetItemCompletedCommand.Execute(viewModel.TodoLists[0].Items[0]);

            Assert.IsFalse(viewModel.TodoLists[0].Items[0].InProgress);
            AssertTodoItemsRefreshed(); 
        }

        [Test]
        public void ShouldSerialiseToBinary()
        {
            Directory.CreateDirectory(System.Configuration.ConfigurationSettings.AppSettings["DataFolder"]);

            var viewModel = CreateAndSelectOneList();
            viewModel.AddItemCommand.Execute(ItemTitle);

            viewModel.SaveCommand.Execute();
            Assert.IsTrue(File.Exists(DataFileLocation));
        }

        [Test]
        public void ShouldDeserialiseBinaryData()
        {
            Directory.CreateDirectory(System.Configuration.ConfigurationSettings.AppSettings["DataFolder"]);

            var viewModel = CreateAndSelectOneList();
            viewModel.AddItemCommand.Execute(ItemTitle);

            viewModel.SaveCommand.Execute();
            Assert.IsTrue(File.Exists(DataFileLocation));

            object result;
            using (var fs = new FileStream(DataFileLocation, FileMode.Open))
            {
                var formater = new BinaryFormatter();
                result = formater.Deserialize(fs);
            }

            Assert.IsInstanceOf(typeof(ObservableCollection<TodoList>),  result);
        }

        [Test]
        public void ShouldSaveWhenNewListsAreCreated()
        {
            Directory.CreateDirectory(System.Configuration.ConfigurationSettings.AppSettings["DataFolder"]);

            File.Delete(DataFileLocation);
            CreateAndSelectOneList();
            
            Assert.IsTrue(File.Exists(DataFileLocation));
        }

        [Test]
        public void ShouldSaveWhenNewItemIsAdded()
        {
            Directory.CreateDirectory(System.Configuration.ConfigurationSettings.AppSettings["DataFolder"]);

            var viewModel = CreateAndSelectOneList();
            File.Delete(DataFileLocation);
            viewModel.AddItemCommand.Execute(ItemTitle);

            Assert.IsTrue(File.Exists(DataFileLocation));
        }

        [Test]
        public void ShouldLoadSavedDataOnStartup()
        {
            var viewModel1 = CreateAndSelectOneList();

            var viewModel2 = new TodoListViewModel();
            Assert.AreEqual(viewModel1.TodoLists[0].Title, viewModel2.TodoLists[0].Title);
        }

        #region Private Methods

        void AssertTodoItemsRefreshed()
        {
            Assert.AreEqual("SelectedListsItems", changedProperty);
        }

        TodoListViewModel CreateAndSelectOneList()
        {
            var viewModel = new TodoListViewModel();
            viewModel.AddItemCommand.CanExecuteChanged += MarkCanExecuteChanged;
            viewModel.PropertyChanged += MarkPropertyChanged;

            viewModel.NewListCommand.Execute("List 1");

            viewModel.SelectedList = viewModel.TodoLists[0];

            return viewModel;
        }

        void MarkPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            changedProperty = e.PropertyName;
        }

        void MarkCanExecuteChanged(object sender, EventArgs e)
        {
            canExecuteChanged = true;
        } 

        #endregion
    }
}
