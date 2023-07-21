using DreamToEarn.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DreamToEarn.Repositories;

namespace DreamToEarn.Controllers
{
    public class DashboardController : AppController
    {
        // GET: Dashboard
        [Authorize(Roles = "1, 2, 3")]
        public ActionResult Index()
        {
            int daysLeft = InvestmentPlanRepository.TotalInvestmentPlanDaysLeft(CurrentUser.UserId);
            if (daysLeft <= 0)
            {
                InvestmentPlanRepository.InactivetPlan(CurrentUser.UserId);
            }
            var user = UsersRepository.GetUser(CurrentUser.UserId);
            ViewBag.Address = user.UserAddress;
            ViewBag.Mobile = user.Mobile;

            ViewBag.TotalEarnings = InvestmentPlanRepository.TotalEarnings(CurrentUser.UserId);
            ViewBag.TotalTeamMember = UsersRepository.TotalTeamMembers(CurrentUser.UserId);
            ViewBag.TotalDaysLeft = InvestmentPlanRepository.TotalInvestmentPlanDaysLeft(CurrentUser.UserId);
            ViewBag.ApproveCommission = CommissionRepository.GetUserApprovedCommision(CurrentUser.UserId);
            ViewBag.PendingCommission = CommissionRepository.GetUserPendingCommision(CurrentUser.UserId);
            ViewBag.InvestmentAmount = UsersRepository.TotalInvestmentPlan(CurrentUser.UserId);
            if (ViewBag.ApproveCommission == null)
                ViewBag.EarningCommission = InvestmentPlanRepository.TotalEarnings(CurrentUser.UserId);
            else
                ViewBag.EarningCommission = InvestmentPlanRepository.TotalEarnings(CurrentUser.UserId) + CommissionRepository.GetUserApprovedCommision(CurrentUser.UserId);


            return View(InvestmentPlanRepository.GetAllInvestmentPlans(CurrentUser.Role, CurrentUser.UserId));
        }
    }
}