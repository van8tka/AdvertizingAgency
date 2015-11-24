using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BusinessLogic.Interfaces
{
   public interface IContaktnoeLicoRepository
   {
       IEnumerable<Контактное_лицо> GetContactnoeLico();
       Контактное_лицо GetContactnoeLicoById(int id);
       void CreateContactnoeLico(int codClient, string Fio, string tel, string email ,string Skype, string Want);
       void DeleteContactnoeLico(Контактное_лицо ContactnoeLico);
       void SaveContactnoeLico(Контактное_лицо ContactnoeLico);
   }
}
