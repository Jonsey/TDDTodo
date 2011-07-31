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
    // TODO Add detail to feature
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
    public class FeatureViewModelTests
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
            var viewModel = new FeaturesViewModel();
            viewModel.Title = "List 1";
            viewModel.NewListCommand.Execute();
            viewModel.Title = "List 2";
            viewModel.NewListCommand.Execute(); 

            Assert.AreEqual(2, viewModel.Features.Count);
        }

        [Test]
        public void ShouldNotBeAbleToAddItemsIfNoListSelected()
        {
            var viewModel = new FeaturesViewModel();
            viewModel.Title = "List 1";
            viewModel.NewListCommand.Execute();

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
            var viewModel = new FeaturesViewModel();
            viewModel.Title = "List 1";
            viewModel.NewListCommand.Execute();
            viewModel.Title = "List 2";
            viewModel.NewListCommand.Execute(); 

            viewModel.SelectedList = viewModel.Features[1];

            Assert.IsTrue(viewModel.AddItemCommand.CanExecute(ItemTitle), "Cannot add item");

            viewModel.AddItemCommand.Execute(ItemTitle);

            Assert.AreEqual(ItemTitle, viewModel.Features[1].Specifications[0].Title);
        }

        [Test]
        public void ShouldExposeSelectedListsItems()
        {
            var viewModel = new FeaturesViewModel();
            viewModel.Title = "List 1";
            viewModel.NewListCommand.Execute();
            viewModel.Title = "List 2";
            viewModel.NewListCommand.Execute(); 

            viewModel.SelectedList = viewModel.Features[1];
            viewModel.AddItemCommand.Execute(ItemTitle);

            Assert.AreEqual(ItemTitle, viewModel.SelectedListsItems[0].Title);
        }

        [Test]
        public void ItemsListShouldRelateToSelectedList()
        {
            var viewModel = new FeaturesViewModel();
            viewModel.Title = "List 1";
            viewModel.NewListCommand.Execute();
            viewModel.Title = "List 2";
            viewModel.NewListCommand.Execute(); 

            viewModel.SelectedList = viewModel.Features[1];
            viewModel.AddItemCommand.Execute("Item 1");

            viewModel.SelectedList = viewModel.Features[0];
            viewModel.AddItemCommand.Execute("Item 0");

            viewModel.SelectedList = viewModel.Features[1];

            Assert.AreEqual("Item 1", viewModel.SelectedList.Specifications[0].Title);
        }

        [Test] 
        public void ShouldNotFailWhenNoListHasBeenSelected()
        {
            var viewModel = new FeaturesViewModel();

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
            viewModel.SetItemInProgressCommand.Execute(viewModel.Features[0].Specifications[0]);

            Assert.IsTrue(viewModel.Features[0].Specifications[0].InProgress);
            AssertTodoItemsRefreshed();
        }

        [Test]
        public void ShouldBeAbleToSetItemsAsComplete()
        {
            var viewModel = CreateAndSelectOneList();
            viewModel.AddItemCommand.Execute(ItemTitle);

            changedProperty = string.Empty;
            
            viewModel.SetItemCompletedCommand.Execute(viewModel.Features[0].Specifications[0]);

            Assert.IsTrue(viewModel.Features[0].Specifications[0].Completed);
            AssertTodoItemsRefreshed();
        }
        
        [Test]
        public void CompletedItemShouldNotBeInProgress()
        {
            var viewModel = CreateAndSelectOneList();
            viewModel.AddItemCommand.Execute(ItemTitle);

            changedProperty = string.Empty;
            viewModel.SetItemInProgressCommand.Execute(viewModel.Features[0].Specifications[0]);
            viewModel.SetItemCompletedCommand.Execute(viewModel.Features[0].Specifications[0]);

            Assert.IsFalse(viewModel.Features[0].Specifications[0].InProgress);
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

            Assert.IsInstanceOf(typeof(ObservableCollection<Feature>),  result);
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

            var viewModel2 = new FeaturesViewModel();
            Assert.AreEqual(viewModel1.Features[0].Title, viewModel2.Features[0].Title);
        }

        [Test]
        public void FeatureShouldHaveDetail()
        {
            var viewModel = CreateAndSelectOneList();

            Assert.IsNotNull(viewModel.Features[0].Detail);
        }

        #region Private Methods

        void AssertTodoItemsRefreshed()
        {
            Assert.AreEqual("SelectedListsItems", changedProperty);
        }

        FeaturesViewModel CreateAndSelectOneList()
        {
            var viewModel = new FeaturesViewModel();
            viewModel.AddItemCommand.CanExecuteChanged += MarkCanExecuteChanged;
            viewModel.PropertyChanged += MarkPropertyChanged;
            viewModel.Title = "List 1";
            viewModel.Detail = "In order the do something As a user I want to be able to do something";

            viewModel.NewListCommand.Execute();

            viewModel.SelectedList = viewModel.Features[0];

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
