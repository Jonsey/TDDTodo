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

            Scenarios = new List<Scenario>();
        } 

        #endregion

        #region Properties

        public string Title { get; private set; }
        public string Detail { get; private set; }

        public List<Scenario> Scenarios { get; private set; }

        #endregion

        #region Public Methods

        public void AddScenario(Scenario Scenario)
        {
            this.Scenarios.Add(Scenario);
        } 

        #endregion       
    }
}
