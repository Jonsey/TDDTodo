using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using NUnit.Framework;
using TDDToDo.Models;
using TDDToDo.ViewModels;

namespace TDDToDo.Tests
{
    public class AppDataFileTests : FeatureViewModelTestsBase
    {
        [Test]
        public void ShouldSerialiseToBinary()
        {
            Directory.CreateDirectory(System.Configuration.ConfigurationSettings.AppSettings["DataFolder"]);

            var viewModel = CreateAndSelectOneFeature();
            viewModel.AddScenarioCommand.Execute(FeatureTitle);

            viewModel.SaveCommand.Execute();
            Assert.IsTrue(File.Exists(DataFileLocation));
        }

        [Test]
        public void ShouldDeserialiseBinaryData()
        {
            Directory.CreateDirectory(System.Configuration.ConfigurationSettings.AppSettings["DataFolder"]);

            var viewModel = CreateAndSelectOneFeature();
            viewModel.AddScenarioCommand.Execute(FeatureTitle);

            viewModel.SaveCommand.Execute();
            Assert.IsTrue(File.Exists(DataFileLocation));

            object result;
            using (var fs = new FileStream(DataFileLocation, FileMode.Open))
            {
                var formater = new BinaryFormatter();
                result = formater.Deserialize(fs);
            }

            Assert.IsInstanceOf(typeof(ObservableCollection<Feature>), result);
        }

        [Test]
        public void ShouldSaveWhenNewFeaturesAreCreated()
        {
            Directory.CreateDirectory(System.Configuration.ConfigurationSettings.AppSettings["DataFolder"]);

            File.Delete(DataFileLocation);
            CreateAndSelectOneFeature();

            Assert.IsTrue(File.Exists(DataFileLocation));
        }

        [Test]
        public void ShouldSaveWhenNewScenarioIsAdded()
        {
            Directory.CreateDirectory(System.Configuration.ConfigurationSettings.AppSettings["DataFolder"]);

            var viewModel = CreateAndSelectOneFeature();
            File.Delete(DataFileLocation);
            viewModel.AddScenarioCommand.Execute(FeatureTitle);

            Assert.IsTrue(File.Exists(DataFileLocation));
        }

        [Test]
        public void ShouldLoadSavedDataOnStartup()
        {
            var viewModel1 = CreateAndSelectOneFeature();

            var viewModel2 = new FeaturesViewModel();
            Assert.AreEqual(viewModel1.Features[0].Title, viewModel2.Features[0].Title);
        }
    }
}
