using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace AdvertizingAgency1.Models
{
    public class ZakazOfDogovorViewModel
    {
        public IEnumerable<Заказы> Zakaz { get; set; }
        public IEnumerable<Услуги> Uslugi { get; set; }
        public IEnumerable<АдресныеЗаказы> AdresZakaz { get; set; }
        public IEnumerable<Адреса> Adresa { get; set; }
        public int IDClient { get; set; }
     
    }
}