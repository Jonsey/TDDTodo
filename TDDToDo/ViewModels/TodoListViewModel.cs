using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using TDDToDo.Models;

namespace TDDToDo.ViewModels
{
    public class TodoListViewModel : NotificationObject
    {
        const string SelectedListsItemsPropertyName = "SelectedListsItems";

        #region Fields

        readonly string dataFolder = System.Configuration.ConfigurationSettings.AppSettings["DataFolder"];
        readonly string pathToDataFile = System.Configuration.ConfigurationSettings.AppSettings["PathToDataFile"];
        TodoList selectedList;

        #region Commands

        DelegateCommand<string> newListCommand;
        DelegateCommand<string> addItemCommand;
        DelegateCommand<TodoItem> setItemInProgressCommand;
        DelegateCommand<TodoItem> setItemCompletedCommand;
        DelegateCommand saveCommand;
        
        #endregion 

        #endregion

        #region Constructors

        public TodoListViewModel()
        {
            TodoLists = new ObservableCollection<TodoList>();
            LoadData();
        }

        #endregion

        #region Properties

        public ObservableCollection<TodoList> TodoLists { get; set; }

        public ObservableCollection<TodoItem> SelectedListsItems
        {
            get
            {
                return selectedList != null ? new ObservableCollection<TodoItem>(selectedList.Items) : null;
            }
        }

        public TodoList SelectedList
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

        public DelegateCommand<string> NewListCommand
        {
            get { return newListCommand ?? (newListCommand = new DelegateCommand<string>(CreateNewList)); }
        }

        public DelegateCommand<string> AddItemCommand
        {
            get { return addItemCommand ?? (addItemCommand = new DelegateCommand<string>(AddItem, CanAddItem)); }
        }

        public DelegateCommand<TodoItem> SetItemInProgressCommand
        {
            get { return setItemInProgressCommand ?? (setItemInProgressCommand = new DelegateCommand<TodoItem>(SetItemInProgress)); }
        }

        public DelegateCommand<TodoItem> SetItemCompletedCommand
        {
            get { return setItemCompletedCommand ?? (setItemCompletedCommand = new DelegateCommand<TodoItem>(SetItemCompleted)); }
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
                TodoLists = formater.Deserialize(fs) as ObservableCollection<TodoList>;
            }
        }

        void CreateNewList(string category)
        {
            TodoLists.Add(new TodoList(category));
            Save();
        }

        bool CanAddItem(string title)
        {
            return SelectedList != null;         
        }

        void AddItem(string title)
        {
            var item = new TodoItem(title);
            SelectedList.AddItem(item);

            RefreshTodoItems();
            Save();
        }

        void SetItemInProgress(TodoItem item)
        {
            item.SetInProgress();
            RefreshTodoItems();
        }

        void SetItemCompleted(TodoItem item)
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
                formater.Serialize(fs, TodoLists);
            }
        }

        void RefreshTodoItems()
        {
            RaisePropertyChanged(SelectedListsItemsPropertyName);
        }

        #endregion
    }
}
