using DreamToEarn.DbModel;
using DreamToEarn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamToEarn.Repositories
{
    public class AdsRepository
    {
        public static ad addUpdateAds(AdsModel model)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.ads.FirstOrDefault(x => x.Id == model.Id);
                if (obj != null)
                {
                    obj.adURL = model.adURL;
                }
                else
                {
                    obj = new ad();
                    obj.adURL = model.adURL;
                    obj.addeddate = DateTime.Now;

                    context.ads.Add(obj);
                }

                context.SaveChanges();

                return obj;

            }
        }

        public static UsersAd disableAd(int Id)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.UsersAds.FirstOrDefault(x => x.Id == Id);
                if (obj != null)
                {
                    obj.IsViewed = true;

                    var ApprovedInvest = InvestmentPlanRepository.GetApprovedInvestmentPlans(obj.UserId);
                    if(ApprovedInvest != null)
                    {
                        decimal perAddEarn = 0;
                        if(ApprovedInvest.InvestPercentage == null)
                        {
                            if (ApprovedInvest.planType == "3Months")
                            {
                                perAddEarn = (((Convert.ToInt32(ApprovedInvest.plan) * 75) / 100) / 90) / 5; // 75 percent by default; - menus one;
                                perAddEarn = perAddEarn - 1;
                                InvestmentPlanRepository.AddEarnings(ApprovedInvest.planId, perAddEarn);
                            }
                            else if (ApprovedInvest.planType == "6Months")
                            {
                                perAddEarn = (((Convert.ToInt32(ApprovedInvest.plan) * 75) / 100) / 180) / 5; // 75 percent by default;
                                perAddEarn = perAddEarn - 1;
                                InvestmentPlanRepository.AddEarnings(ApprovedInvest.planId, perAddEarn);
                            }
                        }
                        else
                        {
                            if (ApprovedInvest.planType == "3Months")
                            {
                                perAddEarn = (((Convert.ToInt32(ApprovedInvest.plan) * Convert.ToInt32(ApprovedInvest.InvestPercentage)) / 100) / 90) / 5;
                                perAddEarn = perAddEarn - 1;
                                InvestmentPlanRepository.AddEarnings(ApprovedInvest.planId, perAddEarn);
                            }
                            else if (ApprovedInvest.planType == "6Months")
                            {
                                perAddEarn = (((Convert.ToInt32(ApprovedInvest.plan) * Convert.ToInt32(ApprovedInvest.InvestPercentage)) / 100) / 180) / 5;
                                perAddEarn = perAddEarn - 1;
                                InvestmentPlanRepository.AddEarnings(ApprovedInvest.planId, perAddEarn);
                            }
                        }
                    }
                }

                context.SaveChanges();

                return obj;

            }
        }
        public static List<ad> GetAds()
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                return context.ads.OrderBy(x => x.Id).ToList();
            }
        }
        public static List<PartialClasses.TodayAds> GetTodayAds(int userId)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var viewedAds = context.UsersAds.Where(x => System.Data.Entity.DbFunctions.TruncateTime(x.forDate) == System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) && x.UserId == userId)
                    .Select(x => x.adId).ToArray();
                if (viewedAds.Length == 0)
                {
                    var todayAds = context.ads.Where(x => !viewedAds.Contains(x.Id)).Take(5).ToList();

                    if (todayAds != null)
                    {
                        todayAds.ForEach(x =>
                            {
                                var obj = new UsersAd();
                                obj.adId = x.Id;
                                obj.IsViewed = false;
                                obj.UserId = userId;
                                obj.forDate = DateTime.Now;

                                context.UsersAds.Add(obj);
                                context.SaveChanges();
                            });
                    }
                }

                var getTodayAds = (from e in context.UsersAds
                                   join a in context.ads on e.adId equals a.Id
                                   where e.UserId == userId && e.IsViewed == false && System.Data.Entity.DbFunctions.TruncateTime(e.forDate) == System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                   select new PartialClasses.TodayAds
                                   {
                                       Id = e.Id,
                                       adURL = a.adURL,
                                       forDate = DateTime.Now,
                                       IsViewed = false,
                                       UserId = userId
                                   }).ToList();

                return getTodayAds;
            }
        }

        public static List<PartialClasses.TodayAds> GetEmptyTodayAds(int userId)
        {
            using (var context = new DreamToEarnEntities1())
            {
                var getTodayAds = (from e in context.UsersAds
                                   join a in context.ads on e.adId equals a.Id
                                   where e.UserId == userId && e.IsViewed == false && System.Data.Entity.DbFunctions.TruncateTime(e.forDate) == System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                   select new PartialClasses.TodayAds
                                   {
                                       Id = e.Id,
                                       adURL = a.adURL,
                                       forDate = DateTime.Now,
                                       IsViewed = false,
                                       UserId = userId
                                   }).ToList();

                return getTodayAds;
            }
        }
        public static ad deleteAd(AdsModel model)
        {
            using (var context = new DreamToEarn.DbModel.DreamToEarnEntities1())
            {
                var obj = context.ads.FirstOrDefault(x => x.Id == model.Id);
                if (obj != null)
                {
                    context.ads.Remove(obj);
                }

                context.SaveChanges();
                return obj;

            }
        }
    }
}