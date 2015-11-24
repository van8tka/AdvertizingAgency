using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvertizingAgency1.Models;
using BusinessLogic;
using DAL;

namespace AdvertizingAgency1.Controllers
{
    public class RazrabotkiController : Controller
    {
        private DataManager dataManager;

        public RazrabotkiController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
         [Authorize(Roles = "admin,designer")]
        //загрузка файлов макета
        [HttpGet]
        public ActionResult AddFileMakets(int id)
        {
            ViewBag.Dogovor = id;
            return View(id);
        }
         [Authorize(Roles = "admin,designer")]
        [HttpPost]
        public ActionResult AddFileMakets(IEnumerable<HttpPostedFileBase> fileUpload, string id)
        {
            string dogovor = id;
            string domainpath = Server.MapPath("~/UploadFiles/Makets/" + dogovor);
            if (!Directory.Exists(domainpath))
                Directory.CreateDirectory(domainpath); // Создаем директорию, если нужно
            Разработки existRazrab = dataManager.RazrabotkiRepository.GetRazrabotkiById(int.Parse(id));
            if (existRazrab != null)
            {
                dataManager.RazrabotkiRepository.ChangeRazrabotki(int.Parse(id), domainpath, existRazrab.Оригинал , existRazrab.Отчёт);
            }
            else
            {
                dataManager.RazrabotkiRepository.CreateRazrabotki(int.Parse(id), domainpath, "нет", "нет");
            }
        
            foreach (var file in fileUpload)
            {
                if (file!= null && file.ContentLength>0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string path = Path.Combine(domainpath, fileName);
                    file.SaveAs(path);
                }
            }     
            return RedirectToAction("SDesigner","Dogovora");
        }
//загрузка файлов оригинала

         [Authorize(Roles = "admin,designer")]
        [HttpGet]
        public ActionResult AddFileOriginal(int id)
        {
            ViewBag.Dogovor = id;
            return View(id);
        }
         [Authorize(Roles = "admin,designer")]
        [HttpPost]
        public ActionResult AddFileOriginal(IEnumerable<HttpPostedFileBase> fileUpload, string id)
        {
            string dogovor = id;
            string domainpath = Server.MapPath("~/UploadFiles/Original/" + dogovor);
            if (!Directory.Exists(domainpath))
                Directory.CreateDirectory(domainpath); // Создаем директорию, если нужно


            Разработки existRazrab = dataManager.RazrabotkiRepository.GetRazrabotkiById(int.Parse(id));
            if (existRazrab != null)
            {
                dataManager.RazrabotkiRepository.ChangeRazrabotki(int.Parse(id), existRazrab.Макет, domainpath, existRazrab.Отчёт);
            }
            else
            {
                dataManager.RazrabotkiRepository.CreateRazrabotki(int.Parse(id), "нет", domainpath, "нет");
            }
          
            foreach (var file in fileUpload)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string path = Path.Combine(domainpath, fileName);
                    file.SaveAs(path);
                }
            }
            return RedirectToAction("SMakeOriginal", "Dogovora");
        }

        //загрузка файлов отчёта установки

         [Authorize(Roles = "admin,installer")]
        [HttpGet]
        public ActionResult AddFileOtchets(int id)
        {
            ViewBag.Dogovor = id;
            return View(id);
        }
         [Authorize(Roles = "admin,installer")]
        [HttpPost]
        public ActionResult AddFileOtchets(IEnumerable<HttpPostedFileBase> fileUpload, string id)
        {
            string dogovor = id;
            string domainpath = Server.MapPath("~/UploadFiles/Otchets/" + dogovor);
            if (!Directory.Exists(domainpath))
                Directory.CreateDirectory(domainpath); // Создаем директорию, если нужно

            Разработки existRazrab = dataManager.RazrabotkiRepository.GetRazrabotkiById(int.Parse(id));
            if(existRazrab!=null)
            {
                dataManager.RazrabotkiRepository.ChangeRazrabotki(int.Parse(id), existRazrab.Макет,existRazrab.Оригинал , domainpath);
            }
            else
            {
                dataManager.RazrabotkiRepository.CreateRazrabotki(int.Parse(id),"нет","нет",domainpath);
            }
           
            foreach (var file in fileUpload)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string path = Path.Combine(domainpath, fileName);
                    file.SaveAs(path);
                }
            }
            return RedirectToAction("SRedyForSets", "Dogovora");
        }
         [Authorize(Roles = "admin,maneger")]
        //скачиваем файлы  макета на компьютер с сервера
        [HttpGet]
        public ActionResult DownloadsMakets(int id)
        {
            ViewBag.Dogovor = id;
            DownloadFilesViewModel model = new DownloadFilesViewModel();
            model.NumberOfDogovor = id;
            if(dataManager.RazrabotkiRepository.GetRazrabotkiById(id)!=null &&dataManager.RazrabotkiRepository.GetRazrabotkiById(id).Макет!="нет")
            { 
                var dir = new DirectoryInfo(dataManager.RazrabotkiRepository.GetRazrabotkiById(id).Макет);
           
                 FileInfo[] fileNames = dir.GetFiles("*.*");
                 List<string> item = new List<string>();
                if(fileNames.Any())
                { 
                    foreach (var file in fileNames)
                    {
                       item.Add(file.Name);
                    }
                    model.FileList = item;
                    return View(model);
                }
                else
                {
                    return RedirectToAction("EmptyFiles");
                }
            }
            return RedirectToAction("EmptyFiles");
        }
        //сохранение файла
        public FileResult Download(string ImageName, int id)
        {
         
            DirectoryInfo dir = new DirectoryInfo(dataManager.RazrabotkiRepository.GetRazrabotkiById(id).Макет+"//");
        
            string contentType = "application/jpg";
            return File(dir + ImageName, contentType, id.ToString()+"Макет.jpg");
        } 
        //страница если нет файлов
        public ActionResult EmptyFiles()
        {
            return View();
        }
         [Authorize(Roles = "admin,master")]
        //скачиваем файлы оригиналов на компьютер с сервера
        [HttpGet]
        public ActionResult DownloadsOriginal(int id)
        {
            ViewBag.Dogovor = id;
            DownloadFilesViewModel model = new DownloadFilesViewModel();
            model.NumberOfDogovor = id;
            if(dataManager.RazrabotkiRepository.GetRazrabotkiById(id)!=null && dataManager.RazrabotkiRepository.GetRazrabotkiById(id).Оригинал!="нет")
            { 
                var dir = new DirectoryInfo(dataManager.RazrabotkiRepository.GetRazrabotkiById(id).Оригинал);
          
                FileInfo[] fileNames = dir.GetFiles("*.*");
                List<string> item = new List<string>();
                if (fileNames.Any())
                {
                    foreach (var file in fileNames)
                    {
                        item.Add(file.Name);
                    }
                    model.FileList = item;
                    return View(model);
                }
                else
                {
                    return RedirectToAction("EmptyFilesOriginal");
                }
            }
            return RedirectToAction("EmptyFilesOriginal");
        }
        //сохранение файла
        public FileResult DownloadOriginal(string ImageName, int id)
        {

            DirectoryInfo dir = new DirectoryInfo(dataManager.RazrabotkiRepository.GetRazrabotkiById(id).Оригинал + "//");

            string contentType = "application/jpg";
            return File(dir + ImageName, contentType, id.ToString() + "Оригинал.jpg");
        }
        //страница если нет файлов

        public ActionResult EmptyFilesOriginal()
        {
            return View();
        }
         [Authorize(Roles = "admin,maneger")]
        //скачиваем файлы отчёты на компьютер с сервера
        [HttpGet]
        public ActionResult DownloadsOtchet(int id)
        {
            ViewBag.Dogovor = id;
            DownloadFilesViewModel model = new DownloadFilesViewModel();
            model.NumberOfDogovor = id;
            if (dataManager.RazrabotkiRepository.GetRazrabotkiById(id) != null && dataManager.RazrabotkiRepository.GetRazrabotkiById(id).Отчёт!="нет")
            { 
            var dir = new DirectoryInfo(dataManager.RazrabotkiRepository.GetRazrabotkiById(id).Отчёт);

            FileInfo[] fileNames = dir.GetFiles("*.*");
            List<string> item = new List<string>();
            if (fileNames.Any())
            {
                foreach (var file in fileNames)
                {
                    item.Add(file.Name);
                }
                model.FileList = item;
                return View(model);
            }
            else
            {
                return RedirectToAction("EmptyFilesOtchet");
            }
            }
            return RedirectToAction("EmptyFilesOtchet");
        }
        //сохранение файла
        public FileResult DownloadOtchet(string ImageName, int id)
        {

            DirectoryInfo dir = new DirectoryInfo(dataManager.RazrabotkiRepository.GetRazrabotkiById(id).Отчёт + "//");

            string contentType = "application/jpg";
            return File(dir + ImageName, contentType, id.ToString() + "Отчёт.jpg");
        }
        //страница если нет файлов
        public ActionResult EmptyFilesOtchet()
        {
            return View();
        }

    }
}
