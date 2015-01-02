namespace TVGuideServices.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class TVSeriesEntry
    {
        public TVSeriesEntry(string name, string time, string day)
        {
            this.Name = name;
            this.Time = time;
            this.Day = day;
        }

        public string Name { get;private set; }
        public string Time { get;private set; }
        public string Day { get;private set; }
    }
}