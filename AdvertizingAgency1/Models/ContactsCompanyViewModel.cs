using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DAL;

namespace AdvertizingAgency1.Models
{
    public class ContactsCompanyViewModel
    {
        public IEnumerable<Сотрудники> SpisSotr { get; set; }
       
        [Required(ErrorMessage = "Введите email адрес!")]
        [RegularExpression(@"^[a-zA-Z0-9.-]{1,20}@[a-zA-Z0-9]{1,20}\.[A-Za-z]{2,4}", ErrorMessage = "Неверный формат email.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Введите номер расчётного счёта!")]
        [RegularExpression(@"^[0-9]{13}", ErrorMessage = "Неверный формат расчётного счёта.(13 цифр)")]
        public string Rsc { get; set; }
        [Required(ErrorMessage = "Введите пароль для входа в email компании!")]
        [DataType(DataType.Password)]
        public string Pasw { get; set; }
    }
}