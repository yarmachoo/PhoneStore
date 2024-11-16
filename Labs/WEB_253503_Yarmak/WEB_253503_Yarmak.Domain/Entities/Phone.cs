using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253503_Yarmak.Domain.Entities
{
    public class Phone
    {
        public int Id {  get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
        [DisplayName("Категория")]
        public int CategoryId {  get; set; }
        [DisplayName("Стоимость")]
        public double Price { get; set; }
        public string? Image { get; set; }
        public string Mime {  get; set; }
    }
}
