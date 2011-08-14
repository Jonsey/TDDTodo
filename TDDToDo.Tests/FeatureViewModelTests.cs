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
    // Expose SelectedFeaturesScenarios as a property
    // Add items to the lists
    // Adding items when no list is selected
    // Select list for editing/viewing
    // Show list items when list is selected
    // Add item to selected list
    // create multiple lists
    // TODO duplicate list categories
    // Add new list

    [TestFixture]
    public class FeatureViewModelTests : FeatureViewModelTestsBase
    {
        #region Setup Teardown

        #endregion

        [Test]
        public void CanCreateMultipleFeatures()
        {
            var viewModel = new FeaturesViewModel();
            viewModel.Title = "Feature 1";
            viewModel.NewFeatureCommand.Execute();
            viewModel.Title = "Feature 2";
            viewModel.NewFeatureCommand.Execute(); 

            Assert.AreEqual(2, viewModel.Features.Count);
        }

        [Test]
        public void ShouldNotBeAbleToAddItemsIfNoFeatureSelected()
        {
            var viewModel = new FeaturesViewModel();
            viewModel.Title = "Feature 1";
            viewModel.NewFeatureCommand.Execute();

            Assert.IsFalse(viewModel.AddScenarioCommand.CanExecute(FeatureTitle));
        }

        [Test]
        public void ShouldBeAbleToAddANewItemIfAFeatureHasBeenSelected()
        {
            var viewModel = CreateAndSelectOneFeature();

            try
            {
                viewModel.AddScenarioCommand.Execute(FeatureTitle);
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void ShouldAddNewItemsToTheSelectedFeature()
        {
            var viewModel = new FeaturesViewModel();
            viewModel.Title = "Feature 1";
            viewModel.NewFeatureCommand.Execute();
            viewModel.Title = "Feature 2";
            viewModel.NewFeatureCommand.Execute(); 

            viewModel.SelectedFeature = viewModel.Features[1];

            Assert.IsTrue(viewModel.AddScenarioCommand.CanExecute(FeatureTitle), "Cannot add scenario.");

            viewModel.AddScenarioCommand.Execute(FeatureTitle);

            Assert.AreEqual(FeatureTitle, viewModel.Features[1].Scenarios[0].Title);
        }

        [Test]
        public void ShouldExposeSelectedFeaturesScenarios()
        {
            var viewModel = new FeaturesViewModel();
            viewModel.Title = "Feature 1";
            viewModel.NewFeatureCommand.Execute();
            viewModel.Title = "Feature 2";
            viewModel.NewFeatureCommand.Execute(); 

            viewModel.SelectedFeature = viewModel.Features[1];
            viewModel.AddScenarioCommand.Execute(FeatureTitle);

            Assert.AreEqual(FeatureTitle, viewModel.SelectedFeaturesScenarios[0].Title);
        }

        [Test]
        public void ItemsFeatureShouldRelateToSelectedFeature()
        {
            var viewModel = new FeaturesViewModel();
            viewModel.Title = "Feature 1";
            viewModel.NewFeatureCommand.Execute();
            viewModel.Title = "Feature 2";
            viewModel.NewFeatureCommand.Execute(); 

            viewModel.SelectedFeature = viewModel.Features[1];
            viewModel.AddScenarioCommand.Execute("Item 1");

            viewModel.SelectedFeature = viewModel.Features[0];
            viewModel.AddScenarioCommand.Execute("Item 0");

            viewModel.SelectedFeature = viewModel.Features[1];

            Assert.AreEqual("Item 1", viewModel.SelectedFeature.Scenarios[0].Title);
        }

        [Test] 
        public void ShouldNotFailWhenNoFeatureHasBeenSelected()
        {
            var viewModel = new FeaturesViewModel();

            Assert.IsNull(viewModel.SelectedFeaturesScenarios);
        }

        [Test]
        public void ShouldAllowNewItemsToBeAddedWhenFeatureHasBeenSelected()
        {
            CreateAndSelectOneFeature();
            Assert.IsTrue(canExecuteChanged);
        }

        [Test]
        public void ShouldRefreshItemsWhenAFeatureIsSelected()
        {
            CreateAndSelectOneFeature();

            AssertScenariosRefreshed();
        }

        [Test]
        public void ShouldRefreshItemsWhenAnItemIsAdded()
        {
            var viewModel = CreateAndSelectOneFeature();
            changedProperty = string.Empty;

            viewModel.AddScenarioCommand.Execute(FeatureTitle);

            AssertScenariosRefreshed();
        }

        [Test]
        public void ShouldBeAbleToSetItemsInProgress()
        {
            var viewModel = CreateAndSelectOneFeature();
            viewModel.AddScenarioCommand.Execute(FeatureTitle);

            changedProperty = string.Empty;
            viewModel.SetItemInProgressCommand.Execute(viewModel.Features[0].Scenarios[0]);

            Assert.IsTrue(viewModel.Features[0].Scenarios[0].InProgress);
            AssertScenariosRefreshed();
        }

        [Test]
        public void ShouldBeAbleToSetItemsAsComplete()
        {
            var viewModel = CreateAndSelectOneFeature();
            viewModel.AddScenarioCommand.Execute(FeatureTitle);

            changedProperty = string.Empty;
            
            viewModel.SetItemCompletedCommand.Execute(viewModel.Features[0].Scenarios[0]);

            Assert.IsTrue(viewModel.Features[0].Scenarios[0].Completed);
            AssertScenariosRefreshed();
        }
        
        [Test]
        public void CompletedItemShouldNotBeInProgress()
        {
            var viewModel = CreateAndSelectOneFeature();
            viewModel.AddScenarioCommand.Execute(FeatureTitle);

            changedProperty = string.Empty;
            viewModel.SetItemInProgressCommand.Execute(viewModel.Features[0].Scenarios[0]);
            viewModel.SetItemCompletedCommand.Execute(viewModel.Features[0].Scenarios[0]);

            Assert.IsFalse(viewModel.Features[0].Scenarios[0].InProgress);
            AssertScenariosRefreshed(); 
        }

        [Test]
        public void FeatureShouldHaveDetail()
        {
            var viewModel = CreateAndSelectOneFeature();

            Assert.IsNotNull(viewModel.Features[0].Detail);
        }

        #region Private Methods

        void AssertScenariosRefreshed()
        {
            Assert.AreEqual("SelectedFeaturesScenarios", changedProperty);
        }

        #endregion
    }
}
