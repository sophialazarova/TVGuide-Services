using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TVGuideServices.Models
{
    public class TVProgramEntry
    {
        public TVProgramEntry(string name, string time)
        {
            this.Name = name;
            this.Time = time;
        }

        public string Time { get;private set; }

        public string Name { get;private set; }
    }
}