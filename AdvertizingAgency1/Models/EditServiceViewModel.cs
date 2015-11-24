using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DAL;

namespace AdvertizingAgency1.Models
{
    public class EditServiceViewModel
    {
       
        public int IDser { get; set; }
        //это значит обязательно к заполнению
        [Required(ErrorMessage = "Введите название услуги!")]
        public string ServiceName { get; set; }
        [Required(ErrorMessage = "Введите цену услуги!")]
        [RegularExpression(@"^[0-9]{1,20}", ErrorMessage = "Неверный формат ввода стоимости")]
        public int ServiceCost { get; set; }
        //тип услуги
        public string Type { get; set; }
    }
}