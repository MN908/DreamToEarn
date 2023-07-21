using DreamToEarn.DbModel;
using DreamToEarn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamToEarn.Repositories
{
    public class CommissionRepository
    {
        public static Commission AddCommissions(int planId, int userId, decimal TotalCommission)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = new Commission();
                obj.planId = planId;
                obj.UserId = userId;
                obj.TotalCommission = TotalCommission;
                obj.ComDate = DateTime.Now;
                obj.IsApproved = false;
                context.Commissions.Add(obj);

                context.SaveChanges();

                decimal? _totalCommission = context.Commissions.Where(x => x.UserId == userId).Sum(x=> x.TotalCommission);

                //UsersRepository.UpdateTotalCommissions(userId, _totalCommission);

                return obj;

            }
        }

        public static User DeductCommissions(int userId, decimal deductCommissions)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.Users.FirstOrDefault(x => x.UserId == userId);
                if (obj != null)
                    obj.TotalCommissions = deductCommissions;

                context.SaveChanges();
                return obj;

            }
        }

        public static Commission ApproveCommissions(int ComId)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.Commissions.FirstOrDefault(x => x.ComId == ComId);

                if (obj != null)
                {
                    obj.IsApproved = true;
                    UsersRepository.UpdateTotalCommissions(obj.UserId, obj.TotalCommission);
                }
                context.SaveChanges();
                return obj;

            }
        }

        public static decimal? GetUserApprovedCommision(int userId)
        {
            using (var context = new DreamToEarnEntities1())
            {
                return context.Users.Where(x => x.UserId == userId).Sum(x=> x.TotalCommissions);
            }
        }

        public static decimal? GetUserPendingCommision(int userId)
        {
            using (var context = new DreamToEarnEntities1())
            {
                return context.Commissions.Where(x => x.UserId == userId && x.IsApproved == false).Sum(x => x.TotalCommission);
            }
        }

        public static List<UsersCommission> ViewCommission(int userId, string role)
        {
            using (var context = new DreamToEarnEntities1())
            {
                if (role == "1")
                    return context.UsersCommissions.Where(x => x.Email != "kumail.mehdi001@gmail.com").OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
                else
                    return context.UsersCommissions.Where(x => x.UserId == userId && x.Email != "kumail.mehdi001@gmail.com").OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();

            }

        }

        public static List<CommReferrer> GetAllReferrer()
        {
            using (var context = new DreamToEarnEntities1())
            {
                return context.CommReferrers.Where(x => x.Email != "kumail.mehdi001@gmail.com")
                    .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName).ToList();
            }

        }
    }
}