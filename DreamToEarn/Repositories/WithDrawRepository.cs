using DreamToEarn.DbModel;
using DreamToEarn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DreamToEarn.Helper;

namespace DreamToEarn.Repositories
{
    public class WithDrawRepository
    {
        public static DrawAmount WithDrawAmount(WithdrawModel model)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {

                var obj = new DrawAmount();
                //drawId, UserId, totalDraw, totalEarningOnDraw, drawDate, OTPTokens, drawStatus, isAdminApprove 
                obj.UserId = model.UserId;
                obj.totalDraw = Convert.ToDecimal(model.totalDraw);
                obj.totalEarningOnDraw = Convert.ToDecimal(model.totalEarningOnDraw);
                obj.OTPTokens = model.OTPTokens;
                obj.drawDate = DateTime.Now;
                obj.drawStatus = false;
                obj.isAdminApprove = false;
                obj.drawSentByExchanger = false;
                obj.Deductions = model.deductions;
                obj.DrawType = model.drawType;

                context.DrawAmounts.Add(obj);

                context.SaveChanges();

                return obj;

            }
        }


        public static bool IsWithdrawPending(int userId)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.DrawAmounts.FirstOrDefault(x => x.UserId == userId && x.isAdminApprove == false);
                if (obj == null)
                    return true;

                return false;
            }
        }

        public static bool VarifyOTPToken(string OTPToken, int userId)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.DrawAmounts.FirstOrDefault(x => x.OTPTokens.Equals(OTPToken) && x.UserId == userId && x.drawStatus == false && x.isAdminApprove == false);
                if (obj != null)
                    return true;

                return false;
            }
        }

        public static DrawAmount UpdateWithdrawStatus(WithdrawModel model)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.DrawAmounts.FirstOrDefault(x => x.OTPTokens.Equals(model.OTPTokens) && x.UserId == model.UserId && x.drawStatus == false && x.isAdminApprove == false);
                if (obj != null)
                {
                    obj.drawDate = model.drawDate;
                    obj.drawStatus = model.drawStatus;

                }
                context.SaveChanges();

                return obj;

            }
        }

        public static DrawAmount ApproveWithdrawStatus(int drawId)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.DrawAmounts.FirstOrDefault(x => x.drawId== drawId && x.isAdminApprove == false);
                if (obj != null)
                {
                    obj.isAdminApprove = true;
                    obj.drawSentByExchanger = false;
                }
                context.SaveChanges();

                return obj;

            }
        }
        public static DrawAmount ApproveWithdrawStatusForExchanger(int drawId)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.DrawAmounts.FirstOrDefault(x => x.drawId == drawId && x.isAdminApprove == true);
                if (obj != null)
                {
                    obj.isAdminApprove = true;
                    obj.drawSentByExchanger = true;
                    obj.DrawSentDate = DateTime.Now;
                }
                context.SaveChanges();

                return obj;

            }
        }

        public static List<UsersWithdraw> GetAllWithDraws()
        {
            using (var context = new DreamToEarnEntities1())
            {
                return context.UsersWithdraws.Where(x => x.drawStatus == true).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            }

        }

        public static List<UsersWithdraw> GetAllWithDrawsForExchanger()
        {
            using (var context = new DreamToEarnEntities1())
            {
                return context.UsersWithdraws.Where(x=> x.isAdminApprove == true && x.drawStatus == true).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            }

        }

        public static List<UsersWithdraw> GetUserWithDraws(int userId)
        {
            using (var context = new DreamToEarnEntities1())
            {
                return context.UsersWithdraws.Where(x => x.UserId == userId).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            }

        }

        public static DrawAmount GetAllWithDraws(int drawId)
        {
            using (var context = new DreamToEarnEntities1())
            {
                return context.DrawAmounts.FirstOrDefault(x => x.drawId == drawId);
            }

        }

        public static UsersDetail GetUserDetail(int? UserId)
        {
            using (var context = new DreamToEarnEntities1())
            {
                return context.UsersDetails.FirstOrDefault(x => x.UserId == UserId);
            }

        }

        public static List<UsersWithdraw> GetAllApprovedWithDraws()
        {
            using (var context = new DreamToEarnEntities1())
            {
                return context.UsersWithdraws.Where(x=> x.isAdminApprove== true && x.drawStatus == true && x.drawSentByExchanger == true).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            }

        }
        public static string getTotalYTDApprovedWithdraws()
        {
            using (var context = new DreamToEarnEntities1())
            {
                var total = context.UsersWithdraws.Where(x => x.isAdminApprove == true && x.drawStatus == true && x.drawSentByExchanger == true)
                    .ToList().Select( i => i.totalDraw).Sum();

                return total.ToDefaultString();
            }
        }

        public static string getLastApprovedWithdraws()
        {
            using (var context = new DreamToEarnEntities1())
            {
                var total = context.UsersWithdraws.Where(x => x.isAdminApprove == true && x.drawStatus == true && x.drawSentByExchanger == true)
                    .OrderByDescending(x => x.drawDate).FirstOrDefault();

                return total.totalDraw.ToDefaultString();
            }
        }


        public static DrawsDay GetAllWithDrawDays()
        {
            using (var context = new DreamToEarnEntities1())
            {
                return context.DrawsDays.FirstOrDefault();
            }
        }

        public static DrawsDay addDrawDays(int dayOne, int dayTwo, int dayThree, int dayFour)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj1 = context.DrawsDays.FirstOrDefault();
                if (obj1 != null)
                {
                    context.DrawsDays.Remove(obj1);
                }
                context.SaveChanges();

                var obj = new DrawsDay();
                obj.FirstDay = dayOne;
                obj.SecondDay = dayTwo;
                obj.ThirdDay = dayThree;
                obj.ForthDay = dayFour;                
                context.DrawsDays.Add(obj);

                context.SaveChanges();

                return obj;

            }
        }
    }
}