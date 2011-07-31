using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using TDDToDo.Models;

namespace TDDToDo.ViewModels
{
    public class FeaturesViewModel : NotificationObject
    {
        const string SelectedFeaturesScenariosPropertyName = "SelectedFeaturesScenarios";

        #region Fields

        readonly string dataFolder = System.Configuration.ConfigurationSettings.AppSettings["DataFolder"];
        readonly string pathToDataFile = System.Configuration.ConfigurationSettings.AppSettings["PathToDataFile"];
        Feature selectedFeature;

        #region Commands

        DelegateCommand newFeatureCommand;
        DelegateCommand<string> addScenarioCommand;
        DelegateCommand<Scenario> setItemInProgressCommand;
        DelegateCommand<Scenario> setItemCompletedCommand;
        DelegateCommand saveCommand;
        
        #endregion 

        #endregion

        #region Constructors

        public FeaturesViewModel()
        {
            Features = new ObservableCollection<Feature>();
            LoadData();
        }

        #endregion

        #region Properties

        public ObservableCollection<Feature> Features { get; set; }

        public string Title { get; set; }

        public string Detail { get; set; }

        public ObservableCollection<Scenario> SelectedFeaturesScenarios
        {
            get
            {
                return selectedFeature != null ? new ObservableCollection<Scenario>(selectedFeature.Scenarios) : null;
            }
        }

        public Feature SelectedFeature
        {
            get { return selectedFeature; }
            set
            {
                selectedFeature = value;

                AddScenarioCommand.RaiseCanExecuteChanged();
                RefreshScenarios();
            }
        }

        #region Commands

        public DelegateCommand NewFeatureCommand
        {
            get { return newFeatureCommand ?? (newFeatureCommand = new DelegateCommand(CreateNewFeature)); }
        }

        public DelegateCommand<string> AddScenarioCommand
        {
            get { return addScenarioCommand ?? (addScenarioCommand = new DelegateCommand<string>(AddScenario, CanAddScenario)); }
        }

        public DelegateCommand<Scenario> SetItemInProgressCommand
        {
            get { return setItemInProgressCommand ?? (setItemInProgressCommand = new DelegateCommand<Scenario>(SetItemInProgress)); }
        }

        public DelegateCommand<Scenario> SetItemCompletedCommand
        {
            get { return setItemCompletedCommand ?? (setItemCompletedCommand = new DelegateCommand<Scenario>(SetItemCompleted)); }
        }

        public DelegateCommand SaveCommand
        {
            get { return saveCommand ?? (saveCommand = new DelegateCommand(Save)); }
        }

        #endregion 

        #endregion

        #region Private Methods

        void LoadData()
        {
            if (!File.Exists(pathToDataFile)) return;

            using (var fs = new FileStream(pathToDataFile, FileMode.Open))
            {
                var formater = new BinaryFormatter();
                Features = formater.Deserialize(fs) as ObservableCollection<Feature>;
            }
        }

        void CreateNewFeature()
        {
            Features.Add(new Feature(Title, Detail));
            Save();
        }

        bool CanAddScenario(string title)
        {
            return SelectedFeature != null;         
        }

        void AddScenario(string title)
        {
            var Scenario = new Scenario(title);
            SelectedFeature.AddScenario(Scenario);

            RefreshScenarios();
            Save();
        }

        void SetItemInProgress(Scenario Scenario)
        {
            Scenario.SetInProgress();
            RefreshScenarios();
        }

        void SetItemCompleted(Scenario Scenario)
        {
            Scenario.SetCompleted();
            RefreshScenarios();
        }

        void Save()
        {
            Directory.CreateDirectory(dataFolder);

            using (Stream fs = new FileStream(pathToDataFile, FileMode.Create))
            {
                var formater = new BinaryFormatter();
                formater.Serialize(fs, Features);
            }
        }

        void RefreshScenarios()
        {
            RaisePropertyChanged(SelectedFeaturesScenariosPropertyName);
        }

        #endregion
    }
}
