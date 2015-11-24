using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using DAL;
using Ninject.Activation;

namespace BusinessLogic.Implementations
{
   public class EfClientsRepository:IClientsRepository
    {
       AdvertizingAgency1Entities context;

       public EfClientsRepository(AdvertizingAgency1Entities context)
       {
           this.context = context;
       }
        public IEnumerable<Клиенты> GetClients()
        {
            return context.Клиенты;
        }

        public Клиенты GetClientsById(int id)
        {
            return context.Клиенты.FirstOrDefault(x=>x.Код_клиента == id);
        }

        public Клиенты GetClientByName(string clientName)
        {
            return context.Клиенты.FirstOrDefault(x => x.Наименование == clientName);
         
        }

        public void CreateClient(int codClient, string clientName, int numberUNP, string Adres, string Status)
        {
            Клиенты client = new Клиенты
            {
                Код_клиента = codClient,
                Наименование = clientName,
                УНП = numberUNP,
                Адрес = Adres,
                Статус = Status
            };
            SaveClient(client);
        }

        public void DeleteClient(Клиенты client)
        {
            context.Клиенты.Remove(client);
            context.SaveChanges();
        }

        public void SaveClient(Клиенты client)
        {
            if (client.Код_клиента == 0)
            {
                context.Клиенты.Add(client);
            }
            else
            {
                Контактное_лицо con =
                    context.Контактное_лицо.Where(x => x.Код_клиента == client.Код_клиента).FirstOrDefault();
                context.Контактное_лицо.Remove(con);
               
                context.Клиенты.Remove(context.Клиенты.Where(x=>x.Код_клиента==client.Код_клиента).FirstOrDefault());
                context.Клиенты.Add(client);
              
            }
            context.SaveChanges();
        }


        public Клиенты GetClientsByUNP(int unp)
        {
            return context.Клиенты.FirstOrDefault(x => x.УНП == unp);
        }

    }
}
