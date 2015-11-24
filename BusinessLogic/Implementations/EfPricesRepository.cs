using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BusinessLogic.Interfaces;
using DAL;

namespace BusinessLogic.Implementations
{
    public class EfPricesRepository:IPricesRepository
    {
        private AdvertizingAgency1Entities context;

        public EfPricesRepository(AdvertizingAgency1Entities context)
        {
            this.context = context;
        }
        public IEnumerable<Услуги> GetPrices()
        {
            return context.Услуги;
        }

        public Услуги GetPricesById(int id)
        {
            return context.Услуги.FirstOrDefault(x => x.Код_услуги == id);
        }

        public Услуги GetPricesByName(string priceName)
        {
            return context.Услуги.FirstOrDefault(x => x.Наименование == priceName);
        }

        public void CreatePrice(int codPrice, string priceName, int priceCost, string priceType)
        {
            Услуги price = new Услуги
            {
                Код_услуги = codPrice,
                Наименование = priceName,
                Цена = priceCost,
                Тип = priceType
            };
            SavePrice(price);    
        }

        public void DeletePrice(Услуги price)
        {
            context.Услуги.Remove(price);
            context.SaveChanges();
        }
        public void SavePrice(Услуги price)
        {
           
            if (price.Код_услуги == 0)
            {
                context.Услуги.Add(price);
            }
      else
            {
                context.Entry(price).State = EntityState.Modified;
            }
            context.SaveChanges();          
        }  
    }
}
