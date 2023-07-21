using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DreamToEarn.Helper;
using DreamToEarn.Repositories;

namespace DreamToEarn.Controllers
{
    public class TeamsController : AppController
    {
        // GET: Teams
        [HttpGet]
        [Authorize(Roles = "1, 2, 3")]
        public ActionResult Index()
        {
            return View(UsersRepository.GetTeam(CurrentUser.UserId));
        }
        [HttpGet]
        [Authorize(Roles = "1")]
        public ActionResult ViewAllUsers()
        {
            return View(UsersRepository.GetAllTeam());
        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public ActionResult ViewAllReferrer(int RefUserId)
        {            
            return View(UsersRepository.GetAllTeam(RefUserId));
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public JsonResult View_AllReferrer()
        {
            try
            {
                var result = CommissionRepository.GetAllReferrer();
                return Json(new { IsSuccess = true, Message = "Data received", Data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "Investment Earnings not Adjusted " }, JsonRequestBehavior.AllowGet);
            } 
        }


    }
}