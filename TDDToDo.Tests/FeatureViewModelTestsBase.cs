using System;
using System.ComponentModel;
using System.IO;
using NUnit.Framework;
using TDDToDo.ViewModels;

namespace TDDToDo.Tests
{
    public class FeatureViewModelTestsBase
    {
        protected bool canExecuteChanged;
        protected string changedProperty;
        protected string DataFolder = System.Configuration.ConfigurationSettings.AppSettings["DataFolder"];
        protected string DataFileLocation = System.Configuration.ConfigurationSettings.AppSettings["PathToDataFile"];
        protected const string FeatureTitle = "Get this test to pass";

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

        protected FeaturesViewModel CreateAndSelectOneFeature()
        {
            var viewModel = new FeaturesViewModel();
            viewModel.AddScenarioCommand.CanExecuteChanged += MarkCanExecuteChanged;
            viewModel.PropertyChanged += MarkPropertyChanged;
            viewModel.Title = "Feature 1";
            viewModel.Detail = "In order the do something As a user I want to be able to do something";

            viewModel.NewFeatureCommand.Execute();

            viewModel.SelectedFeature = viewModel.Features[0];

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
    }
}