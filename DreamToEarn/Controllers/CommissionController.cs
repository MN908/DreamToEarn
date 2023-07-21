using DreamToEarn.DbModel;
using DreamToEarn.Helper;
using DreamToEarn.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DreamToEarn.Controllers
{
    public class CommissionController : AppController
    {
        // GET: Commission        
        [Authorize(Roles = "1")]
        public ActionResult Index(int RefUserId)
        {
            ViewBag.Ref = CommissionRepository.GetAllReferrer();
            return View(UsersRepository.GetAllTeam(RefUserId));
        }
        [HttpGet]
        [Authorize(Roles = "1, 2, 3")]
        public ActionResult ViewCommissions()
        {
            return View(CommissionRepository.ViewCommission(CurrentUser.UserId, CurrentUser.Role).AsEnumerable());
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public JsonResult ApproveCommission(int ComId)
        {
            try
            {
                var Com = CommissionRepository.ApproveCommissions(ComId);
                return Json(new { IsSuccess = true, Message = "Commission Approved successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "Commission Approved failed" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}