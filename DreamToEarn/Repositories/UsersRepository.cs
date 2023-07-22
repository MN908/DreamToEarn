using DreamToEarn.DbModel;
using DreamToEarn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DreamToEarn.Helper;

namespace DreamToEarn.Repositories
{
    public class UsersRepository
    {
        public static bool Login(string Email, string Pass)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {

                var obj = context.Users.FirstOrDefault(x => x.Email.Equals(Email) && x.Password.Equals(Pass, StringComparison.InvariantCulture));
                if (obj != null)
                    return true;

                return false;
            }
        }
        public static bool VarifyToken(string Token, int userId)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.Users.FirstOrDefault(x => x.firstLoginToken.Equals(Token) && x.UserId == userId);
                if (obj != null)
                    return true;

                return false;
            }
        }

        public static bool userHaveReference(int userId)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.Users.FirstOrDefault(x => x.UserId == userId && x.RefUserId > 0);
                if (obj != null)
                    return true;

                return false;
            }
        }

        public static User UpdateUserFirstLogin(int userId)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.Users.FirstOrDefault(x => x.UserId == userId);
                if (obj != null)
                {
                    obj.firstLogin = false;
                }
                context.SaveChanges();

                return obj;

            }
        }

        public static User UpdateTotalCommissionsForAdmin(decimal? commission)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.Users.FirstOrDefault(x => x.Role == 1 && x.SuperAdmin == true);
                if (obj != null)
                {
                    obj.TotalCommissions += commission;
                }
                context.SaveChanges();

                return obj;

            }
        }

        public static User UpdateTotalCommissions(int userId, decimal? totalCommission)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.Users.FirstOrDefault(x => x.UserId == userId);
                if (obj != null)
                {
                    obj.TotalCommissions = totalCommission;
                }
                context.SaveChanges();

                return obj;

            }
        }

        public static User GetUser(string Email)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.Users.FirstOrDefault(x => x.Email.Equals(Email));
                if (obj != null)
                    return obj;

                return obj;
            }
        }
        public static User GetUser(int userId)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {

                var obj = context.Users.FirstOrDefault(x => x.UserId == userId);
                if (obj != null)
                    return obj;

                return obj;
            }
        }
        public static User RegisterUser(RegisterModel user)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {

                var obj = new User();

                obj.UserName = user.UserName;
                obj.FirstName = user.FirstName;
                obj.Email = user.EmailAddress;
                obj.LastName = user.LastName;
                obj.Password = user.Password.Encrypt();
                obj.RefUserId = user.RefUserId;
                obj.Role = 2;
                obj.firstLogin = true;
                obj.firstLoginToken = user.firstLoginToken;
                context.Users.Add(obj);

                context.SaveChanges();

                return obj;

            }
        }

        public static User UpdateUserDetail(RegisterModel user)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.Users.FirstOrDefault(x => x.UserId == user.userId);
                if (obj != null)
                {
                    obj.FirstName = user.FirstName;                    
                    obj.LastName = user.LastName;
                    obj.UserAddress = user.Address;
                    obj.Mobile = user.Mobile;
                    obj.BankAccount = user.BankAccount;
                    obj.BankTitle = user.BankTitle;
                    obj.JazzCash = user.JazzCash;
                    obj.EasyPaisa = user.EasyPaisa;
                }
                context.SaveChanges();

                return obj;

            }
        }

        public static User SetExchangerUser(int userId)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.Users.FirstOrDefault(x => x.UserId == userId);
                if (obj != null)
                {
                    obj.Role = 3;
                }
                context.SaveChanges();

                return obj;

            }
        }

        public static bool isExchangerAlreadySet()
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.Users.FirstOrDefault(x => x.Role == 3);
                if (obj != null)
                    return true;

                return false;
            }
        }
        public static User RemoveExchangerUser(int userId)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.Users.FirstOrDefault(x => x.UserId == userId);
                if (obj != null)
                {
                    obj.Role = 2;
                }
                context.SaveChanges();

                return obj;

            }
        }

        public static bool oldPasswordMatched(ChangePasswordModel pass)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                string password = pass.oldPassword.Encrypt();
                var obj = context.Users.FirstOrDefault(x => x.UserId == pass.userId && x.Password.Equals(password, StringComparison.InvariantCulture));
                if (obj != null)
                    return true;

                return false;

            }
        }

        public static User ChangePassword(ChangePasswordModel pass)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                string password = pass.oldPassword.Encrypt();
                var obj = context.Users.FirstOrDefault(x => x.UserId == pass.userId && x.Password.Equals(password, StringComparison.InvariantCulture));

                if (obj != null)
                {
                    obj.Password = pass.newPassword.Encrypt();
                }
                context.SaveChanges();

                return obj;

            }
        }

        public static bool UserNameAlreadyExist(string UserName)
        {
            using (var context = new DreamToEarnEntities1())
            {
                var obj = context.Users.FirstOrDefault(x => x.UserName == UserName);
                if (obj != null)
                    return true;
                else
                    return false;
            }

        }

        public static bool EmailAlreadyExist(string Email)
        {
            using (var context = new DreamToEarnEntities1())
            {
                var obj = context.Users.FirstOrDefault(x => x.Email.Equals(Email));
                if (obj != null)
                    return true;
                else
                    return false;
            }

        }

        public static int GetIDAgainstUserName(string UserName)
        {
            using (var context = new DreamToEarnEntities1())
            {
                var obj = context.Users.FirstOrDefault(x => x.UserName == UserName);
                if (obj != null)
                    return obj.UserId;
                else
                    return 0;
            }

        }
        public static List<Team> GetTeam(int userId)
        {
            using (var context = new DreamToEarnEntities1())
            {
                return context.Teams.Where(x => x.RefUserId == userId).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            }

        }
        public static List<User> GetAllTeam()
        {
            using (var context = new DreamToEarnEntities1())
            {
                return context.Users.Where(x => x.Email != "kumail.mehdi001@gmail.com" && x.Role != 1 && x.Role != 3).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            }

        }

        public static List<User> GetAllExchangers()
        {
            using (var context = new DreamToEarnEntities1())
            {
                return context.Users.Where(x => x.Email != "kumail.mehdi001@gmail.com" && x.Role == 3 && x.Role != 1).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            }

        }

        public static User GetAllExchanger()
        {
            using (var context = new DreamToEarnEntities1())
            {
                return context.Users.FirstOrDefault(x => x.Email != "kumail.mehdi001@gmail.com" && x.Role == 3 && x.Role != 1);
            }

        }

        public static List<Team> GetAllTeam(int RefUserId)
        {
            using (var context = new DreamToEarnEntities1())
            {
                return context.Teams.Where(x => x.RefUserId == RefUserId && x.RefUserId != 0 && x.Email != "kumail.mehdi001@gmail.com").OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            }

        }

        public static int TotalTeamMembers(int userId)
        {
            using (var context = new DreamToEarnEntities1())
            {
                var obj = context.Teams.Where(x => x.RefUserId == userId).OrderBy(x => x.FirstName).ThenBy(x => x.LastName);
                if (obj != null)
                    return obj.Count();

                return 0;
            }

        }
        public static decimal TotalInvestmentPlan(int userId)
        {
            using (var context = new DreamToEarnEntities1())
            {
                var obj = context.InvestmentPlans.FirstOrDefault(x => x.UserId == userId && x.IsActive == true);
                if (obj != null)
                    return obj.plan.ToDecimal();

                return 0;
            }

        }

        public static int? GetRefUserId(int userId)
        {
            using (var context = new DreamToEarnEntities1())
            {
                var obj = context.Users.FirstOrDefault(x => x.UserId == userId);
                if (obj != null)
                    return obj.RefUserId;
                return 0;
            }

        }
    }
}