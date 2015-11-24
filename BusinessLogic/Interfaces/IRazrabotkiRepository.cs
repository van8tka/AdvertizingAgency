using System.Collections.Generic;
using DAL;

namespace BusinessLogic.Interfaces
{
   public interface IRazrabotkiRepository
    {
        IEnumerable<Разработки> GetRazrabotki();
        Разработки GetRazrabotkiById(int id);
        void CreateRazrabotki(int dog,string maket, string original, string otchet);
        void DeleteRazrabotki(Разработки razrab);
        void ChangeRazrabotki(int dog,string maket, string original, string otchet);
    }
}
