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
        const string SelectedListsItemsPropertyName = "SelectedListsItems";

        #region Fields

        readonly string dataFolder = System.Configuration.ConfigurationSettings.AppSettings["DataFolder"];
        readonly string pathToDataFile = System.Configuration.ConfigurationSettings.AppSettings["PathToDataFile"];
        Feature selectedList;

        #region Commands

        DelegateCommand newListCommand;
        DelegateCommand<string> addItemCommand;
        DelegateCommand<Specification> setItemInProgressCommand;
        DelegateCommand<Specification> setItemCompletedCommand;
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

        public ObservableCollection<Specification> SelectedListsItems
        {
            get
            {
                return selectedList != null ? new ObservableCollection<Specification>(selectedList.Specifications) : null;
            }
        }

        public Feature SelectedList
        {
            get { return selectedList; }
            set
            {
                selectedList = value;

                AddItemCommand.RaiseCanExecuteChanged();
                RefreshTodoItems();
            }
        }

        #region Commands

        public DelegateCommand NewListCommand
        {
            get { return newListCommand ?? (newListCommand = new DelegateCommand(CreateNewList)); }
        }

        public DelegateCommand<string> AddItemCommand
        {
            get { return addItemCommand ?? (addItemCommand = new DelegateCommand<string>(AddItem, CanAddItem)); }
        }

        public DelegateCommand<Specification> SetItemInProgressCommand
        {
            get { return setItemInProgressCommand ?? (setItemInProgressCommand = new DelegateCommand<Specification>(SetItemInProgress)); }
        }

        public DelegateCommand<Specification> SetItemCompletedCommand
        {
            get { return setItemCompletedCommand ?? (setItemCompletedCommand = new DelegateCommand<Specification>(SetItemCompleted)); }
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

        void CreateNewList()
        {
            Features.Add(new Feature(Title, Detail));
            Save();
        }

        bool CanAddItem(string title)
        {
            return SelectedList != null;         
        }

        void AddItem(string title)
        {
            var item = new Specification(title);
            SelectedList.AddItem(item);

            RefreshTodoItems();
            Save();
        }

        void SetItemInProgress(Specification item)
        {
            item.SetInProgress();
            RefreshTodoItems();
        }

        void SetItemCompleted(Specification item)
        {
            item.SetCompleted();
            RefreshTodoItems();
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

        void RefreshTodoItems()
        {
            RaisePropertyChanged(SelectedListsItemsPropertyName);
        }

        #endregion
    }
}
