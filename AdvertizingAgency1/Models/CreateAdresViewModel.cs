using System.ComponentModel.DataAnnotations;

namespace AdvertizingAgency1.Models
{
    public class CreateAdresViewModel
    {
        //это значит обязательно к заполнению
        [Required(ErrorMessage = "Введите адрес!")]
        public string AdresName { get; set; }
        [Required(ErrorMessage = "Введите цену аренды!")]
        [RegularExpression(@"^[0-9]{1,20}", ErrorMessage = "Неверный формат ввода стоимости!")]
        public int AdresCost { get; set; }    
    }
}