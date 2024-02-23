using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESchool.Models
{
    public class Nationality
    {
        [Key]
        public int Id { get; set; }
        [Required,DisplayName("الدولة")]
        public string Country { get; set; }

        //public ICollection<MemberData> members { get; set; }
    }
}