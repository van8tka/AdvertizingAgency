using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace AdvertizingAgency1.Models
{
    public class UsersViewModel
    {
        public IEnumerable<Сотрудники> Users { get; set; }
        public IEnumerable<Администрирование> Registers { get; set; } 
    }
}