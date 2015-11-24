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
   public class EfRazrabotkiRepozitory:IRazrabotkiRepository
   {
       private AdvertizingAgency1Entities context;

       public EfRazrabotkiRepozitory(AdvertizingAgency1Entities context)
       {
           this.context = context;
       }
        public IEnumerable<Разработки> GetRazrabotki()
        {
            return context.Разработки;
        }

        public Разработки GetRazrabotkiById(int id)
        {
            return context.Разработки.Where(x => x.Номер_договора == id).FirstOrDefault();
        }

        public void CreateRazrabotki(int dog,string maket, string original, string otchet)
        {
            Разработки raz = new Разработки()
            {
                Номер_договора = dog,
                Макет = maket,
                Оригинал = original,
                Отчёт = otchet
            };
            context.Разработки.Add(raz);
            context.SaveChanges();
        }

        public void DeleteRazrabotki(Разработки razrab)
        {
            context.Разработки.Remove(razrab);
            context.SaveChanges();
        }

        public void ChangeRazrabotki(int dog,string maket, string original, string otchet)
        {
            Разработки raz = new Разработки()
            {
                Номер_договора = dog,
                Макет = maket,
                Оригинал = original,
                Отчёт = otchet
            };
            DeleteRazrabotki(GetRazrabotkiById(dog));
            context.Разработки.Add(raz);
            context.SaveChanges();
        }
    }
}
