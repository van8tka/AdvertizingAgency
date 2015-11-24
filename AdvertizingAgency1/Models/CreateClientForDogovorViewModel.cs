using System.ComponentModel.DataAnnotations;

namespace AdvertizingAgency1.Models
{
    public class CreateClientForDogovorViewModel
    {
        public int clientId { get; set; }
        [Required(ErrorMessage = "Введите название организации!")]
        public string clientName { get; set; }
        [Required(ErrorMessage = "Введите номер УНП !")]
        [RegularExpression(@"^[0-9]{9}", ErrorMessage = "Неверный формат УНП.(9 цифр)")]
        public int clientUNP { get; set; }
         [Required(ErrorMessage = "Введите адрес!")]
        public string clientAdres { get; set; }
         [Required(ErrorMessage = "Введите фамилию и инициалы")]
         [RegularExpression(@"^[а-яА-я .]{1,20}", ErrorMessage = "Неверный формат ввода, введите: Фамилия И.О.")]
       public string clientFio { get; set; }
         [Required(ErrorMessage = "Введите номер телефона!")]
         [RegularExpression(@"^[0-9]{1,3}-[0-9]{1,2}-[0-9]{1,2}", ErrorMessage = "Неверный формат ввода номера телефона, введите в формате XXX-XX-XX.")]
        public string clientPhone { get; set; }
         [Required(ErrorMessage = "Введите email адрес!")]
         [RegularExpression(@"^[a-zA-Z0-9.-]{1,20}@[a-zA-Z0-9]{1,20}\.[A-Za-z]{2,4}", ErrorMessage = "Неверный формат email.")]
     
        public string clientEmail { get; set; }
         [Required(ErrorMessage = "Введите skype!")]
         [RegularExpression(@"^[a-zA-Z0-9]{1,25}", ErrorMessage = "Неверный формат ввода skype!")]
     
        public string clientSkype { get;set; }
         [Required(ErrorMessage = "Введите дополнительную информацию!")]
        public string clientPogelanie { get; set; }
       
    }
}