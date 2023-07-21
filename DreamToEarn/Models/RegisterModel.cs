using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DreamToEarn.Models
{
    public class RegisterModel
    {        
        public int userId { get; set; }
        public string UserName { get; set; }        
        public string FirstName { get; set; }        
        public string LastName { get; set; }        
        public string EmailAddress { get; set; }      
        public string Password { get; set; }
        public int RefUserId { get; set; }
        public int RefUserName { get; set; }
        public string firstLoginToken { get; set; }
        public bool firstLogin { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string BankAccount { get; set; }
        public string BankTitle { get; set; }
        public string JazzCash { get; set; }
        public string EasyPaisa { get; set; }

    }
}