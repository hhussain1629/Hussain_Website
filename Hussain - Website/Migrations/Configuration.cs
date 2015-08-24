
namespace Hussain___Website.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Hussain___Website.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Hussain___Website.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Hussain___Website.Models.ApplicationDbContext";
        }

        protected override void Seed(Hussain___Website.Models.ApplicationDbContext context)
        {
            var roleManager = new Microsoft.AspNet.Identity.RoleManager<IdentityRole>(
                new Microsoft.AspNet.Identity.EntityFramework.RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Mod"))
            {
                roleManager.Create(new IdentityRole { Name = "Mod" });
            }
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "hhussain1629@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "hhussain1629@gmail.com",
                    Email = "hhussain1629@gmail.com",
                    FirstName = "Hammad",
                    LastName = "Hussain",
                    DisplayName = "Hammad"
                }, "password");
            }
            var userId = userManager.FindByEmail("hhussain1629@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");

            if (!context.Users.Any(u => u.Email == "ajensen@coderfoundry"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ajensen@coderfoundry",
                    Email = "ajensen@coderfoundry",
                    FirstName = "Andrew",
                    LastName = "Jensen",
                    DisplayName = "Andrew"
                }, "password");
            }

            userId = userManager.FindByEmail("ajensen@coderfoundry").Id;
            userManager.AddToRole(userId, "Mod");

            if (!context.Users.Any(u => u.Email == "araynor@coderfoundry"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "araynor@coderfoundry",
                    Email = "araynor@coderfoundry",
                    FirstName = "Antonio",
                    LastName = "Raynor",
                    DisplayName = "Antonio"
                }, "password");
            }

            userId = userManager.FindByEmail("araynor@coderfoundry").Id;
            userManager.AddToRole(userId, "Mod");
        }

        //      context.People.AddOrUpdate(
        //      p => p.FullName,
        //      new Person { FullName = "Andrew Peters" },
        //      new Person { FullName = "Brice Lambson" },
        //      new Person { FullName = "Rowan Miller" }
        //    );
        //
    }
}
