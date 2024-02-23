using ESchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using System.Data.Entity.Validation;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(ESchool.Startup))]
namespace ESchool
{
    public partial class Startup
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            
            ConfigureAuth(app);
            CreateRoles();  
            CreateAdministrator();

        }

        public void CreateAdministrator()
        {
            try
            {
                var superAdmin = db.Users.Where(u => u.UserName.Equals("2313733046")).FirstOrDefault();
                if (superAdmin == null)
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                    var user = new ApplicationUser();
                    user.UserName = "1121131140";
                    user.Name = "admin admin";
                   	    user.Dob = "01/16/1985";
                    user.EnglishName = "admin admin";
                    user.IdentityId = "1121131140";
                    user.PhoneNumber = "0534426545";
                    user.PhoneNumberConfirmed = true;
                    user.Email = "mahmoudfayezhussien@gmail.com";
                    user.EmailConfirmed = true;
                    user.Mobile1 = "966534426545";
                    user.nathionality = ApplicationUser.Nathionality.مصر;
                	      user.TwoFactorEnabled = false;
                    var check = userManager.Create(user, "Qa123456@");
                    if (check.Succeeded)
                    {
                        userManager.AddToRole(user.Id, "superAdmin");
                    }
                    db.SaveChanges();
                }

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

        }
        public void CreateRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            IdentityRole role;
            if (!roleManager.RoleExists("superAdmin"))
            {
                role = new IdentityRole();
                role.Name = "superAdmin";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Admin"))
            {
                role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Student"))
            {
                role = new IdentityRole();
                role.Name = "Student";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Account"))
            {
                role = new IdentityRole();
                role.Name = "Account";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Teacher"))
            {
                role = new IdentityRole();
                role.Name = "Teacher";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("SuperTeacher"))
            {
                role = new IdentityRole();
                role.Name = "SuperTeacher";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Leader"))
            {
                role = new IdentityRole();
                role.Name = "Leader";
                roleManager.Create(role);
            }
        }
    }
}
