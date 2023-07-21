using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamToEarn.Models
{
    public class WithdrawModel
    {        
        public int drawId { get; set; }
        public int UserId { get; set; }
        public double totalDraw { get; set; }
        public double totalEarningOnDraw { get; set; }
        public DateTime drawDate { get; set; }
        public string OTPTokens { get; set; }
        public bool drawStatus { get; set; }
        public bool isAdminApprove { get; set; }
        public decimal deductions { get; set; }
        public double earnings { get; set; }
        public double commissions { get; set; }
        public string drawType { get; set; }
    }
}