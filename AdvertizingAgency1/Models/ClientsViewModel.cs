using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace AdvertizingAgency1.Models
{
    public class ClientsViewModel
    {
        public IEnumerable<Клиенты> client { get; set; }
        public IEnumerable<Контактное_лицо> contactnoeLico { get; set; }

        public PagingInfoViewModel PagingInfo { get; set; }
    }
}