using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDDToDo.Models
{
    [Serializable]
    public class Step
    {
        public string Title { get; private set; }

        public Step(string title)
        {
            Title = title;
        }
    }
}
