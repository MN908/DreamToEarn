//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DreamToEarn.DbModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class UsersWithdraw
    {
        public int drawID { get; set; }
        public Nullable<int> UserId { get; set; }
        public string UserName { get; set; }
        public string BankAccount { get; set; }
        public string BankTitle { get; set; }
        public string EasyPaisa { get; set; }
        public string JazzCash { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public Nullable<decimal> totalDraw { get; set; }
        public Nullable<decimal> totalEarningOnDraw { get; set; }
        public decimal Deductions { get; set; }
        public Nullable<System.DateTime> drawDate { get; set; }
        public Nullable<bool> drawStatus { get; set; }
        public Nullable<bool> isAdminApprove { get; set; }
        public bool drawSentByExchanger { get; set; }
        public string DrawType { get; set; }
        public Nullable<System.DateTime> DrawSentDate { get; set; }
    }
}
