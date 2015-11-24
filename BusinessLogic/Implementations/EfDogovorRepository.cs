using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using DAL;

namespace BusinessLogic.Implementations
{
   public class EfDogovorRepository:IDogovorRepository
   {
       private AdvertizingAgency1Entities context;

       public EfDogovorRepository(AdvertizingAgency1Entities context)
       {
           this.context = context;
       }

        public IEnumerable<Договора> GetDogovora()
        {
            return context.Договора;
        }

        public Договора GetDogovoraById(int id)
        {
            return context.Договора.FirstOrDefault(x=>x.Номер_договора == id);
        }

        public void CreateDogovora(int numberDogovor, int kodsotrudnika, int kodklienta, DateTime datazakaza, DateTime datagotovnosti, int stoimost, string status, string primechanie)
        {
            Договора dogovor = new Договора
            {
                Номер_договора = numberDogovor,
                Код_сотрудника = kodsotrudnika,   
                Код_клиента = kodklienta,
                Дата_заказа = datazakaza,
                Дата_готовности = datagotovnosti,
                Итоговая_стоимость = stoimost,
                Статус = status,
                Примечания = primechanie
            };
            SaveDogovora(dogovor);
        }

        public void DeleteDogovor(Договора dogovor)
        {
            context.Договора.Remove(dogovor);
            context.SaveChanges();
        }

        public void SaveDogovora(Договора dogovor)
        {
             if (dogovor.Номер_договора == 0)
                     context.Договора.Add(dogovor);
            else
            {
                context.Entry(dogovor).State = EntityState.Modified;
            }
            context.SaveChanges();
        }
    }
}
