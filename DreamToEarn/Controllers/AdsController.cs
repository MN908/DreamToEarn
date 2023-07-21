using DreamToEarn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DreamToEarn.Repositories;
using DreamToEarn.Helper;

namespace DreamToEarn.Controllers
{
    public class AdsController : AppController
    {
        // GET: Ads
        [HttpGet]
        [Authorize(Roles = "1")]
        public ActionResult ads()
        {
            return View(AdsRepository.GetAds());
        }
        [HttpPost]
        [Authorize(Roles = "1")]
        public JsonResult ads(AdsModel model)
        {
            try
            {
                var user = AdsRepository.addUpdateAds(model);
                return Json(new { IsSuccess = true, Message = "URL added successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "URL not added" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [Authorize(Roles = "1")]
        public JsonResult deleteAds(AdsModel model)
        {
            try
            {
                var user = AdsRepository.deleteAd(model);
                return Json(new { IsSuccess = true, Message = "URL deleted successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "URL not deleted" }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Ads
        [HttpGet]
        [Authorize(Roles = "2, 3")]
        public ActionResult viewTodayAds()
        {
            var obj = InvestmentPlanRepository.IsPlanPendingForUser(CurrentUser.UserId);
            if (obj == null)
            {
                return View(AdsRepository.GetEmptyTodayAds(0));
            }

            if (obj.planStatus == "Approved")
                return View(AdsRepository.GetTodayAds(CurrentUser.UserId));
            else
                return View(AdsRepository.GetEmptyTodayAds(0));


        }
        [HttpPost]
        [Authorize(Roles = "2, 3")]
        public JsonResult disableAds(int Id)
        {
            try
            {
                var user = AdsRepository.disableAd(Id);
                return Json(new { IsSuccess = true, Message = "Ad viewed successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "Ad viewed failed" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}