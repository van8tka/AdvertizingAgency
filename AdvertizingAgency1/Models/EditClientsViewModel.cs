using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace AdvertizingAgency1.Models
{
    public class EditClientsViewModel
    {
        public Клиенты client { get; set; }
        public Контактное_лицо contact { get; set; }
    }
}