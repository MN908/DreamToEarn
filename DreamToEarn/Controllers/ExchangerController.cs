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
    public class ExchangerController : AppController
    {
        // GET: Exchanger
        [HttpGet]
        [Authorize(Roles="1")]
        public ActionResult SetExchanger()
        {
            ViewBag.Users = UsersRepository.GetAllTeam();
            return View(UsersRepository.GetAllExchangers());
        }
        [HttpPost]
        [Authorize(Roles = "1")]
        public JsonResult SetExchanger(int UserId)
        {
            try
            {               
                if(UsersRepository.isExchangerAlreadySet())
                    return Json(new { IsSuccess = false, Message = "You can not add two exchanger." }, JsonRequestBehavior.AllowGet);

                var user = UsersRepository.SetExchangerUser(UserId);
                return Json(new { IsSuccess = true, Message = "User set as Exchanger successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "User set as Exchanger faield, please try again!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public JsonResult RemoveExchanger(int UserId)
        {
            try
            {
                var user = UsersRepository.RemoveExchangerUser(UserId);
                return Json(new { IsSuccess = true, Message = "User removed as Exchanger successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "User remove as Exchanger faield, please try again!" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}