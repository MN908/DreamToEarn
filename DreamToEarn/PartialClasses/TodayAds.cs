using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamToEarn.PartialClasses
{
    public class TodayAds
    {
        public int Id { get; set; }
        public string adURL { get; set; }
        public DateTime forDate { get; set; }
        public bool IsViewed { get; set; }
        public int UserId { get; set; }

    }
}