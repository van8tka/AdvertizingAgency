using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BusinessLogic.Interfaces
{
   public interface IDogovorRepository
    {

        IEnumerable<Договора> GetDogovora();
        Договора GetDogovoraById(int id);

        void CreateDogovora(int numberDogovor,int kodsotrudnika, int kodklienta, DateTime datazakaza, DateTime datagotovnosti,
           int stoimost, string status, string primechanie);

        void DeleteDogovor(Договора dogovor);
        void SaveDogovora(Договора dogovor);
    }
}
