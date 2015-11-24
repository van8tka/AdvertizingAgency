using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BusinessLogic.Interfaces
{
   public interface IClientsRepository
    {
        IEnumerable<Клиенты> GetClients();
        Клиенты GetClientsById(int id);
        Клиенты GetClientsByUNP(int unp);
        Клиенты GetClientByName(string clientName);
      void CreateClient(int codClient, string clientName,int numberUNP,string Adres, string Status);
        void DeleteClient(Клиенты client);
        void SaveClient(Клиенты client);
    }
}
