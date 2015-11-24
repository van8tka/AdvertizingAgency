using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BusinessLogic.Interfaces
{
   public interface IZakazyRepository
    {
       IEnumerable<Заказы> GetZakaz();
       IEnumerable<Заказы> GetZakazDogovora(int idDogovora);
       Заказы GetZakazById(int id);
       void CreateZakaz(int codZak, int codUslugi, int codDogov, int allCost);
       void DeleteZakaz(Заказы zakaz);
       void SaveZakaz(Заказы zakaz);
    }
}
