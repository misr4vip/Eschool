using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ESchool.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MinLength(10, ErrorMessage = "رقم الهوية اقل من 10 ارقام"), MaxLength(10, ErrorMessage = "رقم الهوية اكثر من 10 ارقام"), DisplayName("رقم الهوية")]
        public string IdentityId { get; set; }
        [Required, Display(Name = "الاسم")]
        public string Name { get; set; }
        [Display(Name = "الاسم باللغة الإنجليزية")]
        public string EnglishName { get; set; }
        [Required, Display(Name = "تاريخ الميلاد ")]
        public string Dob { get; set; }
        [Required, Display(Name = "الجنسية")]
        public Nathionality nathionality { get; set; }

        [Display(Name = "جوال 1 ")]
        public string Mobile1 { get; set; }
        [Display(Name = "جوال 2 ")]
        public string Mobile2 { get; set; }
        [Display(Name = "هاتف المنزل ")]
        public string tel { get; set; }
        //public virtual Nationality nationality { get; set; }
        [Display(Name = "الحالة ")]
        public MemberStatus status { get; set; }
        [Display(Name = "نوع الحساب ")]
        public MemberType type { get; set; }

        public enum MemberStatus
        {
            نشط, موقوف, محول
        }

        public enum MemberType
        {
           superAdmin, Admin, Student, Account, Teacher, SuperTeacher, Leader
        }
        public virtual ICollection<MemberExpenses> expenses { get; set; }
        public virtual ICollection<DisposalDocument> disposals { get; set; }



        public enum Nathionality
        {
            السعودية, مصر, اليمن, سوريا, العراق, عمان, قطر, البحرين, الإمارات, الكويت, الهند, بنجلاديش, انجلترا, امريكا, فرنسا, تركيا, المانيا, كندا, فلسطين, الأردن, لبنان
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

      
        public System.Data.Entity.DbSet<ESchool.Models.Setting> Settings { get; set; }

        public System.Data.Entity.DbSet<ESchool.Models.marhala> marhalas { get; set; }

    //   public System.Data.Entity.DbSet<ESchool.Models.MemberData> MemberDatas { get; set; }

        public System.Data.Entity.DbSet<ESchool.Models.ClassRoom> ClassRooms { get; set; }

        public System.Data.Entity.DbSet<ESchool.Models.Nationality> Nationalities { get; set; }

        public System.Data.Entity.DbSet<ESchool.Models.sauf> saufs { get; set; }

        public System.Data.Entity.DbSet<ESchool.Models.DisposalDocument> DisposalDocuments { get; set; }

        public System.Data.Entity.DbSet<ESchool.Models.MemberExpenses> MemberExpenses { get; set; }

        public System.Data.Entity.DbSet<ESchool.Models.MemberClassYear> MemberClassYears { get; set; }
    }
}