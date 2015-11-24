using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdvertizingAgency1.Models
{
    public class CreateServiceViewModel
    {
      //это значит обязательно к заполнению
        [Required(ErrorMessage = "Введите название услуги!")]
        public string ServiceName { get; set; }
         [Required(ErrorMessage = "Введите цену услуги!")]
         [RegularExpression(@"^[0-9]{1,20}", ErrorMessage = "Неверный формат ввода стоимости")]
        public int  ServiceCost { get; set;}
        //тип услуги
        public string Type { get; set; }
     }
}