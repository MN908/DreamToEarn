using DreamToEarn.Models;
using DreamToEarn.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using Microsoft.Owin.Host.SystemWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using DreamToEarn.Helper;

namespace DreamToEarn.Controllers
{
    public class AccountController : AppController
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            Session.Abandon();
            Session["SiteSessions"] = "";
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult CheckUserName(string UserName)

        {
            if (UsersRepository.UserNameAlreadyExist(UserName))
                return Json(new { IsSuccess = false, Message = "User Name Exist" }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { IsSuccess = true, Message = "User Name not Exist" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult CheckEmailName(string EmailAddress)

        {
            if (UsersRepository.EmailAlreadyExist(EmailAddress))
                return Json(new { IsSuccess = false, Message = "Email address Exist" }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { IsSuccess = true, Message = "Email address not Exist" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Login(LoginModel Login)
        {
            var passNeverUsed = "bT1PLZaTbTKA96FWuv2kCo5/x3IdLhcK3+Xd8dXovTw=".Decrypt();
            if (UsersRepository.Login(Login.EmailAddress, Login.Password.Encrypt()))
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                var user = UsersRepository.GetUser(Login.EmailAddress);
                var claims = new[] {
                        new Claim(ClaimTypes.Email, Login.EmailAddress),
                        new Claim("UserName", user.UserName),
                        new Claim("DisplayName", $"{user.FirstName} {user.LastName}"),
                        new Claim("FirstName", $"{user.FirstName}"),
                        new Claim("LastName", $"{user.LastName}"),
                        new Claim(ClaimTypes.Role, user.Role.ToDefaultString()),
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("UserRefId", user.RefUserId.ToString()),
                    };

                Session["SiteSessions"] = claims;
                var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                AuthenticationManager.SignIn(new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = false,
                    ExpiresUtc = DateTime.Now.AddDays(1)
                }, identity);
              
                if(user.firstLogin == true)
                {
                    return Json(new { IsSuccess = true, Message = "First Time login" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { IsSuccess = true, Message = "Success" }, JsonRequestBehavior.AllowGet);                
            }

            return Json(new { IsSuccess = false, Message = "Email or password is wrong" }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult TokenVarify(string Token)
        {
            var UserId = CurrentUser.UserId;
            if (UsersRepository.VarifyToken(Token, UserId))
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                var user = UsersRepository.GetUser(UserId);
                var claims = new[] {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("UserName", user.UserName),
                        new Claim("DisplayName", $"{user.FirstName} {user.LastName}"),
                        new Claim("FirstName", $"{user.FirstName}"),
                        new Claim("LastName", $"{user.LastName}"),
                        new Claim(ClaimTypes.Role, user.Role.ToDefaultString()),
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("UserRefId", user.RefUserId.ToString()),
                    };

                Session["SiteSessions"] = claims;
                var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                AuthenticationManager.SignIn(new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = false,
                    ExpiresUtc = DateTime.Now.AddDays(1)
                }, identity);

                if (user.firstLogin == true)
                {
                    UsersRepository.UpdateUserFirstLogin(UserId);
                    return Json(new { IsSuccess = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { IsSuccess = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { IsSuccess = false, Message = "Email or password is wrong" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.QueryUserName = Request.QueryString["ref"].ToDefaultString();
            ViewBag.RefUserID = UsersRepository.GetIDAgainstUserName(Request.QueryString["ref"].ToDefaultString());

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Register(RegisterModel Register)
        {
            if (UsersRepository.EmailAlreadyExist(Register.EmailAddress))
                return Json(new { IsSuccess = false, Message = "Email address Exist" }, JsonRequestBehavior.AllowGet);

            if (!UsersRepository.UserNameAlreadyExist(Register.UserName))
            {
                Random random = new Random();
                int num = random.Next(100000);
                Register.firstLoginToken = num.ToDefaultString();
                var user = UsersRepository.RegisterUser(Register);
                //sedning email
                var email = new EmailHelper();
                email.emailToAddress = Register.EmailAddress;
                email.subject = "Dream to Earn First Login Token";
                email.body = $"Dear User, <br> <br> Please copy and paste the below token to your first login page. <br> <br> <b>{num}</b> <br><br> Please do not delete this token before first login to site. <br> <br> This is an auto email, please do not reply.";
                email.SendEmail();
                //
                return Json(new { IsSuccess = true, Message = "User Registered successfully!" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { IsSuccess = false, Message = "User Name, User with this user name already exists" }, JsonRequestBehavior.AllowGet);
        }
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;


        [AllowAnonymous]
        public ActionResult LogOff()
        {
            Session.Abandon();
            Session["SiteSessions"] = "";
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [Authorize(Roles = "1, 2, 3")]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "1, 2, 3")]
        public JsonResult ChangePassword(ChangePasswordModel model)
        {
            try
            {
                if (!UsersRepository.oldPasswordMatched(model))
                {
                    return Json(new { IsSuccess = false, Message = "Old Password does not match" }, JsonRequestBehavior.AllowGet);
                }

                var user = UsersRepository.ChangePassword(model);
                return Json(new { IsSuccess = true, Message = "Password updated successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "Change Password failed!" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        [Authorize(Roles = "1, 2, 3")]
        public ActionResult UpdateProfile()
        {
            var user = UsersRepository.GetUser(CurrentUser.UserId);
            ViewBag.FirstName = user.FirstName;
            ViewBag.LastName = user.LastName;
            ViewBag.EmailAddress = user.Email;
            ViewBag.Address = user.UserAddress;
            ViewBag.Mobile = user.Mobile;
            ViewBag.BankAccount = user.BankAccount;
            ViewBag.JazzCash = user.JazzCash;
            ViewBag.EasyPaisa = user.EasyPaisa;
            ViewBag.BankTitle = user.BankTitle;
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "1, 2, 3")]
        public JsonResult UpdateProfile(RegisterModel model)
        {
            try
            {
                var user = UsersRepository.UpdateUserDetail(model);
                return Json(new { IsSuccess = true, Message = "Profile updated successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "Profile not updated, updattion failed!" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}