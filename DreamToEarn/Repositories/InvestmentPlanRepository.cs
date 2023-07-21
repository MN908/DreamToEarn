using DreamToEarn.DbModel;
using DreamToEarn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DreamToEarn.Helper;

namespace DreamToEarn.Repositories
{
    public class InvestmentPlanRepository
    {
        public static InvestmentPlan AddUpdateinvestmentPlan(InvestmentPlanModel investment)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.InvestmentPlans.FirstOrDefault(x => x.UserId == investment.userId && x.IsActive == true);
                if (obj != null)
                {
                    obj.planStatus = investment.planStatus;
                    obj.UserId = investment.userId;
                    obj.planActiveDate = DateTime.Now;

                    if (UsersRepository.userHaveReference(investment.userId)) // this funtion calls when user plan approve.
                    {
                        int? RefUserId = UsersRepository.GetRefUserId(investment.userId);
                        if (RefUserId > 0)
                        {
                            double Comm = (Convert.ToInt32(obj.plan) * 5) / 100;
                            CommissionRepository.AddCommissions(obj.planId, RefUserId.ToInt(), Comm.ToDecimal());
                        }
                    }
                }
                else
                {
                    obj = new InvestmentPlan();
                    obj.plan = investment.plan;
                    obj.planType = investment.planType;
                    obj.planAddDate = DateTime.Now;
                    obj.planStatus = investment.planStatus;
                    obj.UserId = investment.userId;
                    obj.planActiveDate = null;
                    obj.IsActive = true;

                    context.InvestmentPlans.Add(obj);
                }

                context.SaveChanges();
                return obj;

            }
        }
        public static InvestmentPlan InactivetPlan(int UserId)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.InvestmentPlans.FirstOrDefault(x => x.UserId == UserId && x.IsActive == true && x.planStatus == "Approved");
                if (obj != null)
                {
                    obj.IsActive = false;

                }

                context.SaveChanges();

                return obj;

            }
        }

        public static InvestmentPlan InactiveinvestmentPlan(int planId)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.InvestmentPlans.FirstOrDefault(x => x.planId == planId);
                if (obj != null)
                {
                    obj.IsActive = false;

                }

                context.SaveChanges();

                return obj;

            }
        }

        public static InvestmentPlan UpdateinvestmentPlanPercentage(InvestmentPlanModel investment)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.InvestmentPlans.FirstOrDefault(x => x.UserId == investment.userId);
                if (obj != null)
                {
                    obj.InvestPercentage = investment.InvestmentPercentage;
                }

                context.SaveChanges();

                return obj;

            }
        }

        public static InvestmentPlan AdjustEarnings(InvestmentPlanModel investment)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.InvestmentPlans.FirstOrDefault(x => x.UserId == investment.userId);
                if (obj != null)
                {
                    obj.TotalEarnings = investment.totalEarnings;
                }

                context.SaveChanges();

                return obj;

            }
        }

        public static decimal? TotalEarnings(int userId)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.InvestmentPlans.FirstOrDefault(x => x.UserId == userId);
                if (obj != null)
                {
                    if (obj.TotalEarnings != null)
                        return obj.TotalEarnings;

                    return 0;
                }
                return 0;
            }
        }

        public static int TotalInvestmentPlanDaysLeft(int userId)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.InvestmentPlans.FirstOrDefault(x => x.UserId == userId && x.planStatus == "Approved");
                if (obj != null)
                {
                    var ActiveDate = Convert.ToDateTime(obj.planActiveDate);
                    TimeSpan days = ActiveDate - DateTime.Now;
                    if (obj.planType == "3Months")
                    {
                        days = ActiveDate.AddDays(90) - DateTime.Now;
                    }
                    else if (obj.planType == "6Months")
                    {
                        days = ActiveDate.AddDays(180) - DateTime.Now;
                    }
                    if (Convert.ToInt32(days.TotalDays) > 0)
                        return Convert.ToInt32(days.TotalDays);
                }
                return 0;

            }
        }

        public static InvestmentPlan DeleteinvestmentPlan(InvestmentPlanModel investment)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.InvestmentPlans.FirstOrDefault(x => x.UserId == investment.userId);
                if (obj != null)
                {
                    context.InvestmentPlans.Remove(obj);
                }

                context.SaveChanges();
                return obj;

            }
        }

        public static InvestmentPlan AddEarnings(int planId, decimal earning)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.InvestmentPlans.FirstOrDefault(x => x.planId == planId && x.planStatus == "Approved" && x.IsActive == true);
                if (obj != null)
                {
                    if (obj.TotalEarnings == null)
                        obj.TotalEarnings = earning;
                    else
                        obj.TotalEarnings += earning;

                }

                context.SaveChanges();
                return obj;

            }
        }
        public static InvestmentPlan DeductEarnings(int userId, decimal deductEarning)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.InvestmentPlans.FirstOrDefault(x => x.UserId == userId && x.planStatus == "Approved" && x.IsActive == true);
                if (obj != null)
                    obj.TotalEarnings = deductEarning;

                context.SaveChanges();
                return obj;

            }
        }

        public static bool IsPlanExistForUser(int UserId)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.InvestmentPlans.FirstOrDefault(x => x.UserId == UserId && x.IsActive == true);
                if (obj != null)
                    return true;
                return false;
            }
        }

        public static InvestmentPlan IsPlanPendingForUser(int UserId)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.InvestmentPlans.FirstOrDefault(x => x.UserId == UserId);
                if (obj != null)
                    return obj;

                return obj;
            }
        }

        public static List<InvestmentPlan> GetInvestmentPlans(int userId)
        {
            using (var context = new DreamToEarnEntities1())
            {
                var list = context.InvestmentPlans.Where(x => x.UserId == userId)
                    .ToList();
                return list;
            }
        }
        public static InvestmentPlan GetApprovedInvestmentPlans(int userId)
        {
            using (var context = new DreamToEarnEntities1())
            {
                var list = context.InvestmentPlans.FirstOrDefault(x => x.UserId == userId && x.planStatus == "Approved" && x.IsActive == true);

                return list;
            }
        }
        public static List<UsersDetail> GetAllInvestmentPlans(string Role, int userId)
        {
            using (var context = new DreamToEarnEntities1())
            {
                if (Role == "2" || Role == "3")
                    return context.UsersDetails.Where(x => x.UserId == userId).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
                else
                    return context.UsersDetails.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            }

        }
        public static List<UsersDetail> GetInvestmentPlanDetail(int userId)
        {
            using (var context = new DreamToEarnEntities1())
            {
                return context.UsersDetails.Where(x => x.UserId == userId).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            }

        }
        public static List<UsersDetail> GetAllInvestmentPlanDetail()
        {
            using (var context = new DreamToEarnEntities1())
            {
                return context.UsersDetails.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            }

        }
    }
}