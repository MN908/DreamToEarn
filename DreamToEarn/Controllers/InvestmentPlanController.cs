using DreamToEarn.Helper;
using DreamToEarn.Models;
using DreamToEarn.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DreamToEarn.Controllers
{
    public class InvestmentPlanController : AppController
    {
        // GET: InvestmentPlan
        public ActionResult Index()
        {            
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "1, 2, 3")]
        public ActionResult threeMonthPlan()
        {
            int daysLeft = InvestmentPlanRepository.TotalInvestmentPlanDaysLeft(CurrentUser.UserId);
            if(daysLeft <= 0)
            {
                InvestmentPlanRepository.InactivetPlan(CurrentUser.UserId);
            }

            return View(InvestmentPlanRepository.GetInvestmentPlans(CurrentUser.UserId).AsEnumerable());
        }

        [HttpPost]
        [Authorize(Roles = "1, 2, 3")]
        public JsonResult threeMonthPlan(InvestmentPlanModel model)
        {
            try
            {                
                if (InvestmentPlanRepository.IsPlanExistForUser(model.userId))
                {
                    return Json(new { IsSuccess = false, Message = "Active Plan already exist for this user" }, JsonRequestBehavior.AllowGet);
                }
                var user = InvestmentPlanRepository.AddUpdateinvestmentPlan(model);
                return Json(new { IsSuccess = true, Message = "Plan added successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "Plan not added" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        [Authorize(Roles = "1, 2, 3")]
        public ActionResult SixMonthPlan()
        {
            int daysLeft = InvestmentPlanRepository.TotalInvestmentPlanDaysLeft(CurrentUser.UserId);
            if (daysLeft <= 0)
            {
                InvestmentPlanRepository.InactivetPlan(CurrentUser.UserId);
            }

            return View(InvestmentPlanRepository.GetInvestmentPlans(CurrentUser.UserId).AsEnumerable());
        }

        [HttpPost]
        [Authorize(Roles = "1, 2, 3")]
        public JsonResult SixMonthPlan(InvestmentPlanModel model)
        {
            try
            {
                if (InvestmentPlanRepository.IsPlanExistForUser(model.userId))
                {
                    return Json(new { IsSuccess = false, Message = "Plan already exist for this user" }, JsonRequestBehavior.AllowGet);
                }
                var user = InvestmentPlanRepository.AddUpdateinvestmentPlan(model);
                return Json(new { IsSuccess = true, Message = "Plan added successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "Plan not added" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public ActionResult ViewAllInvestment()
        {
            return View(InvestmentPlanRepository.GetAllInvestmentPlanDetail());
        }       

        [HttpGet]
        [Authorize(Roles = "1")]
        public ActionResult ApproveInvestmentForUser(string UserId)
        {            
            return View(InvestmentPlanRepository.GetInvestmentPlanDetail(Convert.ToInt32(UserId)));
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public JsonResult ApproveInvestmentForUser(InvestmentPlanModel model)
        {
            try
            {                
                var user = InvestmentPlanRepository.AddUpdateinvestmentPlan(model);
                return Json(new { IsSuccess = true, Message = "Plan added successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "Plan not added" }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        [Authorize(Roles = "1")]
        public JsonResult InactiveInvestmentForUser(string planId)
        {
            try
            {
                var user = InvestmentPlanRepository.InactiveinvestmentPlan(Convert.ToInt32(planId));
                return Json(new { IsSuccess = true, Message = "Plan inactive successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "Plan not added" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public ActionResult AdjustInvestmentForUser(string UserId)
        {            
            return View(InvestmentPlanRepository.GetInvestmentPlanDetail(Convert.ToInt32(UserId)));
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public JsonResult AdjustInvestmentForUser(InvestmentPlanModel model)
        {
            try
            {
                var user = InvestmentPlanRepository.AdjustEarnings(model);
                return Json(new { IsSuccess = true, Message = "Investment Earnings Adjusted successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "Investment Earnings not Adjusted " }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public JsonResult UpdateInvestment(InvestmentPlanModel model)
        {
            try
            {
                var user = InvestmentPlanRepository.UpdateinvestmentPlanPercentage(model);
                return Json(new { IsSuccess = true, Message = "Investment Updated successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "Investment not Updated " }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public JsonResult DeleteInvestmentForUser(InvestmentPlanModel model)
        {
            try
            {
                var user = InvestmentPlanRepository.DeleteinvestmentPlan(model);
                return Json(new { IsSuccess = true, Message = "Plan removed successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "Plan not removed, error" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}