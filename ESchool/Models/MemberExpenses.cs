using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESchool.Models
{
    public class MemberExpenses
    {
        [Key]
        public int Id { get; set; }
        [Required,DisplayName("نوع المصروفات")]
        public ExpensesType expensesText { get; set; }
        [Required, DisplayName("قيمة المصروفات")]
        public double ExpensesValue { get; set; }
        [Required, DisplayName("عن السنة الدراسية")]
        public string YearOfStudy { get; set; }
        [Required, DisplayName("عن الفصل الدراسي")]
        public string Semster { get; set; }
        [Required, DisplayName("ضريبة القيمة المضافة")]
        public double Vat { get; set; }
        [Required, DisplayName("قيمة الخصم")]
        public double Discount { get; set; }
        [Required, DisplayName("الاجمالي ")]
        public double TotalExpensesValue { get; set; }
        [Required, DisplayName("بيانات الطالب")]
        public string userId { get; set; }
       
        public virtual ApplicationUser user { get; set; }

        public enum ExpensesType
        {
            رسوم_الفصل_الدراسي ,حافلة_اتجاه_واحد,حافلة_اتجاهين,أخري,مديونية_سابقة
        }


    }
}