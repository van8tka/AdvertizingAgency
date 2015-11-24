using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AdvertizingAgency1.Models
{
    public class RegisterUserViewModel
    {
        public int Kod { get; set; }
        [Required(ErrorMessage = "Введите логин")]
        [RegularExpression(@"^[a-zA-Z0-9]{1,20}", ErrorMessage = "Неверный формат логина.")]
        public string Login { get; set; }
         [Required(ErrorMessage = "Введите пароль")]
         [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Подтвердите пароль")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароль подтверждён не верно")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Выберите роль сотрудника")]
        public string SelectedRoles { get; set; }

        private List<SelectListItem> rolers = new List<SelectListItem>(); 
        [Required(ErrorMessage = "Выберите роль")]
        public List<SelectListItem> SpisokRoles
        {
            get
            {
                rolers.Clear();
                rolers.Add(new SelectListItem(){ Text = "admin",Value = "1"});
                rolers.Add(new SelectListItem() { Text = "maneger", Value = "2" });
                rolers.Add(new SelectListItem() { Text = "designer", Value = "3" });
                rolers.Add(new SelectListItem() { Text = "master", Value = "4" });
                rolers.Add(new SelectListItem() { Text = "installer", Value = "5" });
                rolers.Add(new SelectListItem() { Text = "bookkeeper", Value = "6" });
                return rolers;
            }
        }
    }
}