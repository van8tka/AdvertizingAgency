using System.Linq;
using System.Xml.Linq;
using AdvertizingAgency1.Models;
using DAL;

namespace AdvertizingAgency1.AboutCompany
{
    public class AboutCompany
    {
        
       //метод вывода содержимого о компании из xml файла
        public static ContactsCompanyViewModel FromXML(string path)
        {
            AdvertizingAgency1Entities context = new AdvertizingAgency1Entities();
            ContactsCompanyViewModel model = new ContactsCompanyViewModel();
         
            //загрузили файл
            XDocument xdoc = XDocument.Load(path);

            model.SpisSotr = context.Сотрудники.Where(x => x.Должность == "менеджер" || x.Должность == "руководитель");
           
            foreach (XElement aboutElement in xdoc.Element("about").Elements("RshCompany"))
            {
                XAttribute nameatr = aboutElement.Attribute("nameRsc");
                XElement Rsc = aboutElement.Element("Rsc");
                XElement EmComp = aboutElement.Element("Em");
                XElement Paswor = aboutElement.Element("Pasw");
                model.Rsc = Rsc.Value;
                model.Email = EmComp.Value;
                model.Pasw = Paswor.Value;
            }
            return model;

        }
    }
}