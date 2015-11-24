using System.Collections.Generic;
using DAL;

namespace BusinessLogic.Interfaces
{
  public interface IZakazyAdresyRepository
    {
        IEnumerable<АдресныеЗаказы> GetAdZakaz();
    
        АдресныеЗаказы GetAdrZakazByIdZakaza(int id);
        void CreateAdrZakaz(int codZak, int codAdres, string DateEnd);
      void ChangeAdreZakazProdlen(int codzak, int codAdres, string newDate, int StoimRazm);
        void DeleteAdrZakaz(АдресныеЗаказы zakaz);
    
    }
}
