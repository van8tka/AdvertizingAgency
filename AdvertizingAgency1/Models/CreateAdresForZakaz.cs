using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdvertizingAgency1.Models
{
    public class CreateAdresForZakaz
    {
        public int NumberOfZakaz { get; set; }
         [Required(ErrorMessage = "Выберите адрес!")]
        public int AdresId { get; set; }
        [Required(ErrorMessage = "Ошибка заполнения поля цены адреса!")]
        [RegularExpression(@"^[0-9]{1,20}", ErrorMessage = "Неверный формат ввода стоимости")]
        public int AdresCost { get; set; }
        //[Required(ErrorMessage = "Ошибка заполнения поля цены продления!")]
        //[RegularExpression(@"^[0-9]{1,20}", ErrorMessage = "Неверный формат ввода стоимости")]
        public int AdresCostProdlen { get; set; }
        [Required(ErrorMessage = "Введите дату окончания обслуживания!")]
        [RegularExpression(@"^[0-9]{1,2}.[0-9]{1,2}.[2]{1}[0]{1}[0-9]{1,2}", ErrorMessage = "Неверный формат ввода даты, введите: дд.мм.гггг")]
        public string AdresDateEnd { get; set; }
      
        public string AdresSrokProdlen { get; set; }
         
        public IEnumerable<SelectListItem> SpisokAdresov { get; set; }
        public int NumberOfDog { get; set; }
    }
}