using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvertizingAgency1.Models;
using BusinessLogic;

namespace AdvertizingAgency1.Controllers
{
    public class ResponsesController : Controller
    {
       private DataManager dataManager;

       public ResponsesController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

       public ActionResult Index()
       {
           DownloadFilesViewModel model = new DownloadFilesViewModel();
           string domainpath = Server.MapPath("~/Content/Responses");
           var dir = new DirectoryInfo(domainpath);
           FileInfo[] fileNames = dir.GetFiles("*.*");
           List<string> item = new List<string>();
           foreach (var file in fileNames)
           {
               item.Add(file.Name);
           }
           model.FileList = item;
           return View(model);
       }
         [Authorize(Roles = "admin")]
       //удаление файлов отзывы
       [HttpGet]
       public ActionResult DeleteResponse(string id, string file)
       {
           string filePath = Server.MapPath("~/Content/Responses/" + file);
           System.IO.File.Delete(filePath);
           return RedirectToAction("Index");
       }
       //загрузка файлов отзывы
         [Authorize(Roles = "admin")]
       [HttpGet]
       public ActionResult AddFileResponse()
       {
           return View();
       }
         [Authorize(Roles = "admin")]
       [HttpPost]
       public ActionResult AddFileResponse(IEnumerable<HttpPostedFileBase> fileUpload, string id)
       {

           string domainpath = Server.MapPath("~/Content/Responses/");
           if (!Directory.Exists(domainpath))
               Directory.CreateDirectory(domainpath);
           foreach (var file in fileUpload)
           {
               if (file != null && file.ContentLength > 0)
               {
                   string fileName = Path.GetFileName(file.FileName);
                   string path = Path.Combine(domainpath, fileName);
                   file.SaveAs(path);
               }
           }
           return RedirectToAction("Index");
       }


    }
}
