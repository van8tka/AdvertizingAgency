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
    public class OurWorksController : Controller
    {
         private DataManager dataManager;

         public OurWorksController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
     public ActionResult Index()
        {      
           DownloadFilesViewModel model = new DownloadFilesViewModel();
           string domainpath = Server.MapPath("~/Content/OurWorks");
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
        //удаление файлов наши работы
        [HttpGet]
        public ActionResult DeleteOurWorks(string id, string file)
        {
            string filePath = Server.MapPath("~/Content/OurWorks/"+file);
                System.IO.File.Delete(filePath);
            return RedirectToAction("Index");
        }
         [Authorize(Roles = "admin")]
        //загрузка файлов Наши работы
        [HttpGet]
        public ActionResult AddFileWorks()
        {
           return View();
        }
         [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult AddFileWorks(IEnumerable<HttpPostedFileBase> fileUpload, string id)
        {
           
            string domainpath = Server.MapPath("~/Content/OurWorks/" );
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
            return RedirectToAction("Index", "OurWorks");
        }
    }
}
