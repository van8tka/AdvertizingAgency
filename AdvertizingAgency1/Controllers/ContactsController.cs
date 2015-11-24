using System.Web.Mvc;
using System.Xml.Linq;
using AdvertizingAgency1.Models;
using BusinessLogic;

namespace AdvertizingAgency1.Controllers
{
    public class ContactsController : Controller
    {
        private DataManager dataManager;
        public ContactsController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        [HttpGet]
        public ActionResult Index()
        {
            ContactsCompanyViewModel model = new ContactsCompanyViewModel();
            string path = Server.MapPath("~//App_Data//AboutCompany.xml");
            model = AdvertizingAgency1.AboutCompany.AboutCompany.FromXML(path);
            return View(model);
        }
         [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult ChangeData()
        {
            ContactsCompanyViewModel model = new ContactsCompanyViewModel();
            string path = Server.MapPath("~//App_Data//AboutCompany.xml");
            model = AdvertizingAgency1.AboutCompany.AboutCompany.FromXML(path);
            return View(model);
        }
         [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult ChangeData(ContactsCompanyViewModel model)
        {
             if(ModelState.IsValid)
             { 
            CreateInXML(model.Email, model.Rsc, model.Pasw);
            return RedirectToAction("Index");
             }
             return View(model);
        }


     
        //метод сохранения в файле XML данных компании
        public void CreateInXML(string email, string RashetnSchet,string passw)
        {
          //создали контекст объекта
             
                XDocument Doc = new XDocument();
          
                XElement RshCompany = new XElement("RshCompany");
                XAttribute NameRsc = new XAttribute("nameRsc", "RscCompanyForWork");
                XElement RscValue = new XElement("Rsc", RashetnSchet);
                XElement EmailValue = new XElement("Em", email);
                XElement ParolValue = new XElement("Pasw", passw);
                RshCompany.Add(NameRsc);
                RshCompany.Add(RscValue);
                RshCompany.Add(EmailValue);
                RshCompany.Add(ParolValue);
            // создаем корневой элемент
                XElement about = new XElement("about");
                // добавляем в корневой элемент
                about.Add(RshCompany);
                //about.Add(EmailCompany);
                // добавляем корневой элемент в документ
                Doc.Add(about);
                string path = Server.MapPath("~//App_Data//AboutCompany.xml");
           
                //сохраняем документ
                if(path!=null)
                { 
                    Doc.Save(path);
                }   
            }
           
        }

    } 

