using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESchool.Models
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }
        [Required, Display(Name = "الفصل الدراسي")]
        public Semster semster { get; set; }
        [Required, Display(Name = "العام الدراسي")]

        public string yearOfStudy { get; set; }

        [Required,Display(Name ="رسوم الإبتدائية")]
        public string Primary { get; set; }
        [Required, Display(Name = "رسوم المتوسطة")]
        public string Intermediate { get; set; }
        [Required, Display(Name = "رسوم الثانوية")]
        public string Secondary { get; set; }
        [Required, Display(Name = "رسوم باص اتجاه")]
        public double Bus1Way { get; set; }
        [Required, Display(Name = "رسوم باص اتجاهين")]
        public double Bus2Way { get; set; }
      
        [Required, Display(Name = "ضريبة القيمة المضافة نسبة ")]

        public double Vat { get; set; }
        [Required, Display(Name = "هل الحالي؟")]
        public Boolean IsCurrent { get; set; }

        public enum Semster
        {
            الأول,الثاني,الثالث,الرابع
        }

    }
}