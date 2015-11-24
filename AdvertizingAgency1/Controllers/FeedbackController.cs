using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvertizingAgency1.Models;
using BusinessLogic;

namespace AdvertizingAgency1.Controllers
{
    public class FeedbackController : Controller
    {
        private DataManager dataManager;

        public FeedbackController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        [HttpGet]
        public ActionResult Index()
        {
            CreateClientForDogovorViewModel model = new CreateClientForDogovorViewModel();
            model.clientAdres = "отсутствует";
            model.clientUNP = 100000000;
            model.clientName = "отсутствует";

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CreateClientForDogovorViewModel model)
        {
            if (ModelState.IsValid)
            {
                dataManager.ClientsRepository.CreateClient(0, model.clientName, model.clientUNP, model.clientAdres,"новый");
               int id = dataManager.ClientsRepository.GetClients().OrderBy(x => x.Код_клиента).Last().Код_клиента;                   
                dataManager.ContaktnoeLicoRepository.CreateContactnoeLico(id, model.clientFio, model.clientPhone,
                model.clientEmail, model.clientSkype, model.clientPogelanie);
                return RedirectToAction("Answer");
            }
            return View();
        }

        public ActionResult Answer()
        {
            return View();
        }
    }
}
