using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using DAL;

namespace BusinessLogic.Implementations
{
   public class EfContaktnoeLicoRepository:IContaktnoeLicoRepository
    {
        private AdvertizingAgency1Entities context;

        public EfContaktnoeLicoRepository(AdvertizingAgency1Entities context)
        {
            this.context = context;
        }

        public IEnumerable<Контактное_лицо> GetContactnoeLico()
        {
            return context.Контактное_лицо;
        }

        public Контактное_лицо GetContactnoeLicoById(int id)
        {
            return context.Контактное_лицо.FirstOrDefault(x=>x.Код_клиента == id);
        }
        public Контактное_лицо CetContactnoeLicoByName(string Fio)
        {
            return context.Контактное_лицо.FirstOrDefault(x => x.ФИО == Fio);
        }
        public void CreateContactnoeLico(int codClient, string Fio, string tel, string email, string Skype, string Want)
        {
            Контактное_лицо contactnoeLico = new Контактное_лицо
            {
                Код_клиента = codClient,
                ФИО = Fio,
                Телефон = tel,
                Email = email,
                Skype = Skype,
                Пожелания = Want
            };
            SaveContactnoeLico(contactnoeLico);
        }

        public void DeleteContactnoeLico(Контактное_лицо ContactnoeLico)
        {
            context.Контактное_лицо.Remove(ContactnoeLico);
            context.SaveChanges();
        }

        public void SaveContactnoeLico(Контактное_лицо ContactnoeLico)
        {

            if (context.Контактное_лицо.Find(ContactnoeLico.Код_клиента)==null)
            {
                context.Контактное_лицо.Add(ContactnoeLico);
            }
            else
            {
                //DeleteContactnoeLico(GetContactnoeLicoById(ContactnoeLico.Код_клиента));
                context.Контактное_лицо.Add(ContactnoeLico);
            }
            context.SaveChanges();
        }
    }
}
