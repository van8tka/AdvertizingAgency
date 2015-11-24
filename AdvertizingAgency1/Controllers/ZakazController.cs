using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using AdvertizingAgency1.AboutCompany;
using AdvertizingAgency1.Models;
using BusinessLogic;
using Word = Microsoft.Office.Interop.Word;
using DAL;


namespace AdvertizingAgency1.Controllers
{
     [Authorize(Roles = "admin,maneger,bookkeeper,designer,master,installer")]
    public class ZakazController : Controller
    {
        private DataManager dataManager;
       
        public ZakazController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
           [HttpGet]
        public ActionResult Index(int id, bool newdogovor)
        {
            //создадим пустой договор
            if (newdogovor)
            {
                string name = System.Web.HttpContext.Current.User.Identity.Name.ToString();
                //получим по логину код сотрудника!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                int Kod = dataManager.UsersRepository.GetRegisterUser().Where(x => x.Логин == name).FirstOrDefault().Код_сотрудника;

                dataManager.DogovorRepository.CreateDogovora(0, Kod, id, DateTime.Now, DateTime.Now.AddDays(3), 0, "к оплате", "нет");
            }

            CreateZakazViewModel model = new CreateZakazViewModel();
            model.NumberOfDogovor = dataManager.DogovorRepository.GetDogovora().OrderBy(x => x.Номер_договора).Last().Номер_договора;
            //заполняем список для выпадающего списка

               model.SpisokType = from usl in dataManager.PricesRepository.GetPrices().OrderBy(x => x.Тип).GroupBy(t => t.Тип)
                                  select new SelectListItem { Text = usl.Key, Value = usl.Key };
      return View(model);
        }




        [HttpPost]
        public ActionResult Index(CreateZakazViewModel model)
        {
            if (ModelState.IsValid)
            {
                dataManager.ZakazyRepository.CreateZakaz(0, model.uslId, model.NumberOfDogovor, model.Cost);
                return RedirectToAction("ZakazOfDogovor", "Zakaz", new { id = model.NumberOfDogovor });
            }
            model.SpisokUslug = from usl in dataManager.PricesRepository.GetPrices().OrderBy(x => x.Наименование)
                                select new SelectListItem { Text = usl.Наименование, Value = usl.Код_услуги.ToString() };
            return View(model);
        }

        [HttpGet]
        //action для возврата частичного представления (jquery)
        public ActionResult UslugiType(string id)
        {
            CreateZakazViewModel model = new CreateZakazViewModel();
           model.SpisokUslug = from usl in dataManager.PricesRepository.GetPrices().OrderBy(x => x.Наименование).Where(z=>z.Тип==id)
                               select new SelectListItem { Text = usl.Наименование, Value = usl.Код_услуги.ToString() };
          
            return View(model);
        }


        [HttpGet]
        //action для возврата частичного представления (jquery)
        public ActionResult CostResult(int id)
        {
            CreateZakazViewModel model = new CreateZakazViewModel();
            model.Cost = (int)dataManager.PricesRepository.GetPricesById(id).Цена;
            return View(model);
        }



        [HttpGet]
        public ActionResult ZakazOfDogovor(int id)
        {
            ViewBag.NomDog = id;
            ZakazOfDogovorViewModel model = new ZakazOfDogovorViewModel();
            model.Zakaz = dataManager.ZakazyRepository.GetZakazDogovora(id);
            model.Uslugi = dataManager.PricesRepository.GetPrices();
            model.AdresZakaz = dataManager.ZakazyAdresyRepository.GetAdZakaz();
            model.IDClient = dataManager.DogovorRepository.GetDogovoraById(id).Код_клиента;
            model.Adresa = dataManager.AdresRepository.GetAdreses();
            return PartialView(model);
        }


          [Authorize(Roles = "admin,maneger,installer")]
        public ActionResult DelleteZakaz(int id)
        {
            int dognom = dataManager.ZakazyRepository.GetZakazById(id).Номер_договора;
            if (dataManager.ZakazyAdresyRepository.GetAdrZakazByIdZakaza(id)!=null)
                dataManager.ZakazyAdresyRepository.DeleteAdrZakaz(dataManager.ZakazyAdresyRepository.GetAdrZakazByIdZakaza(id));
            dataManager.ZakazyRepository.DeleteZakaz(dataManager.ZakazyRepository.GetZakazById(id));
            return RedirectToAction("ZakazOfDogovor", "Zakaz", new { id = dognom});
        }
          [Authorize(Roles = "admin,maneger")]
        [HttpGet]
        public ActionResult AddAdresInZakaz(int id)
        {
            CreateAdresForZakaz model = new CreateAdresForZakaz();
            model.NumberOfZakaz = id;
            model.NumberOfDog = dataManager.ZakazyRepository.GetZakazById(id).Номер_договора;
            
            model.SpisokAdresov = from adrzak in dataManager.AdresRepository.GetAdreses().OrderBy(x => x.Адрес)
                select new SelectListItem {Text = adrzak.Адрес, Value = adrzak.Код_адреса.ToString()};
            return View(model);
        }
          [Authorize(Roles = "admin,maneger")]
        [HttpPost]
        public ActionResult AddAdresInZakaz(CreateAdresForZakaz model)
        {
            if (ModelState.IsValid)
            {
                dataManager.ZakazyAdresyRepository.CreateAdrZakaz(model.NumberOfZakaz, model.AdresId, model.AdresDateEnd);
                return RedirectToAction("ZakazOfDogovor", "Zakaz", new { id = dataManager.ZakazyRepository.GetZakazById(model.NumberOfZakaz).Номер_договора });
            }
            model.SpisokAdresov = from adrzak in dataManager.AdresRepository.GetAdreses().OrderBy(x => x.Адрес)
                                  select new SelectListItem { Text = adrzak.Адрес, Value = adrzak.Код_адреса.ToString() };
            return View(model);
        }

     // action for change cost adres, return partial view (jqery)
     
        public ActionResult CostResultAdres(int id)
        {
            CreateAdresForZakaz model = new CreateAdresForZakaz();
            model.AdresCost = (int) dataManager.AdresRepository.GetAdresesById(id).Цена_на_месяц;
             return PartialView(model);
        }
          [Authorize(Roles = "admin,maneger")]
        // в данном action создаем письмо для отправки на email клиенту
        [HttpGet]
        public ActionResult OformitZakaz(int id)
        {
            if (Internet.CheckConnection())
            {
                //создание документа Word на основе шаблона
                Договора dogov = dataManager.DogovorRepository.GetDogovora().OrderBy(x => x.Номер_договора).Last();
               ClassLog.Write("Вызов метода создания файла договора");
                CreateDoc(dogov.Номер_договора);

                MailMessage message = new MailMessage();
                string adresKlient =
                    dataManager.ContaktnoeLicoRepository.GetContactnoeLicoById(
                        dataManager.DogovorRepository.GetDogovoraById(id).Код_клиента).Email;
                string fioKlient = dataManager.ContaktnoeLicoRepository.GetContactnoeLicoById(
                            dataManager.DogovorRepository.GetDogovoraById(id).Код_клиента).ФИО;
                message.To.Add(adresKlient);
                string path = Server.MapPath("~//App_Data//AboutCompany.xml");
                ContactsCompanyViewModel model = AdvertizingAgency1.AboutCompany.AboutCompany.FromXML(path);

                message.From = new System.Net.Mail.MailAddress(model.Email, "Рекламнок агетство наружной рекламы");

                message.Subject = "Договор на предоставление услуг рекламы"; // указание темы письма
                message.BodyEncoding = System.Text.Encoding.UTF8; // указание кодировки письма
                message.IsBodyHtml = false; // указание формата письма (true - HTML, false - не HTML)
                string NameFile = id + ".doc";
                string file = Server.MapPath("~//App_data//Dogovora//" + NameFile);
                if (System.IO.File.Exists(file))
                {
                    message.Body = "Здраствуйте уважаемый(ая) " + fioKlient +
                                   ", это письмо отправлено Вам, потому что вы являетесь клиентом нашей компании и заключили с нами договор на предоставление услуг рекламы. Копия договора со всеми необходимыми реквизитами  прикреплена к письму в виде файла.\n\n С наилучшими пожеланиями, Руководство компании!";
                    // указание текста (тела) письма       
                    Attachment attach = new Attachment(file);
                    message.Attachments.Add(attach);
                }
                else
                {
                    Сотрудники sotr = dataManager.UsersRepository.GetUsersById(dogov.Код_сотрудника);
                    message.Body = "Здраствуйте уважаемый(ая) " + fioKlient + ", это письмо отправлено Вам, потому что вы являетесь клиентом нашей компании и заключили с нами договор на предоставление услуг рекламы. Копия договора со всеми необходимыми реквизитами  прикреплена к письму в виде файла. Если файла с договором нет, обратитесь пожалуйста к своему менеджеру " + sotr.ФИО_сотрудника + " по телефону: " + sotr.Телефон + " \n \nС наилучшими пожеланиями, Руководство компании!"; // указание текста (тела) письма
                }


                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                // создание нового подключения к серверу "smtp.domain.tld"
                client.DeliveryMethod = SmtpDeliveryMethod.Network; // определяет метод отправки сообщений
                client.EnableSsl = true; // отключает необходимость использования защищенного соединения с сервером
                client.UseDefaultCredentials = false; // отключение использования реквизитов авторизации "по-умолчанию"
                // указание нужных реквизитов (имени пользователя и пароля) для авторизации на SMTP-сервере
                client.Credentials = new NetworkCredential(model.Email, model.Pasw);
                // указание нужных реквизитов (имени пользователя и пароля) для авторизации на SMTP-сервере

                client.Send(message); // отправка сообщения           
                return View();
            }
            else
            {
                return RedirectToAction("FailedSendMessage");
            }
                      
        }

         public ActionResult FailedSendMessage()
         {
             return View();
         }



         //метод создания документа из шаблона
         public void CreateDoc(int id)
         {
    //для работы с Word
             ClassLog.Write("1 вход в метод для работы с Word");
             string NameFile = id + ".doc";
             ClassLog.Write("2имя файла:"+NameFile);
             string path = Server.MapPath("~//App_data//DogovorTemp.dotx");
             ClassLog.Write("3 путь к шаблону:"+path);
             string pathDog = Server.MapPath("~//App_data//Dogovora//" + NameFile);
             ClassLog.Write("4 Full path:"+pathDog);
             if (path != null)
             {
                 ClassLog.Write("5 Create new word application");
                 Word._Application oWord = new Word.Application();
                 ClassLog.Write("6 Create new word document");
                 Word._Document oDoc;
                 try
                 {
                     ClassLog.Write("7 Open word document witch path = path to templace");
                     oDoc = oWord.Documents.Open(FileName: path);
                     if (oDoc != null)
                     {
                         ClassLog.Write("8 Open method create document with template");
                         SetTemplate(oDoc);
                         ClassLog.Write("9 Open method save new document word");
                         oDoc.SaveAs(FileName: pathDog);
                         ClassLog.Write("10 Closed document template");
                         oDoc.Close();
                     }
                     else
                     {
                         ClassLog.Write("7.1 NOT OPENED TEMPLATES");
                         oDoc.Close();
                     }
                 }
                 catch (Exception er)
                 {
                     ClassLog.Write(er);
                     oWord.Quit();
                 }
                 ClassLog.Write("Выполняется в любом случае!!!!!!!!!!!!!!!!!!!!!!!!!закрытие word app");              
                 oWord.Quit();
             }
         }

         //метод заполнения данными шаблона
        private void SetTemplate(Word._Document oDoc)
        {
            ClassLog.Write("8.1 заполняем данными шаблон");
           string path = Server.MapPath("~//App_Data//AboutCompany.xml");
           ClassLog.Write("8.2 получаем путь к xml файлу:"+path);
         ContactsCompanyViewModel model = AdvertizingAgency1.AboutCompany.AboutCompany.FromXML(path);

        Договора dogov = dataManager.DogovorRepository.GetDogovora().OrderBy(x => x.Номер_договора).Last();
            Контактное_лицо cont = dataManager.ContaktnoeLicoRepository.GetContactnoeLicoById(dogov.Код_клиента);
            Сотрудники sotr = dataManager.UsersRepository.GetUsersById(dogov.Код_сотрудника);
            ClassLog.Write("8.3 заполняем по закладкам имеющиеся данные:" + "1 " + cont.ФИО + "2 " + dogov.Итоговая_стоимость.ToString() + "3 " + sotr.ФИО_сотрудника + "4 " + DateTime.Now.ToShortDateString() + "5 " + model.Rsc);
            oDoc.Bookmarks["Klient"].Range.Text = cont.ФИО;
            oDoc.Bookmarks["Summa"].Range.Text = dogov.Итоговая_стоимость.ToString();
            oDoc.Bookmarks["Menedger"].Range.Text = sotr.ФИО_сотрудника;
            oDoc.Bookmarks["Date"].Range.Text = DateTime.Now.ToShortDateString();
            oDoc.Bookmarks["Rsc"].Range.Text = model.Rsc;    
            ClassLog.Write("8.4 выход из метода");
        }
  
         [Authorize(Roles = "admin,maneger")]
        [HttpPost]
        public ActionResult OformitZakaz()
        {
            return View();
        }


    }
}
