using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BusinessLogic.Interfaces;
using DAL;

namespace BusinessLogic.Implementations
{
    public class EfAdresRepository:IAdresRepository
    {
         private AdvertizingAgency1Entities context;

         public EfAdresRepository(AdvertizingAgency1Entities context)
        {
            this.context = context;
        }

        public IEnumerable<Адреса> GetAdreses()
        {
            return context.Адреса;
        }

        public Адреса GetAdresesById(int id)
        {
            return context.Адреса.FirstOrDefault(x => x.Код_адреса == id);
        }

        public Адреса GetAdresByName(string adresName)
        {
            return context.Адреса.FirstOrDefault(x => x.Адрес == adresName);
        }

        public void CreateAdres(int codAdres, string adresName, int adresCost)
        {
            Адреса adres = new Адреса
            {
                Код_адреса = codAdres,
                Адрес = adresName,
                Цена_на_месяц = adresCost
            };
            SaveAdres(adres);
        }

        public void DeleteAdres(Адреса adres)
        {
            context.Адреса.Remove(adres);
            context.SaveChanges();
        }

        public void SaveAdres(Адреса adres)
        {
            if (adres.Код_адреса == 0)
                context.Адреса.Add(adres);
            else
            {
                context.Entry(adres).State = EntityState.Modified;
            }
            context.SaveChanges();
        }


        public Адреса GetAdresByIdZakaza(int id)
        {
            IEnumerable<Адреса> adres = from az in context.АдресныеЗаказы
                join ax in context.Адреса
                    on az.Код_адреса equals ax.Код_адреса
                where az.Код_заказа == id
                select new Адреса();
            return adres.FirstOrDefault();
        }


        public IEnumerable<Координаты> GetKoordinats()
        {
            return context.Координаты;
        }

        public Координаты GetKoordinatByID(int id)
        {
            return context.Координаты.Where(x => x.Код_адреса == id).FirstOrDefault();
        }

        public void CreateKoordinat(int cod, float Lat, float Lng)
        {
            Координаты k = context.Координаты.Where(x => x.Код_адреса == cod).FirstOrDefault();
            if (k != null)
            {
                context.Координаты.Remove(k);
                context.SaveChanges();
            }
            Координаты koord = new Координаты()
            {
                Код_адреса = cod,
                Широта = Lat,
                Долгота = Lng
            };
            context.Координаты.Add(koord);
            context.SaveChanges();
        }
    }
}
