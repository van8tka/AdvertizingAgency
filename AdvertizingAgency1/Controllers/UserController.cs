using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using AdvertizingAgency1.Models;
using BusinessLogic;
using DAL;

namespace AdvertizingAgency1.Controllers
{
    [Authorize(Roles = "admin,maneger,designer,master,installer,bookkeeper")]
    public class UserController : Controller
    {

        private DataManager dataManager;

        public UserController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public ActionResult Index()
        {
            UsersViewModel users = new UsersViewModel();
            users.Users = dataManager.UsersRepository.GetUsers().OrderBy(x => x.ФИО_сотрудника);
            users.Registers = dataManager.UsersRepository.GetRegisterUser();
            return View(users);
        }
         [Authorize(Roles = "admin")]
        public ActionResult DeleteUser(int id)
        {
            dataManager.UsersRepository.DeleteUser(dataManager.UsersRepository.GetUsersById(id));
            return RedirectToAction("Index","User");
        }
         [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult CreateUser()
        {
            return View();
        }
         [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult CreateUser(CreateUserViewModel model)
        {
           int Kod = 0;
            if (ModelState.IsValid)
            {
                dataManager.UsersRepository.CreateUser(Kod,model.Fio,model.Dolgnost,model.Adres,model.Phone,model.Email);
                int idSotrLast = dataManager.UsersRepository.GetUsers().OrderBy(x => x.Код_сотрудника).Last().Код_сотрудника;
                return RedirectToAction("RegisterUser", new { id = idSotrLast });              
            }
            return View(model);
        }

         [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult ChangeUser(int id)
        {         
            Сотрудники sotr = dataManager.UsersRepository.GetUsersById(id);
            CreateUserViewModel model = new CreateUserViewModel
            {
                Kod = sotr.Код_сотрудника,
                Adres = sotr.Адрес,
                Dolgnost = sotr.Должность,
                Email = sotr.Email,
                Fio = sotr.ФИО_сотрудника,
                Phone = sotr.Телефон
            };
            ViewBag.Kod = id;
            return View(model);
        }
         [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult ChangeUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                dataManager.UsersRepository.CreateUser(model.Kod, model.Fio, model.Dolgnost, model.Adres, model.Phone, model.Email);
                return RedirectToAction("RegisterUser",new{id=model.Kod});
            }
            return View(model);
        }
         [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult RegisterUser(int id)
        {
            RegisterUserViewModel model = new RegisterUserViewModel();
          
            model.Kod = id;
             Администрирование admin =
                    dataManager.UsersRepository.GetRegisterUser().Where(x => x.Код_сотрудника == id).FirstOrDefault();

            if (admin != null)
            {
                model.Login = admin.Логин;
                model.Password = admin.Пароль;
                model.SelectedRoles = admin.Группа_доступа;
                model.ConfirmPassword = admin.Пароль;
            }            
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult RegisterUser(RegisterUserViewModel model)
        {
            bool nologin = true;
            foreach (Администрирование a in dataManager.UsersRepository.GetRegisterUser())
            {
               
                if (model.Login == a.Логин)
                {
                    nologin = false;
                    break;
                }
            }
            if (ModelState.IsValid && nologin)
            {
                Администрирование admin =
                    dataManager.UsersRepository.GetRegisterUser().Where(x => x.Код_сотрудника == model.Kod).FirstOrDefault();
                if (admin == null)
                {
                    dataManager.UsersRepository.RegisterUser(model.Kod, model.SelectedRoles , model.Login, model.Password);
                }
                else
                {
                    dataManager.UsersRepository.ChangeRegisterUser(model.Kod, model.SelectedRoles, model.Login, model.Password);
                }
                return RedirectToAction("Index");
            }
           ModelState.AddModelError("","Такой логин уже существует!");
            return View(model);
        }
    }
}
