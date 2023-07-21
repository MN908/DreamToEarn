using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamToEarn.Models
{
    public class InvestmentPlanModel
    {
        public int planId { get; set; }
        public string plan { get; set; }
        public string planType { get; set; }
        public DateTime planAddDate { get; set; }
        public DateTime planActiveDate { get; set; }
        public string planStatus { get; set; }
        public int userId { get; set; }
        public int totalEarnings { get; set; }
        public int InvestmentPercentage { get; set; }
    }
}