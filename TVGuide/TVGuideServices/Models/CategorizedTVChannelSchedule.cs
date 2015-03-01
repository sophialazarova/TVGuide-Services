namespace TVGuideServices.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class CategorizedTVChannelSchedule
    {
        public CategorizedTVChannelSchedule(string nameOfTV, List<CategorizedScheduleEntry> series)
        {
            this.NameOfTV = nameOfTV;
            this.Series = series;
        }

        public string NameOfTV { get; set; }

        public List<CategorizedScheduleEntry> Series { get; set; }
    }
}