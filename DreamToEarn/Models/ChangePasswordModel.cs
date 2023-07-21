using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamToEarn.Models
{
    public class ChangePasswordModel
    {
        public int userId { get; set; }
        public string newPassword { get; set; }
        public string oldPassword { get; set; }
    }
}