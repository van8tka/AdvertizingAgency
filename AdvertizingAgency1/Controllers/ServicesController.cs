using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvertizingAgency1.Models;
using BusinessLogic;
using DAL;
using System.IO;

namespace AdvertizingAgency1.Controllers
{
    public class ServicesController : Controller
    {
         private DataManager dataManager;
        public int PageSize = 15;

         public ServicesController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
      

        public ActionResult Index2(int page = 1)
        {
            ServicesViewModel service = new ServicesViewModel();
            service.Services = dataManager.PricesRepository.GetPrices().OrderBy(x => x.Код_услуги).Skip((page - 1) * PageSize).Take(PageSize);
          
            var spisPriceTyp = dataManager.PricesRepository.GetPrices().GroupBy(x=>x.Тип).Select(g=> g.Key);

            foreach (var s in spisPriceTyp)
            {
                service.TypPrice.Add(s.ToString());
            }
            //получаем путь к папке
            string domainpath = Server.MapPath("~/Content/UslugiImage");
            //получаем путь 
            var dir = new DirectoryInfo(domainpath);
            //получаем список файлов
            FileInfo[] fileNames = dir.GetFiles("*.*");
            List<string> item = new List<string>();
            //добавляем их в список
            foreach (var file in fileNames)
            {
                item.Add(file.Name);
            }
            service.FileList = item;
            return View(service);
        }

//action с менюхай сбоку по услугам
        public ActionResult SelectTypeUslug(string s, int page = 1)
        {
            ViewBag.Typ = s;
            ServicesViewModel model = new ServicesViewModel();
              model.Services = dataManager.PricesRepository.GetPrices().Where(x=>x.Тип == s).OrderBy(x => x.Код_услуги).Skip((page - 1) * PageSize).Take(PageSize);
            model.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.PricesRepository.GetPrices().Where(x => x.Тип == s).Count()
            };
            
            var spisPriceTyp = dataManager.PricesRepository.GetPrices().GroupBy(x=>x.Тип).Select(g=> g.Key);
            foreach (var sp in spisPriceTyp)
            {
                model.TypPrice.Add(sp.ToString());
            }
            //получаем путь к папке
            string domainpath = Server.MapPath("~/Content/UslugiImage");
            //получаем путь 
            var dir = new DirectoryInfo(domainpath);
            //получаем список файлов
            FileInfo[] fileNames = dir.GetFiles("*.*");
            List<string> item = new List<string>();
            //добавляем их в список
            foreach (var file in fileNames)
            {
                item.Add(file.Name);
            }
            model.FileList = item;
            return View(model);
        }


        public ActionResult PartialMenyService()
        {
                    return PartialView();
        }


     
        [Authorize(Roles = "admin,maneger")]
        [HttpGet]
        public ActionResult CreateService(string s)
        {
            CreateServiceViewModel model = new CreateServiceViewModel();
            model.Type = s;
          return View(model);
        }
        //----------------добавление новой услуги-----------------------------
         [Authorize(Roles = "admin,maneger")]
        [HttpPost]
        public ActionResult CreateService(CreateServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                dataManager.PricesRepository.CreatePrice(0, model.ServiceName, model.ServiceCost, model.Type);
               return RedirectToAction("SelectTypeUslug","Services",new {s=model.Type});
            }
            return View(model);
        }

        //изменение данных
         [Authorize(Roles = "admin,maneger")]
        [HttpGet]
        public ActionResult EditService(int id)
        {
             Услуги usl = dataManager.PricesRepository.GetPricesById(id);
             EditServiceViewModel model = new EditServiceViewModel()
             {
                IDser = usl.Код_услуги,
                ServiceName = usl.Наименование,
                ServiceCost = (int)usl.Цена,
                Type = usl.Тип
             };
           
            return View(model);
        }
         [Authorize(Roles = "admin,maneger")]
        [HttpPost]
        public ActionResult EditService(EditServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                dataManager.PricesRepository.CreatePrice(model.IDser, model.ServiceName,
                model.ServiceCost, model.Type);
                return RedirectToAction("SelectTypeUslug", "Services", new { s = model.Type});
            }
            return View(model);
        }
         [Authorize(Roles = "admin,maneger")]
        [HttpGet]
        public ActionResult DeleteService(int id)
         {
             string typedel = dataManager.PricesRepository.GetPricesById(id).Тип;
            dataManager.PricesRepository.DeletePrice(dataManager.PricesRepository.GetPricesById(id));
            return RedirectToAction("SelectTypeUslug", "Services", new { s = typedel });
        }

     
    }
}
