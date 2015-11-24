using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BusinessLogic.Interfaces;
using DAL;

namespace BusinessLogic.Implementations
{
   public class EfZakazyAdresyRepository:IZakazyAdresyRepository
   {
       private AdvertizingAgency1Entities context;

       public EfZakazyAdresyRepository(AdvertizingAgency1Entities context)
       {
           this.context = context;
       }
        public IEnumerable<АдресныеЗаказы> GetAdZakaz()
        {
            return context.АдресныеЗаказы;
        }

     

        public АдресныеЗаказы GetAdrZakazByIdZakaza(int id)
        {
            return context.АдресныеЗаказы.FirstOrDefault(x => x.Код_заказа == id);
        }

        public void CreateAdrZakaz(int codZak, int codAdres, string DateEnd)
        {
            АдресныеЗаказы zakaz = new АдресныеЗаказы
            {
                Код_заказа = codZak,
                Код_адреса = codAdres,
                Дата_окончания = DateTime.Parse(DateEnd),
                Срок_продления = 0,
                Стоимость_размещения = 0,
                Стоимость_продления = 0
            };
            context.АдресныеЗаказы.Add(zakaz);
            context.SaveChanges();
        }

        public void ChangeAdreZakazProdlen(int codzak, int codAdres, string newDate, int StoimRazm)
        {
            АдресныеЗаказы zakaz = new АдресныеЗаказы
            {
                Код_заказа = codzak,
                Код_адреса = codAdres,
                Дата_окончания = DateTime.Parse(newDate),
                Срок_продления = 0,
                Стоимость_размещения = StoimRazm,
                Стоимость_продления = 0
            };
            context.Entry(zakaz).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void DeleteAdrZakaz(АдресныеЗаказы zakaz)
        {
            context.АдресныеЗаказы.Remove(zakaz);
            context.SaveChanges();
        }

      
    }
}
