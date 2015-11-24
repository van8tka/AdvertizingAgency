using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdvertizingAgency1.Models
{
    public class CreateUserViewModel
    {

        public int Kod { get; set; }
         [Required(ErrorMessage = "Введите фамилию и инициалы")]
         [RegularExpression(@"^[а-яА-я .]{1,20}", ErrorMessage = "Неверный формат ввода, введите: Фамилия И.О.")]
        public string Fio{get;set;}
         [Required(ErrorMessage = "Введите должность")]
         [RegularExpression(@"^[а-яА-Я]{1,30}", ErrorMessage = "Неверный формат должности!")]
        public string Dolgnost{get;set;}
         [Required(ErrorMessage = "Введите адрес")]
        public string Adres{get;set;}
         [Required(ErrorMessage = "Введите номер телефона, в формате ххх-хх-хх")]
         [RegularExpression(@"^[0-9]{1,3}-[0-9]{1,2}-[0-9]{1,2}", ErrorMessage = "Неверный формат ввода номера телефона, введите в формате XXX-XX-XX.")]
        public string Phone{get;set;}
         [Required(ErrorMessage = "Введите email")]
         [RegularExpression(@"^[a-zA-Z0-9.-]{1,20}@[a-zA-Z0-9]{1,20}\.[A-Za-z]{2,4}", ErrorMessage = "Неверный формат email.")]
        public string Email{get;set;}

    }
}