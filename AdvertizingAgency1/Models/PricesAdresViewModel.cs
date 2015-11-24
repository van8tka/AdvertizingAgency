using System.Collections.Generic;
using DAL;

namespace AdvertizingAgency1.Models
{
    public class PricesAdresViewModel
    {
        public IEnumerable<Адреса> Adres { get; set; }
        public PagingInfoViewModel PagingInfoAdres { get; set; }
    }
}