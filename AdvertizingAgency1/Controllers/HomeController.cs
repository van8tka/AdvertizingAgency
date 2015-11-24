using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AdvertizingAgency1.AboutCompany;
using AdvertizingAgency1.Models;
using BusinessLogic;


namespace AdvertizingAgency1.Controllers
{
    public class HomeController : Controller
    {
        private DataManager dataManager;

        public HomeController(DataManager dataManager)
        {
           
            this.dataManager = dataManager;
        }


        public ActionResult Index()
        {
         
            ServicesViewModel service = new ServicesViewModel();
          
            var spisPriceTyp = dataManager.PricesRepository.GetPrices().GroupBy(x => x.Тип).Select(g => g.Key);

            foreach (var s in spisPriceTyp)
            {
                service.TypPrice.Add(s.ToString());
            }
            //получаем путь к папке
        
            string domainpath = Server.MapPath("~/Content/Slider");
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

        [HttpGet]
        public ActionResult AboutProgram()
        {
            return View();
        }





        public ActionResult GetHowPeople()
        {
            // вычеслим колличество новых клиентов
            int KolNewPeople = dataManager.ClientsRepository.GetClients().Where(x => x.Статус == "новый").Count();
            return Json(KolNewPeople, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetHowDogovor()
        {
            // создадим список договоров
       
           int koplate = dataManager.DogovorRepository.GetDogovora().Where(x => x.Статус == "к оплате").Count();
           int koplatena = dataManager.DogovorRepository.GetDogovora().Where(x => x.Статус == "к оплате на продление").Count();
           int oplachen = dataManager.DogovorRepository.GetDogovora().Where(x => x.Статус == "оплачено").Count();
           int oplachenprodlen = dataManager.DogovorRepository.GetDogovora().Where(x => x.Статус == "оплачено продление").Count();
           int ocenkamak = dataManager.DogovorRepository.GetDogovora().Where(x => x.Статус == "оценка макета").Count();
           int trebuetprodlen = dataManager.DogovorRepository.GetDogovora().Where(x => x.Статус == "требует продления").Count();
           int chastichn = dataManager.DogovorRepository.GetDogovora().Where(x => x.Статус == "частичный демонтаж").Count();
           int polnuy = dataManager.DogovorRepository.GetDogovora().Where(x => x.Статус == "полный демонтаж").Count();
           int ystanovlen = dataManager.DogovorRepository.GetDogovora().Where(x => x.Статус == "установлено").Count();
           int gotov = dataManager.DogovorRepository.GetDogovora().Where(x => x.Статус == "готово").Count();
           int zakryt = dataManager.DogovorRepository.GetDogovora().Where(x => x.Статус == "закрыто").Count();
           int razrabdiz = dataManager.DogovorRepository.GetDogovora().Where(x => x.Статус == "разработка дизайна").Count();
           int nadorab = dataManager.DogovorRepository.GetDogovora().Where(x => x.Статус == "на доработку").Count();
           int gotovorig = dataManager.DogovorRepository.GetDogovora().Where(x => x.Статус == "готовить оригинал").Count();
           int vmasters = dataManager.DogovorRepository.GetDogovora().Where(x => x.Статус == "в мастерскую").Count();
           int gotkust = dataManager.DogovorRepository.GetDogovora().Where(x => x.Статус == "готов к установке").Count();
           int gotkdemontag = dataManager.DogovorRepository.GetDogovora().Where(x => x.Статус == "готов к демонтажу").Count();
           int all = dataManager.DogovorRepository.GetDogovora().Count();

            List<SelectListItem> dog = new List<SelectListItem>();
            dog.Add(new SelectListItem { Text = "к оплате", Value = koplate.ToString() });
            dog.Add(new SelectListItem { Text = "к оплате на продление", Value = koplatena.ToString() });
            dog.Add(new SelectListItem { Text = "оплачено", Value = oplachen.ToString() });
            dog.Add(new SelectListItem { Text = "оплачено продление", Value = oplachenprodlen.ToString() });
            dog.Add(new SelectListItem { Text = "оценка макета", Value = ocenkamak.ToString() });
            dog.Add(new SelectListItem { Text = "требует продления", Value = trebuetprodlen.ToString() });
            dog.Add(new SelectListItem { Text = "частичный демонтаж", Value = chastichn.ToString() });
            dog.Add(new SelectListItem { Text = "полный демонтаж", Value = polnuy.ToString() });
            dog.Add(new SelectListItem { Text = "установлено", Value = ystanovlen.ToString() });
            dog.Add(new SelectListItem { Text = "готово", Value = gotov.ToString() });
            dog.Add(new SelectListItem { Text = "закрыто", Value = zakryt.ToString() });
            dog.Add(new SelectListItem { Text = "разработка дизайна", Value = razrabdiz.ToString() });
            dog.Add(new SelectListItem { Text = "на доработку", Value = nadorab.ToString() });
            dog.Add(new SelectListItem { Text = "готовить оригинал", Value = gotovorig.ToString() });
            dog.Add(new SelectListItem { Text = "в мастерскую", Value = vmasters.ToString() });
            dog.Add(new SelectListItem { Text = "готов к установке", Value = gotkust.ToString() });
            dog.Add(new SelectListItem { Text = "готов к демонтажу", Value = gotkdemontag.ToString() });
            dog.Add(new SelectListItem { Text = "всего", Value = all.ToString() });        
         
            return Json(dog, JsonRequestBehavior.AllowGet);
        }


    }
}
