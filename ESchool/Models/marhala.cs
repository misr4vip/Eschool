using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESchool.Models
{
    public class marhala
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="المرحلة")]
        public string Name { get; set; }
        public  ICollection<sauf> GetSaufs { get; set; }
    }
}