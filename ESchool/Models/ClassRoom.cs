using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESchool.Models
{
    public class ClassRoom
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="الفصل")]
        public string Name { get; set; }
        [Display(Name = "الصف")]
        public int saufId { get; set; }
        public sauf sauf { get; set; }
        
    }
}