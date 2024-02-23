using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESchool.Models
{
    public class DisposalDocument
    {
        [Key, DisplayName("رقم الإيصال")]
        public int Id { get; set; }
        [Required, DisplayName("تاريخ الدفع")]
        public string DateOfPay { get; set; }
        [Required, DisplayName("المبلغ ")]
        public double Amount { get; set; }
        [Required, DisplayName("ضريبة القيمة المضافة")]
        public double Vat { get; set; }
        [Required, DisplayName("المبلغ الإجمالي")]
        public double TotalAmount { get; set; }
        [Required, DisplayName("طريقة الدفع")]
        public PayType payType { get; set; }
        [Required, DisplayName("وذلك عن")]
        public string Thisfor { get; set; }
        [ DisplayName("ملاحظات")]
        public string Notes { get; set; }
        public string userId { get; set; }
        public virtual ApplicationUser user { get; set; }

        public enum PayType
        {
            بطاقة, نقداً, شيك, تحويل_بنكي
        }
    }
}