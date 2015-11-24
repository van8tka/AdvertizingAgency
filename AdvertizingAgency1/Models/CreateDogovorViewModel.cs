using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdvertizingAgency1.Models
{
    public class CreateDogovorViewModel
    {

        public int IDdog { get; set;}

        [Required(ErrorMessage = "Заполните дополнительную информацию!")]
        public string dogovorPrimechanie { get; set; }
        
        [Required(ErrorMessage = "Введите дату готовности договора!")]
        [RegularExpression(@"^[0-9]{1,2}.[0-9]{1,2}.[2]{1}[0]{1}[0-9]{1,2}", ErrorMessage = "Неверный формат ввода даты, введите: дд.мм.гггг")]
        public  string dogovorGotovnost { get; set; }

        [Required(ErrorMessage = "Введите дату заключения договора!")]
        [RegularExpression(@"^[0-9]{1,2}.[0-9]{1,2}.[2]{1}[0]{1}[0-9]{1,2}", ErrorMessage = "Неверный формат ввода даты, введите: дд.мм.гггг")]
        public string dogovorZaclechen { get; set; }
        public int dogovorClientId { get; set; }
        public int dogStoimost { get; set; }

    }
}