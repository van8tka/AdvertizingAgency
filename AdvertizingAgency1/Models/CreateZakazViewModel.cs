using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;

namespace AdvertizingAgency1.Models
{
    public class CreateZakazViewModel
    {
       
       public int NumberOfDogovor { get; set; }

         [Required(ErrorMessage = "Выберите услугу")]
        public int uslId { get; set; }
        public string uslType { get; set; }
        public int Cost { get; set; }

        public IEnumerable<SelectListItem> SpisokType { get; set; }
       
        public IEnumerable<SelectListItem> SpisokUslug  { get; set; }

    }
}