using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESchool.Models
{
    public class MemberClassYear
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="الاسم")]
        public string MemberId { get; set; }
        [Display(Name = "السنه الدراسية")]
        public int Year { get; set; }
        [Display(Name = "المرحلة"),Required]
        public int marhalaId { get; set; }
        [Display(Name = "الصف"), Required]
        public int SaufId { get; set; }
        [Display(Name = "الفصل"), Required]
        public int ClassId { get; set; }

        [Display(Name = "هل هو العام الحالي؟")]
        public bool IsCurrent { get; set; }
     
       
    }
}