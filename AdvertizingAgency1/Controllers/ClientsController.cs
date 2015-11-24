using System.Linq;
using System.Web.Mvc;
using AdvertizingAgency1.Models;
using BusinessLogic;
using DAL;

namespace AdvertizingAgency1.Controllers
{
    public class ClientsController : Controller
    {
        private DataManager dataManager;
        public int PageSize = 15;

        public ClientsController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
         [Authorize(Roles = "admin,maneger,bookkeeper,designer,master,installer")]
        public ActionResult Index(int page = 1)
        {
            ClientsViewModel allclients = new ClientsViewModel();
            allclients.client = dataManager.ClientsRepository.GetClients().Skip((page-1)*PageSize).Take(PageSize);
            allclients.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            allclients.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.ClientsRepository.GetClients().Where(x => x.Статус != "новый").Count()
            };
            return View(allclients);
        }
         [Authorize(Roles = "admin,maneger")]
        public ActionResult NewClients(int page=1)
        {
            ClientsViewModel allclients = new ClientsViewModel();
            allclients.client = dataManager.ClientsRepository.GetClients();
            allclients.contactnoeLico = dataManager.ContaktnoeLicoRepository.GetContactnoeLico();
            allclients.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataManager.ClientsRepository.GetClients().Where(x => x.Статус == "новый").Count()
            };
            return View(allclients);
        }

          [Authorize(Roles = "admin,maneger")]
        [HttpGet]
        public ActionResult EditClient(int id)
        {
              CreateClientForDogovorViewModel model = new CreateClientForDogovorViewModel();
              Клиенты client = dataManager.ClientsRepository.GetClientsById(id);
              Контактное_лицо contact = dataManager.ContaktnoeLicoRepository.GetContactnoeLicoById(id);
              model.clientId = id;
              model.clientName = client.Наименование;
              model.clientUNP = (int)client.УНП;
              model.clientAdres = client.Адрес;
              model.clientEmail = contact.Email;
              model.clientFio = contact.ФИО;
              model.clientPhone = contact.Телефон;
              model.clientSkype = contact.Skype;
              model.clientPogelanie = contact.Пожелания;
         return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin,maneger")]
          public ActionResult EditClient(CreateClientForDogovorViewModel model)
        {
            if (ModelState.IsValid)
            {
                dataManager.ClientsRepository.CreateClient(model.clientId,model.clientName,model.clientUNP,model.clientAdres,dataManager.ClientsRepository.GetClientsById(model.clientId).Статус);
                int ID =
                    dataManager.ClientsRepository.GetClients().OrderBy(x => x.Код_клиента).LastOrDefault().Код_клиента;
                dataManager.ContaktnoeLicoRepository.CreateContactnoeLico(ID,model.clientFio,model.clientPhone,model.clientEmail,model.clientSkype,model.clientPogelanie);
              
                return RedirectToAction("Index");
            }
            return View(model);
       }
          [Authorize(Roles = "admin,maneger")]
        [HttpGet]
        public ActionResult DeleteClients(int id)
        {
            dataManager.ClientsRepository.DeleteClient(dataManager.ClientsRepository.GetClientsById(id));
            return RedirectToAction("Index");
        }

    }
}
