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
    
    public partial class CommReferrer
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Nullable<int> RefUserId { get; set; }
        public Nullable<int> Role { get; set; }
        public string firstLoginToken { get; set; }
        public Nullable<bool> firstLogin { get; set; }
        public string TransactionToken { get; set; }
    }
}
