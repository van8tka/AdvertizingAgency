using System.Collections.Generic;
using DAL;

namespace BusinessLogic.Interfaces
{
   public interface IAdresRepository
   {
       IEnumerable<Адреса> GetAdreses();
       IEnumerable<Координаты> GetKoordinats();
       Координаты GetKoordinatByID(int id);
       void CreateKoordinat(int cod, float Lat, float Lng);
       
       Адреса GetAdresesById(int id);
       Адреса GetAdresByIdZakaza(int id);
       Адреса GetAdresByName(string adresName);
       void CreateAdres(int codAdres, string adresName, int adresCost);
       void DeleteAdres(Адреса adres);
       void SaveAdres(Адреса adres);
           
    }
}
