namespace TVGuideServices.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class TVChannelScheduleForSeries
    {
        public TVChannelScheduleForSeries(string nameOfTV, List<TVSeriesEntry> series)
        {
            this.NameOfTV = nameOfTV;
            this.Series = series;
        }

        public string NameOfTV { get; set; }

        public List<TVSeriesEntry> Series { get; set; }
    }
}