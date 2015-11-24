using System.Collections.Generic;
using DAL;

namespace AdvertizingAgency1.Models
{
    public class ServicesViewModel
    {
        public IEnumerable<Услуги> Services { get; set; }
        public PagingInfoViewModel PagingInfo { get; set; }
        //добавим для новой страницы прайс(список типов услуг)
        public List<string> TypPrice = new List<string>();
        //список изображений к услугам
        public List<string> FileList { get; set; }
    }
}