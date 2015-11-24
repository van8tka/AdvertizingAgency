using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BusinessLogic.Interfaces;
using DAL;

namespace BusinessLogic.Implementations
{
   public class EfZakazyRepository:IZakazyRepository
   {
       private AdvertizingAgency1Entities context;

       public EfZakazyRepository(AdvertizingAgency1Entities context)
       {
           this.context = context;
       }
        public IEnumerable<Заказы> GetZakaz()
        {
            return context.Заказы;
        }

        public IEnumerable<Заказы> GetZakazDogovora(int idDogovora)
        {
            return context.Заказы.Where(x=>x.Номер_договора == idDogovora);
        }

        public Заказы GetZakazById(int id)
        {
            return  context.Заказы.FirstOrDefault(x => x.Код_заказа == id);
        }

        public void CreateZakaz(int codZak, int codUslugi, int codDogov, int allCost)
        {
            Заказы zakaz = new Заказы
            {
                Код_заказа = codZak,
                Код_услуги = codUslugi,
                Номер_договора = codDogov,
                Стоимость = allCost
            };
            SaveZakaz(zakaz);
        }

        public void DeleteZakaz(Заказы zakaz)
        {
            context.Заказы.Remove(zakaz);
            context.SaveChanges();
        }

        public void SaveZakaz(Заказы zakaz)
        {
            if (zakaz.Код_заказа == 0)
            {
                context.Заказы.Add(zakaz);
            }
            else
            {
                context.Entry(zakaz).State = EntityState.Modified;
            }
            context.SaveChanges();
        }
    }
}
