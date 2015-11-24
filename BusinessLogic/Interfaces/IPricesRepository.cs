using System.Collections.Generic;
using DAL;

namespace BusinessLogic.Interfaces
{
   public interface IPricesRepository
   {
       IEnumerable<Услуги> GetPrices();

   
          Услуги GetPricesById(int id);
       Услуги GetPricesByName(string priceName);
       void CreatePrice(int cod, string priceName, int priceCost, string priceType);
       void DeletePrice(Услуги price);
       void SavePrice(Услуги price);


    
   }
}
