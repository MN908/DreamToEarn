using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DreamToEarn.Models
{
    public class LoginModel
    {        
        public string EmailAddress { get; set; }        
        public string Password { get; set; }      
        public bool Remember { get; set; }
    }
}