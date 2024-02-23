using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESchool.Models
{
    public class sauf
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "الصف")]
        public string Name { get; set; }
        [Display(Name = "المرحلة")]
        public int marhalaId { get; set; }
        public marhala marhala { get; set; }
        public ICollection<ClassRoom> GetClassRooms { get; set; }
    }
}