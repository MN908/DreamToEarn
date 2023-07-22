using DreamToEarn.DbModel;
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
    public class DrawAmountController : AppController
    {
        // GET: DrawAmount
        [HttpGet]
        [Authorize(Roles = "1,2,3")]
        public ActionResult Withdraw()
        {
            var date = DateTime.Now;
            var days = WithDrawRepository.GetAllWithDrawDays();
            ViewBag.isWithdrawDate = "false";

            if (date.Day == days.FirstDay)
                ViewBag.isWithdrawDate = "true";
            if (date.Day == days.SecondDay)
                ViewBag.isWithdrawDate = "true";
            if (date.Day == days.ThirdDay)
                ViewBag.isWithdrawDate = "true";
            if (date.Day == days.ForthDay)
                ViewBag.isWithdrawDate = "true";

            ViewBag.dayOne = days.FirstDay;
            ViewBag.dayTwo = days.SecondDay;
            ViewBag.dayThree = days.ThirdDay;
            ViewBag.dayFour = days.ForthDay;

            var earnings = ViewBag.TotalEarnings = InvestmentPlanRepository.TotalEarnings(CurrentUser.UserId);
            var commisions = ViewBag.TotalCommissions = CommissionRepository.GetUserApprovedCommision(CurrentUser.UserId);
            if (earnings == null)
            {
                earnings = 0;
                ViewBag.TotalEarnings = 0;
            }
            if (commisions == null)
            {
                commisions = 0;
                ViewBag.TotalCommissions = 0;
            }

            if (earnings > 0 || commisions > 0)
                ViewBag.TotalBalance = Convert.ToDecimal(earnings) + Convert.ToDecimal(commisions);// earning should be commission + earnings    
            else
                ViewBag.TotalBalance = 0;

            var usersDetail = UsersRepository.GetUser(CurrentUser.UserId);
            ViewBag.BankAccount = usersDetail.BankAccount.ToDefaultString();
            ViewBag.JazzCash = usersDetail.JazzCash.ToDefaultString();
            ViewBag.EasyPaisa = usersDetail.EasyPaisa.ToDefaultString();
            ViewBag.BankTitle = usersDetail.BankTitle;

            var ex = UsersRepository.GetAllExchanger();
            ViewBag.ExchangerName = $"{ex.FirstName} {ex.LastName}";


            return View();
        }

        [HttpPost]
        [Authorize(Roles = "1,2,3")]
        public JsonResult Withdraw(WithdrawModel model)
        {
            try
            {
                var obj = UsersRepository.GetUser(model.UserId);
                if (model.drawType == "BankAccount")
                {
                    if (obj.BankAccount.ToDefaultString() == "")
                        return Json(new { IsSuccess = false, Message = "Your Bank Account detail is missing, please update your profle before withdraw." }, JsonRequestBehavior.AllowGet);
                }
                if (model.drawType == "JazzCash")
                {
                    if (obj.JazzCash.ToDefaultString() == "")
                        return Json(new { IsSuccess = false, Message = "Your Jazz Cash number is missing, please update your profle before withdraw." }, JsonRequestBehavior.AllowGet);
                }
                if (model.drawType == "EasyPaisa")
                {
                    if (obj.EasyPaisa.ToDefaultString() == "")
                        return Json(new { IsSuccess = false, Message = "Your Easy Paisa number is missing, please update your profle before withdraw." }, JsonRequestBehavior.AllowGet);
                }

                if (WithDrawRepository.IsWithdrawPending(model.UserId) == false)
                {
                    return Json(new { IsSuccess = false, Message = "Your transaction is already in Pending state. You can not withdraw." }, JsonRequestBehavior.AllowGet);
                }
                if (model.totalDraw >= model.totalEarningOnDraw)
                {
                    return Json(new { IsSuccess = false, Message = "Draw amount should be less than then total balance." }, JsonRequestBehavior.AllowGet);
                }

                var deduction = (model.totalDraw * 2.75) / 100;
                var grandTotalDraw = model.totalDraw + deduction;

                if (grandTotalDraw > model.totalEarningOnDraw) // if total draw + deduction is greater than total balance
                {
                    return Json(new { IsSuccess = false, Message = "Please enter less withdraw amount, your balance is insufficient for this request." }, JsonRequestBehavior.AllowGet);
                }
                model.deductions = Convert.ToDecimal(deduction);

                Random random = new Random();
                int OTPToken = random.Next(10000000);

                model.OTPTokens = OTPToken.ToDefaultString();

                var email = new EmailHelper();
                email.emailToAddress = CurrentUser.Email;
                email.subject = "Dream to Earn First Login Token";
                email.body = $"Dear User, <br> <br> Please copy following OTP for your withdraw transaction. <br> <br> Your OTP is <b>{OTPToken}</b> <br><br> Please do not delete this token before withdraw. <br> <br> This is an auto email, please do not reply.";
                email.SendEmail();

                var draw = WithDrawRepository.WithDrawAmount(model);



                return Json(new { IsSuccess = true, Message = "OTP sent" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "Amount with draw failed" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [Authorize(Roles = "1,2,3")]
        public JsonResult VerifyOTPTokens(WithdrawModel model)
        {
            try
            {

                if (WithDrawRepository.VarifyOTPToken(model.OTPTokens, model.UserId))
                {
                    //var deduction = Math.Round((model.totalDraw * 2.75) / 100, 2, MidpointRounding.ToEven);

                    //var totalBalance = model.totalEarningOnDraw;

                    //var totalLeftBalance = totalBalance - model.totalDraw - deduction; //3200 - 3000 - 82.5 = 1717.5
                    //                                                                   //now 82.5 should be added in admin balance; and 1717.5 should be updated to investor commission or earnings

                    //var earnings = model.earnings; //2000
                    //var commissions = model.commissions; //1200                

                    model.drawDate = DateTime.Now;
                    model.drawStatus = true;
                    model.isAdminApprove = false;

                    WithDrawRepository.UpdateWithdrawStatus(model);
                    //make earning zero and add remaining balance to commission

                    //CommissionRepository.DeductCommissions(model.UserId, Convert.ToDecimal(totalLeftBalance));
                    //InvestmentPlanRepository.DeductEarnings(model.UserId, 0);


                    //UsersRepository.UpdateTotalCommissionsForAdmin(Convert.ToDecimal(deduction));

                    return Json(new { IsSuccess = true, Message = "Withdraw successfull, OTP matched" }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { IsSuccess = false, Message = "Withdraw failed, OTP not matched" }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "Withdraw failed" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Authorize(Roles = "1,3")]
        public ActionResult ApproveWithdraw()
        {
            if (CurrentUser.Role == "1")
                return View(WithDrawRepository.GetAllWithDraws());
            else
                return View(WithDrawRepository.GetAllWithDrawsForExchanger());
        }

        [HttpGet]
        [Authorize(Roles = "2, 3")]
        public ActionResult ViewWithDraw()
        {
            return View(WithDrawRepository.GetUserWithDraws(CurrentUser.UserId));
        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public ActionResult setDrawDates()
        {
            var days = WithDrawRepository.GetAllWithDrawDays();
            ViewBag.dayOne = days.FirstDay;
            ViewBag.dayTwo = days.SecondDay;
            ViewBag.dayThree = days.ThirdDay;
            ViewBag.dayFour = days.ForthDay;
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public ActionResult drawHistory()
        {
            var totalYTDWithdraw = WithDrawRepository.getTotalYTDApprovedWithdraws();
            var lastWithDraw = WithDrawRepository.getLastApprovedWithdraws();
            ViewBag.totalYTDWithdraw = totalYTDWithdraw;
            ViewBag.lastWithDraw = lastWithDraw;  
            
            return View(WithDrawRepository.GetAllApprovedWithDraws());
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public JsonResult setDrawDates(int dayOne, int dayTwo, int dayThree, int dayFour)
        {
            try
            {
                WithDrawRepository.addDrawDays(dayOne, dayTwo, dayThree, dayFour);

                return Json(new { IsSuccess = true, Message = "Withdraw days added successfull" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "Adding Withdraw days fail" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = "1,3")]
        public JsonResult ApproveWithdraw(int drawId, int UserId)
        {
            try
            {
                var model = WithDrawRepository.GetAllWithDraws(drawId);

                //var deduction = Math.Round((Convert.ToDouble(model.totalDraw) * 2.75) / 100, 2, MidpointRounding.ToEven);

                //var totalBalance = Convert.ToDouble(model.totalEarningOnDraw);

                //var totalLeftBalance = totalBalance - Convert.ToDouble(model.totalDraw) - deduction; //3200 - 3000 - 82.5 = 1717.5
                //now 82.5 should be added in admin balance; and 1717.5 should be updated to investor commission or earnings
                //make earning zero and add remaining balance to commission

                WithDrawRepository.ApproveWithdrawStatus(drawId);
                //CommissionRepository.DeductCommissions(UserId, Convert.ToDecimal(totalLeftBalance));
                //InvestmentPlanRepository.DeductEarnings(UserId, 0);
                //UsersRepository.UpdateTotalCommissionsForAdmin(Convert.ToDecimal(deduction));

                return Json(new { IsSuccess = true, Message = "Withdraw approved successfull" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "Withdraw approved failed" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [Authorize(Roles = "1,3")]
        public JsonResult ApproveExchangerWithdraw(int drawId, int UserId)
        {
            try
            {
                var model = WithDrawRepository.GetAllWithDraws(drawId);

                var UserDetail = WithDrawRepository.GetUserDetail(model.UserId);

                var deduction = Math.Round((Convert.ToDouble(model.totalDraw) * 2.75) / 100, 2, MidpointRounding.ToEven);

                var totalBalance = Convert.ToDouble(UserDetail.TotalEarnings) + Convert.ToDouble(UserDetail.TotalCommissionsx); // Current balance;

                var totalLeftBalance = totalBalance - Convert.ToDouble(model.totalDraw) - deduction; //3200 - 3000 - 82.5 = 1717.5
                                                                                                     //now 82.5 should be added in admin balance; and 1717.5 should be updated to investor commission or earnings

                //make earning zero and add remaining balance to commission

                WithDrawRepository.ApproveWithdrawStatusForExchanger(drawId);
                CommissionRepository.DeductCommissions(UserId, Convert.ToDecimal(totalLeftBalance));
                InvestmentPlanRepository.DeductEarnings(UserId, 0);


                UsersRepository.UpdateTotalCommissionsForAdmin(Convert.ToDecimal(deduction));

                return Json(new { IsSuccess = true, Message = "Withdraw approved successfull" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "Withdraw approved failed" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}