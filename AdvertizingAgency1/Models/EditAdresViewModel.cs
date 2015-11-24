using System.ComponentModel.DataAnnotations;
using DAL;

namespace AdvertizingAgency1.Models
{
    public class EditAdresViewModel
    {
        //   [Required(ErrorMessage = "Введите номер расчётного счёта!")]
        //public Адреса adres { get; set; }

           public int IDadres { get; set; }
           //это значит обязательно к заполнению
           [Required(ErrorMessage = "Введите адрес!")]
           public string NameAdres { get; set; }
           [Required(ErrorMessage = "Введите цену аренды!")]
           [RegularExpression(@"^[0-9]{1,20}", ErrorMessage = "Неверный формат ввода стоимости!")]
           public int AdresCost { get; set; }
          
    }
}