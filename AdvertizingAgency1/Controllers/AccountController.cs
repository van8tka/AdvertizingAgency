using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AdvertizingAgency1.Models;
using BusinessLogic;
using DAL;

namespace AdvertizingAgency1.Controllers
{
     [AllowAnonymous]
    public class AccountController : Controller
    {
        private DataManager dataManager;
         private Сотрудники SotrudnikIn;
         
        public AccountController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        [HttpGet]
      
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName,model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        //return RedirectToAction("Index", "Request");
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("","Неверный логин и пароль!");
                }
            }
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private bool ValidateUser(string login, string password)
        {
            bool isValid = false;
            try
            {
                Администрирование user = dataManager.UsersRepository.GetRegisterUser().Where(x=>x.Логин==login && x.Пароль == password).
                FirstOrDefault();
                if (user != null)
                {
                   SotrudnikIn = dataManager.UsersRepository.GetUsersById(user.Код_сотрудника);
                    isValid = true;
                }
            }
            catch 
            {
                isValid = false;
            }
            return isValid;
        }

       
    }
}
