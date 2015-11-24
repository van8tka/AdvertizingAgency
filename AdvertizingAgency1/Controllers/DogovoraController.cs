using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvertizingAgency1.Models;
using BusinessLogic;
using DAL;

namespace AdvertizingAgency1.Controllers
{
    public class DogovoraController : Controller
    {
        private DataManager dataManager;
        public int PageSize = 15;
        public DogovoraController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        //стартовая страница
         [Authorize(Roles = "admin,maneger,bookkeeper,designer,master,installer")]
        public ActionResult Index(int page = 1)
        {
            AfterChangeStatusOnSRequestLong();
            DogovorViewModel model = new DogovorViewModel();
            model.dogovora = dataManager.DogovorRepository.GetDogovora().OrderBy(x=>x.Номер_договора).Skip((page-1)*PageSize).Take(PageSize);
          
            model.sotrudniki = dataManager.UsersRepository.GetUsers();
            model.client = dataManager.ClientsRepository.GetClients();
            model.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            model.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.DogovorRepository.GetDogovora().Count()
            };
            return View(model); 
        }
        //метод автоматического изменения статуса на требует продления если остается 3 дня до окончания обслуживания
        private void AfterChangeStatusOnSRequestLong()
        {
           
            List<int> Iddog = new List<int>();
            //получаем список номеров договоров требующих продления
            Iddog = NumberEndDog();
            if(Iddog.Count!=0)
            { 
                foreach (int id in Iddog)
                {
                    Договора dogo = dataManager.DogovorRepository.GetDogovoraById(id);
                    dogo.Статус = "требует продления";
                    dataManager.DogovorRepository.SaveDogovora(dogo);

                }
            }
        }

        //метод получения списка договоров у которых три дня до окончания срока обслуживания
        private List<int> NumberEndDog()
        {
            List<int> Iddog = new List<int>();
            IEnumerable<Договора> SpDogovGotov = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "готово");
            foreach (Договора dogovor in SpDogovGotov)
            {
                IEnumerable<Заказы> SpZakaz =
                    dataManager.ZakazyRepository.GetZakaz().Where(x => x.Номер_договора == dogovor.Номер_договора);
                if (SpZakaz != null)
                {
                    foreach (Заказы zakaz in SpZakaz)
                    {
                        IEnumerable<АдресныеЗаказы> SpAdresnZakaz =
                            dataManager.ZakazyAdresyRepository.GetAdZakaz().Where(x => x.Код_заказа == zakaz.Код_заказа);
                        if (SpAdresnZakaz != null)
                        {
                            foreach (АдресныеЗаказы adrzak in SpAdresnZakaz)
                            {
                                string today3 = DateTime.Now.AddDays(3).ToShortDateString();
                                string today2 = DateTime.Now.AddDays(2).ToShortDateString();
                                string today1 = DateTime.Now.AddDays(1).ToShortDateString();
                                string EqualsDate = ((DateTime)adrzak.Дата_окончания).ToShortDateString();

                                if (string.Equals(today3, EqualsDate) || string.Equals(today2, EqualsDate) || string.Equals(today1, EqualsDate) || string.Equals(DateTime.Now.ToShortDateString(), EqualsDate))
                                {
                                    Iddog.Add(dogovor.Номер_договора);
                                    break;
                                }
                            }
                        }

                    }

                }

            }
            return Iddog;
        }
            
            
            [Authorize(Roles = "admin,maneger")]
        //удаление договора
        public ActionResult DeleteDogovor(int id)
        {
            dataManager.DogovorRepository.DeleteDogovor(dataManager.DogovorRepository.GetDogovoraById(id));
                //и удалим файл договора
            string pathDog = Server.MapPath("~//App_data//Dogovora//" + id+".doc");
                if (System.IO.File.Exists(pathDog))
                {
                    System.IO.File.Delete(pathDog);
                }
             
            return RedirectToAction("Index");
        }
        //создание клиента в договоре
         [Authorize(Roles = "admin,maneger")]
        [HttpGet]
        public ActionResult CreateClientForDogovor()
        {
            CreateClientForDogovorViewModel model = new CreateClientForDogovorViewModel();
            model.clientId = 0;
            return View(model);
        }
         [Authorize(Roles = "admin,maneger")]
        [HttpPost]
        public ActionResult CreateClientForDogovor(CreateClientForDogovorViewModel model)
        {
           
            if (ModelState.IsValid)
            {
                dataManager.ClientsRepository.CreateClient(model.clientId,model.clientName,model.clientUNP,model.clientAdres,"потенциальный");
                dataManager.ContaktnoeLicoRepository.CreateContactnoeLico(dataManager.ClientsRepository.GetClients().OrderBy(x=>x.Код_клиента).Last().Код_клиента , model.clientFio, model.clientPhone, model.clientEmail, model.clientSkype, model.clientPogelanie);
           return RedirectToAction("Index", "Zakaz", new { id = dataManager.ClientsRepository.GetClients().OrderBy(x => x.Код_клиента).Last().Код_клиента, newdogovor = true });
            }
            return View();
        }
        //создание договора
        [Authorize(Roles = "admin,maneger")]
        [HttpGet]
        public ActionResult CreateDogovor(int id)
        {
            Договора  dog = dataManager.DogovorRepository.GetDogovoraById(id);
            CreateDogovorViewModel model = new CreateDogovorViewModel();           
            model.dogovorZaclechen = DateTime.Now.ToShortDateString();
            model.dogovorPrimechanie = dog.Примечания;
            model.dogovorGotovnost = DateTime.Now.AddDays(5).ToShortDateString();
            model.dogovorClientId = dog.Код_клиента;
            model.dogStoimost = (int)dog.Итоговая_стоимость;
            model.IDdog = id;
            return View(model);
        }

        //создание договора
        [Authorize(Roles = "admin,maneger")]
        [HttpPost]
        public ActionResult CreateDogovor(CreateDogovorViewModel mod)
        {
            string name = System.Web.HttpContext.Current.User.Identity.Name.ToString();
            //получим по логину код сотрудника!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            int Kod =
                dataManager.UsersRepository.GetRegisterUser()
                    .Where(x => x.Логин == name)
                    .FirstOrDefault()
                    .Код_сотрудника;
            if (ModelState.IsValid)
            {
                dataManager.DogovorRepository.CreateDogovora(mod.IDdog, Kod, mod.dogovorClientId,
                    DateTime.Parse(mod.dogovorZaclechen), DateTime.Parse(mod.dogovorGotovnost), mod.dogStoimost, "к оплате",
                    mod.dogovorPrimechanie);
                //return RedirectToAction("Index","Zakaz", new { id = dataManager.DogovorRepository.GetDogovora().OrderBy(x=>x.Номер_договора).Last().Номер_договора });
                return RedirectToAction("OformitZakaz", "Zakaz", new {id = mod.IDdog});
            }
            return View(mod);

        }

   
         [Authorize(Roles = "admin,maneger")]
        [HttpGet]
        public ActionResult UpdateClients(int id)
        {
            CreateClientForDogovorViewModel model = new CreateClientForDogovorViewModel();
            Контактное_лицо klient = dataManager.ContaktnoeLicoRepository.GetContactnoeLicoById(id);
                model.clientFio = klient.ФИО;
                model.clientPhone = klient.Телефон;
                model.clientSkype = klient.Skype;
                model.clientPogelanie = klient.Пожелания;
                model.clientEmail = klient.Email;
                 model.clientId = klient.Код_клиента;
            return View(model);
        }
         [Authorize(Roles = "admin,maneger")]
        [HttpPost]
        public ActionResult UpdateClients(CreateClientForDogovorViewModel model)
        {
            if (ModelState.IsValid)
            {
                dataManager.ClientsRepository.CreateClient(model.clientId, model.clientName, model.clientUNP, model.clientAdres, "потенциальный");
                int ID =
                     dataManager.ClientsRepository.GetClients().OrderBy(x => x.Код_клиента).LastOrDefault().Код_клиента;
              
                dataManager.ContaktnoeLicoRepository.CreateContactnoeLico(ID, model.clientFio, model.clientPhone, model.clientEmail, model.clientSkype, model.clientPogelanie);
                //return RedirectToAction("CreateDogovor", new { id = ID });
                return RedirectToAction("Index", "Zakaz", new { id = ID,newdogovor = true });
          
            }
             
            return View(model);
        }
         [Authorize(Roles = "admin,maneger,bookkeeper,designer,master,installer")]
        [HttpGet]
        public ActionResult ZakazOfDogovor(int id)
        {
            ViewBag.NomDog = id;
            ZakazOfDogovorViewModel model = new ZakazOfDogovorViewModel();
            model.Zakaz = dataManager.ZakazyRepository.GetZakazDogovora(id);
            model.Uslugi = dataManager.PricesRepository.GetPrices();
            model.AdresZakaz = dataManager.ZakazyAdresyRepository.GetAdZakaz();
            model.Adresa = dataManager.AdresRepository.GetAdreses();
            return View(model);
        }

         [Authorize(Roles = "admin,installer")]
        //actions по демонтажу изделий со стендов и изменения статуса
        public ActionResult DemontagZakazFromAdres(int id)
        {
            bool fulldem = true;
            int dognom = dataManager.ZakazyRepository.GetZakazById(id).Номер_договора;
            dataManager.ZakazyAdresyRepository.DeleteAdrZakaz(dataManager.ZakazyAdresyRepository.GetAdrZakazByIdZakaza(id));
            Договора Dogovor = dataManager.DogovorRepository.GetDogovoraById(dognom);
            IEnumerable<Заказы> SpZakazov =
                dataManager.ZakazyRepository.GetZakaz().Where(z => z.Номер_договора == Dogovor.Номер_договора);
            foreach (Заказы Z in SpZakazov)
            {
                АдресныеЗаказы adz = dataManager.ZakazyAdresyRepository.GetAdrZakazByIdZakaza(Z.Код_заказа);
                if (adz != null)
                {
                    fulldem = false;
                    break;
                }
            }
            if (fulldem)
            {
                Dogovor.Статус = "полный демонтаж";
            }
            else
            {
                Dogovor.Статус = "частичный демонтаж";
            }
            dataManager.DogovorRepository.SaveDogovora(Dogovor);
            return RedirectToAction("ZakazOfDogovor", "Dogovora", new { id = dognom });
        }




         [Authorize(Roles = "admin,maneger,installer")]
        public ActionResult DelleteZakaz(int id)
        {
            int dognom = dataManager.ZakazyRepository.GetZakazById(id).Номер_договора;
            if (dataManager.ZakazyAdresyRepository.GetAdrZakazByIdZakaza(id) != null)
                dataManager.ZakazyAdresyRepository.DeleteAdrZakaz(dataManager.ZakazyAdresyRepository.GetAdrZakazByIdZakaza(id));
            dataManager.ZakazyRepository.DeleteZakaz(dataManager.ZakazyRepository.GetZakazById(id));
            return RedirectToAction("ZakazOfDogovor", "Dogovora", new { id = dognom });
        }

    //actions для вывода списка договоров по статусам (17)
        //к оплате
        [Authorize(Roles = "admin,bookkeeper")]
        public ActionResult SForPay(int page = 1)
        {
            DogovorViewModel model = new DogovorViewModel();
            model.dogovora = dataManager.DogovorRepository.GetDogovora().Where(z=>z.Статус=="к оплате").OrderBy(x => x.Номер_договора).Skip((page - 1) * PageSize).Take(PageSize);
            model.sotrudniki = dataManager.UsersRepository.GetUsers();
            model.client = dataManager.ClientsRepository.GetClients();
            model.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            model.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "к оплате").Count()
            };
            return View(model);
        }
          [Authorize(Roles = "admin,bookkeeper")]
        public ActionResult ChangeStatusOnPayed(int id)
        {
            Договора dog = dataManager.DogovorRepository.GetDogovoraById(id);
            dog.Статус = "оплачено";
            dataManager.DogovorRepository.SaveDogovora(dog);
            return RedirectToAction("SForPay");
        }


         [Authorize(Roles = "admin,maneger")]
        //оплачено
        public ActionResult SPayed(int page = 1)
        {
            DogovorViewModel model = new DogovorViewModel();
            model.dogovora = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "оплачено").OrderBy(x => x.Номер_договора).Skip((page - 1) * PageSize).Take(PageSize);
            model.sotrudniki = dataManager.UsersRepository.GetUsers();
            model.client = dataManager.ClientsRepository.GetClients();
            model.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            model.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "оплачено").Count()
            };
            return View(model);
        }
         [Authorize(Roles = "admin,maneger")]
        public ActionResult ChangeStatusOnDesign(int id)
        {
            Договора dog = dataManager.DogovorRepository.GetDogovoraById(id);
            dog.Статус = "разработка дизайна";
            dataManager.DogovorRepository.SaveDogovora(dog);
            return RedirectToAction("SPayed");
        }
        //оплачено продление
         [Authorize(Roles = "admin,maneger")]
        public ActionResult SPayedLong(int page = 1)
        {
            DogovorViewModel model = new DogovorViewModel();
            model.dogovora = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "оплачено продление").OrderBy(x => x.Номер_договора).Skip((page - 1) * PageSize).Take(PageSize);
            model.sotrudniki = dataManager.UsersRepository.GetUsers();
            model.client = dataManager.ClientsRepository.GetClients();
            model.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            model.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "оплачено продление").Count()
            };
            return View(model);
        }

         [Authorize(Roles = "admin,maneger")]
        public ActionResult ChangeStatusOnSRedyFromPayedLong(int id)
        {
            Договора dog = dataManager.DogovorRepository.GetDogovoraById(id);
            dog.Статус = "готово";
            dataManager.DogovorRepository.SaveDogovora(dog);
            return RedirectToAction("SPayedLong");
        }
        //разработка дизайна
         [Authorize(Roles = "admin,designer")]
        public ActionResult SDesigner(int page = 1)
        {
            DogovorViewModel model = new DogovorViewModel();
            model.dogovora = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "разработка дизайна").OrderBy(x => x.Номер_договора).Skip((page - 1) * PageSize).Take(PageSize);
            model.sotrudniki = dataManager.UsersRepository.GetUsers();
            model.client = dataManager.ClientsRepository.GetClients();
            model.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            model.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "разработка дизайна").Count()
            };
            return View(model);
        }
         [Authorize(Roles = "admin,designer")]
        public ActionResult ChangeStatusOnLookMaket(int id)
        {
            Договора dog = dataManager.DogovorRepository.GetDogovoraById(id);
            dog.Статус = "оценка макета";
            dataManager.DogovorRepository.SaveDogovora(dog);
            return RedirectToAction("SDesigner");
        }
        //Оценка макета
         [Authorize(Roles = "admin,maneger")]
        public ActionResult SLookMaket(int page = 1)
        {
            DogovorViewModel model = new DogovorViewModel();
            model.dogovora = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "оценка макета").OrderBy(x => x.Номер_договора).Skip((page - 1) * PageSize).Take(PageSize);
            model.sotrudniki = dataManager.UsersRepository.GetUsers();
            model.client = dataManager.ClientsRepository.GetClients();
            model.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            model.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "оценка макета").Count()
            };
            return View(model);
        }
         [Authorize(Roles = "admin,maneger")]
        public ActionResult ChangeStatusOnSForRedesign(int id)
        {
            Договора dog = dataManager.DogovorRepository.GetDogovoraById(id);
            dog.Статус = "на доработку";
            dataManager.DogovorRepository.SaveDogovora(dog);
            return RedirectToAction("SLookMaket");
        }
         [Authorize(Roles = "admin,maneger")]
  public ActionResult ChangeStatusOnSMakeOriginal(int id)
  {
      Договора dog = dataManager.DogovorRepository.GetDogovoraById(id);
      dog.Статус = "готовить оригинал";
      dataManager.DogovorRepository.SaveDogovora(dog);
      return RedirectToAction("SLookMaket");
  }





        //На доработку
         [Authorize(Roles = "admin,designer")]
        public ActionResult SForRedesign(int page = 1)
        {
            DogovorViewModel model = new DogovorViewModel();
            model.dogovora = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "на доработку").OrderBy(x => x.Номер_договора).Skip((page - 1) * PageSize).Take(PageSize);
            model.sotrudniki = dataManager.UsersRepository.GetUsers();
            model.client = dataManager.ClientsRepository.GetClients();
            model.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            model.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "на доработку").Count()
            };
            return View(model);
        }

         [Authorize(Roles = "admin,designer")]
        public ActionResult ChangeStatusOnLookMaketAfterRedesign(int id)
        {
            Договора dog = dataManager.DogovorRepository.GetDogovoraById(id);
            dog.Статус = "оценка макета";
            dataManager.DogovorRepository.SaveDogovora(dog);
            return RedirectToAction("SForRedesign");
        }

         [Authorize(Roles = "admin,designer")]
        //готовить оригинал
        public ActionResult SMakeOriginal(int page = 1)
        {
            DogovorViewModel model = new DogovorViewModel();
            model.dogovora =
                dataManager.DogovorRepository.GetDogovora()
                    .Where(z => z.Статус == "готовить оригинал")
                    .OrderBy(x => x.Номер_договора)
                    .Skip((page - 1)*PageSize)
                    .Take(PageSize);
            model.sotrudniki = dataManager.UsersRepository.GetUsers();
            model.client = dataManager.ClientsRepository.GetClients();
            model.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            model.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "готовить оригинал").Count()
            };
            return View(model);
        }

         [Authorize(Roles = "admin,designer")]
        public ActionResult ChangeStatusOnInMasters(int id)
  {
      Договора dog = dataManager.DogovorRepository.GetDogovoraById(id);
      dog.Статус = "в мастерскую";
      dataManager.DogovorRepository.SaveDogovora(dog);
      return RedirectToAction("SMakeOriginal");
  }


         [Authorize(Roles = "admin,master")]
        //в мастерскую
        public ActionResult SInMasters(int page = 1)
        {
            DogovorViewModel model = new DogovorViewModel();
            model.dogovora =
                dataManager.DogovorRepository.GetDogovora()
                    .Where(z => z.Статус == "в мастерскую")
                    .OrderBy(x => x.Номер_договора)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize);
            model.sotrudniki = dataManager.UsersRepository.GetUsers();
            model.client = dataManager.ClientsRepository.GetClients();
            model.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            model.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "в мастерскую").Count()
            };
            return View(model);
        }

         [Authorize(Roles = "admin,master")]
        public ActionResult ChangeStatusOnSRedyForSets(int id)
  {
      Договора dog = dataManager.DogovorRepository.GetDogovoraById(id);
      dog.Статус = "готов к установке";
      dataManager.DogovorRepository.SaveDogovora(dog);
      return RedirectToAction("SInMasters");
  }

         [Authorize(Roles = "admin,installer")]
        //готов к установке
        public ActionResult SRedyForSets(int page = 1)
        {
            DogovorViewModel model = new DogovorViewModel();
            model.dogovora =
                dataManager.DogovorRepository.GetDogovora()
                    .Where(z => z.Статус == "готов к установке")
                    .OrderBy(x => x.Номер_договора)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize);
            model.sotrudniki = dataManager.UsersRepository.GetUsers();
            model.client = dataManager.ClientsRepository.GetClients();
            model.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            model.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "готов к установке").Count()
            };
            return View(model);
        }
         [Authorize(Roles = "admin,installer")]
        public ActionResult ChangeStatusOnSSets(int id)
        {
            Договора dog = dataManager.DogovorRepository.GetDogovoraById(id);
            dog.Статус = "установлено";
            dataManager.DogovorRepository.SaveDogovora(dog);
            return RedirectToAction("SRedyForSets");
        }

         [Authorize(Roles = "admin,maneger")]
//установлено
        public ActionResult SSets(int page = 1)
        {
            DogovorViewModel model = new DogovorViewModel();
            model.dogovora =
                dataManager.DogovorRepository.GetDogovora()
                    .Where(z => z.Статус == "установлено")
                    .OrderBy(x => x.Номер_договора)
                    .Skip((page - 1)*PageSize)
                    .Take(PageSize);
            model.sotrudniki = dataManager.UsersRepository.GetUsers();
            model.client = dataManager.ClientsRepository.GetClients();
            model.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            model.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "установлено").Count()
            };
            return View(model);
        }

         [Authorize(Roles = "admin,maneger")]
        public ActionResult ChangeStatusOnSRedy(int id)
         {
             bool da = false;
            Договора dog = dataManager.DogovorRepository.GetDogovoraById(id);
             IEnumerable<Заказы> z = dataManager.ZakazyRepository.GetZakazDogovora(id);
             foreach (var s in z)
             {
                 АдресныеЗаказы adr = dataManager.ZakazyAdresyRepository.GetAdZakaz().Where(x => x.Код_заказа == s.Код_заказа).FirstOrDefault();
                 if (adr != null)
                 {
                     da = true;
                     break;
                 }
             }
             if (da)
             { dog.Статус = "готово"; }
             else
             { dog.Статус = "закрыто"; }
           
            dataManager.DogovorRepository.SaveDogovora(dog);
            return RedirectToAction("SSets");
        }
         [Authorize(Roles = "admin,maneger")]
        //готово
        public ActionResult SRedy(int page = 1)
        {
            DogovorViewModel model = new DogovorViewModel();
            model.dogovora =
                dataManager.DogovorRepository.GetDogovora()
                    .Where(z => z.Статус == "готово")
                    .OrderBy(x => x.Номер_договора)
                    .Skip((page - 1)*PageSize)
                    .Take(PageSize);
            model.sotrudniki = dataManager.UsersRepository.GetUsers();
            model.client = dataManager.ClientsRepository.GetClients();
            model.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            model.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "готово").Count()
            };
            return View(model);
        }
         [Authorize(Roles = "admin,maneger")]
        public ActionResult ChangeStatusOnCloseifRedy(int id)
        {
            Договора dog = dataManager.DogovorRepository.GetDogovoraById(id);
            IEnumerable<Заказы> zak =
                dataManager.ZakazyRepository.GetZakaz().Where(x => x.Номер_договора == dog.Номер_договора);
            bool adrzakaz=false;
            IEnumerable<АдресныеЗаказы> spisadrzak = dataManager.ZakazyAdresyRepository.GetAdZakaz();
            foreach (АдресныеЗаказы adz in spisadrzak)
            {
                foreach (Заказы z in zak)
                {
                    if (z.Код_заказа == adz.Код_заказа)
                    {
                        adrzakaz = true;
                        break;
                    }
                }
            }
            if(!adrzakaz)
            { 
                dog.Статус = "закрыто";
                dataManager.DogovorRepository.SaveDogovora(dog);  
            }
            return RedirectToAction("SRedy");
        }
        //требует продления--------------------------------------------------------------высчитывать автоматически количество договоров по дате
       [Authorize(Roles = "admin,maneger")]
        public ActionResult SRequestLong(int page = 1)
        {
            DogovorViewModel model = new DogovorViewModel();
            model.dogovora =
                dataManager.DogovorRepository.GetDogovora()
                    .Where(z => z.Статус == "требует продления")
                    .OrderBy(x => x.Номер_договора)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize);
            model.sotrudniki = dataManager.UsersRepository.GetUsers();
            model.client = dataManager.ClientsRepository.GetClients();
            model.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            model.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "требует продления").Count()
            };
            return View(model);
        }


         [Authorize(Roles = "admin,maneger")]
        public ActionResult ChangeStatusOnSRedyToDestroy(int id)
        {
            Договора dog = dataManager.DogovorRepository.GetDogovoraById(id);
            dog.Статус = "готов к демонтажу";
            dataManager.DogovorRepository.SaveDogovora(dog);
            return RedirectToAction("SRequestLong");
        }
        //готов к демонтажу
       [Authorize(Roles = "admin,installer")]
        public ActionResult SRedyToDestroy(int page = 1)
        {
            DogovorViewModel model = new DogovorViewModel();
            model.dogovora =
                dataManager.DogovorRepository.GetDogovora()
                    .Where(z => z.Статус == "готов к демонтажу")
                    .OrderBy(x => x.Номер_договора)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize);
            model.sotrudniki = dataManager.UsersRepository.GetUsers();
            model.client = dataManager.ClientsRepository.GetClients();
            model.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            model.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.DogovorRepository.GetDogovora().Count()
            };
            return View(model);
        }
        //частично демонтирован
         [Authorize(Roles = "admin,maneger")]
        public ActionResult SPartDestroy(int page = 1)
        {
            DogovorViewModel model = new DogovorViewModel();
            model.dogovora =
                dataManager.DogovorRepository.GetDogovora()
                    .Where(z => z.Статус == "частичный демонтаж")
                    .OrderBy(x => x.Номер_договора)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize);
            model.sotrudniki = dataManager.UsersRepository.GetUsers();
            model.client = dataManager.ClientsRepository.GetClients();
            model.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            model.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "частичный демонтаж").Count()
            };
            return View(model);
        }
         [Authorize(Roles = "admin,maneger")]
        public ActionResult ChangeStatusOnSRedyFromDemontag(int id)
        {
            Договора dog = dataManager.DogovorRepository.GetDogovoraById(id);
            dog.Статус = "готово";
            dataManager.DogovorRepository.SaveDogovora(dog);
            return RedirectToAction("SPartDestroy");
        }
         [Authorize(Roles = "admin,maneger")]
        //демонтирован полностью
        public ActionResult SFullDestroy(int page = 1)
        {
            DogovorViewModel model = new DogovorViewModel();
            model.dogovora =
                dataManager.DogovorRepository.GetDogovora()
                    .Where(z => z.Статус == "полный демонтаж")
                    .OrderBy(x => x.Номер_договора)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize);
            model.sotrudniki = dataManager.UsersRepository.GetUsers();
            model.client = dataManager.ClientsRepository.GetClients();
            model.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            model.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "полный демонтаж").Count()
            };
            return View(model);
        }
         [Authorize(Roles = "admin,maneger")]
        public ActionResult ChangeStatusOnSCloseFromDemontag(int id)
        {
            Договора dog = dataManager.DogovorRepository.GetDogovoraById(id);
            dog.Статус = "закрыто";
            dataManager.DogovorRepository.SaveDogovora(dog);
            return RedirectToAction("SFullDestroy");
        }
         [Authorize(Roles = "admin,maneger")]
        //закрыто
        public ActionResult SClose(int page = 1)
        {
            DogovorViewModel model = new DogovorViewModel();
            model.dogovora =
                dataManager.DogovorRepository.GetDogovora()
                    .Where(z => z.Статус == "закрыто")
                    .OrderBy(x => x.Номер_договора)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize);
            model.sotrudniki = dataManager.UsersRepository.GetUsers();
            model.client = dataManager.ClientsRepository.GetClients();
            model.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            model.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "закрыто").Count()
            };
            return View(model);
        }
       [Authorize(Roles = "admin,bookkeeper")]
        //к оплате на продление
        public ActionResult SToPayForLong(int page = 1)
        {
            DogovorViewModel model = new DogovorViewModel();
            model.dogovora =
                dataManager.DogovorRepository.GetDogovora()
                    .Where(z => z.Статус == "к оплате на продление")
                    .OrderBy(x => x.Номер_договора)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize);
            model.sotrudniki = dataManager.UsersRepository.GetUsers();
            model.client = dataManager.ClientsRepository.GetClients();
            model.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            model.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.DogovorRepository.GetDogovora().Where(z => z.Статус == "к оплате на продление").Count()
            };
            return View(model);
        }

         [Authorize(Roles = "admin,bookkeeper")]
        public ActionResult ChangeStatusOnSPayedLong(int id)
        {
            Договора dog = dataManager.DogovorRepository.GetDogovoraById(id);
            dog.Статус = "оплачено продление";
            dataManager.DogovorRepository.SaveDogovora(dog);
            return RedirectToAction("SToPayForLong");
        }



        //------------------------------------------метод продления срока обслуживания реализован в качестве триггера в БД ----------
        //action по изменению данных продления
        [Authorize(Roles = "admin,maneger")]
        [HttpGet]
        public ActionResult ToLongServiceAdresZakaz(int id)
        {

            АдресныеЗаказы adr = dataManager.ZakazyAdresyRepository.GetAdrZakazByIdZakaza(id);
            ToLongAdresnZakazViewModel model = new ToLongAdresnZakazViewModel()
            {
                DateFinish = adr.Дата_окончания.ToString(),
                KodZ = adr.Код_заказа,
                 KodA = adr.Код_адреса,
                 Srok = (int)adr.Срок_продления,
                 StoimostProdl = (int)adr.Стоимость_продления,
                 StoimostRazm = (int)adr.Стоимость_размещения
            };
            return View(model);
        }
         [Authorize(Roles = "admin,maneger")]
        [HttpPost]
        public ActionResult ToLongServiceAdresZakaz(ToLongAdresnZakazViewModel model)
        {
           dataManager.ZakazyAdresyRepository.ChangeAdreZakazProdlen(model.KodZ,model.KodA,model.DateFinish,model.StoimostRazm);
           return RedirectToAction("ToLongServiceAdresZakaz", new { id = model.KodZ});
        }
    }
}
