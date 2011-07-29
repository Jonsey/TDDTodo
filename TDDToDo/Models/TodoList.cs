using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TDDToDo.Models
{
    [Serializable]
    public class TodoList
    {
        #region Constructors

        public TodoList(string title)
        {
            Title = title;
            Items = new List<TodoItem>();
        } 

        #endregion

        #region Properties

        public string Title { get; private set; }
        public List<TodoItem> Items { get; private set; }

        #endregion

        #region Public Methods

        public void AddItem(TodoItem todoItem)
        {
            this.Items.Add(todoItem);
        } 

        #endregion       
    }
}
