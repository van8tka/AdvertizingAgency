using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvertizingAgency1.AboutCompany;
using AdvertizingAgency1.Models;
using BusinessLogic;
using DAL;

namespace AdvertizingAgency1.Controllers
{
    public class AdresController : Controller
    {
        private DataManager dataManager;
        public int PageSize = 15;
        public AdresController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index(int page = 1)
        {
          
            PricesAdresViewModel adres = new PricesAdresViewModel();
            adres.Adres = dataManager.AdresRepository.GetAdreses().OrderBy(x=>x.Код_адреса).Skip((page-1)*PageSize).Take(PageSize);
           adres.PagingInfoAdres = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.AdresRepository.GetAdreses().Count()
            };
            return View(adres);
        }

          [Authorize(Roles = "admin,maneger")]
        [HttpGet]
        public ActionResult CreateAdres()
        {
            return View();
        }
         [Authorize(Roles = "admin,maneger")]
        [HttpPost]
        public ActionResult CreateAdres(CreateAdresViewModel model)
        {
            if (ModelState.IsValid)
            {
                dataManager.AdresRepository.CreateAdres(0, model.AdresName, model.AdresCost);

                return RedirectToAction("WasCreateAdres",new {id=dataManager.AdresRepository.GetAdreses().OrderBy(x => x.Код_адреса).Last().Код_адреса});
            }
            return View();
        }
        
        [HttpGet]
        public ActionResult WasCreateAdres(int id)
        {

            return View(id);
        }
         [Authorize(Roles = "admin,maneger")]
        [HttpGet]
        public ActionResult EditAdres(int id)
        {
            EditAdresViewModel model = new EditAdresViewModel();
            model.IDadres = id;
             Адреса adr = dataManager.AdresRepository.GetAdresesById(id);
             model.NameAdres = adr.Адрес;
             model.AdresCost = (int)adr.Цена_на_месяц;
            return View(model);
        }

      [Authorize(Roles = "admin,maneger")]
        [HttpPost]
        public ActionResult EditAdres(EditAdresViewModel model)
        {
            if (ModelState.IsValid)
            {
                dataManager.AdresRepository.CreateAdres(model.IDadres, model.NameAdres,
                model.AdresCost);
             
                return RedirectToAction("WasCreateAdres",new {id=model.IDadres});
            }
            return View();
        }
         [Authorize(Roles = "admin,maneger")]
        [HttpGet]
        public ActionResult DeleteAdres(int id)
        {
            dataManager.AdresRepository.DeleteAdres( dataManager.AdresRepository.GetAdresesById(id));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AdressTableUse()
        {
          AdressTableUseViewModel model = new AdressTableUseViewModel();
            model.Adres = dataManager.AdresRepository.GetAdreses().OrderBy(x=>x.Код_адреса);
            model.AdreZakaz = dataManager.ZakazyAdresyRepository.GetAdZakaz().OrderBy(x=>x.Код_адреса);
            model.CurrentYear = DateTime.Now.Year;
            model.CurrentMonth = DateTime.Now.Month;
           return View(model);
        }
        //для пользователя
        [HttpGet]
        public ActionResult AdresMapForClients()
        {
           
            return View();
        }



         [Authorize(Roles = "admin,maneger")]
        //для сотрудников
        [HttpGet]
        public ActionResult AdresMap(int id)
        {
          
            return View(id);
        }
         [Authorize(Roles = "admin,maneger")]
        [HttpPost]
        public ActionResult AdresMap(string lat, string lng,string id)
        {
           int IDAdresLast=int.Parse(id);
          
            float Lat = AsFloat(lat);
            float Lng =AsFloat(lng);
            
            dataManager.AdresRepository.CreateKoordinat(IDAdresLast,Lng,Lat);
       
            return RedirectToAction("Index");
        }
        public static float AsFloat(string s)
        {
            return float.Parse(
            s.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator),
            CultureInfo.InvariantCulture);
        }
        public ActionResult GetData()
        {
            // создадим список данных
            List<GoogleMapAdresViewModel> AdresInMap = new List<GoogleMapAdresViewModel>();

            IEnumerable<Координаты> koord = dataManager.AdresRepository.GetKoordinats();
            foreach (Координаты k in koord)
            {
                AdresInMap.Add(new GoogleMapAdresViewModel()
                {
                    ID = k.Код_адреса,
                    Name = dataManager.AdresRepository.GetAdresesById(k.Код_адреса).Адрес,
                    Cost = (int)dataManager.AdresRepository.GetAdresesById(k.Код_адреса).Цена_на_месяц,
                    GeoLat = (double)k.Широта,
                    GeoLong = (double)k.Долгота
                });
            }
            return Json(AdresInMap, JsonRequestBehavior.AllowGet);
        }
    }
}
