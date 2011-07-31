using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TDDToDo.Models
{
    [Serializable]
    public class Feature
    {
        #region Constructors

        public Feature(string title, string detail)
        {
            Title = title;
            Detail = detail;

            Specifications = new List<Specification>();
        } 

        #endregion

        #region Properties

        public string Title { get; private set; }
        public string Detail { get; private set; }

        public List<Specification> Specifications { get; private set; }

        #endregion

        #region Public Methods

        public void AddItem(Specification specification)
        {
            this.Specifications.Add(specification);
        } 

        #endregion       
    }
}
