using System.Collections.Generic;
using DAL;

namespace AdvertizingAgency1.Models
{
    public class AdressTableUseViewModel
    {
      public  IEnumerable<Адреса> Adres { get; set; }
     public   IEnumerable<АдресныеЗаказы> AdreZakaz { get; set; }
     public int CurrentYear { get; set; }
     public int CurrentMonth { get; set; }
    }
}