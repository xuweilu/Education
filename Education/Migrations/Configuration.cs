namespace Education.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Education.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Education.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            if (!roleManager.RoleExists(Role.Student))
            {
                roleManager.Create(new IdentityRole { Name = Role.Student });
            }
            if (!roleManager.RoleExists(Role.Teacher))
            {
                roleManager.Create(new IdentityRole { Name = Role.Teacher });
            }
            if (!roleManager.RoleExists(Role.Administrator))
            {
                roleManager.Create(new IdentityRole { Name = Role.Administrator });
            }
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            if(!context.Users.Any(u => u.UserName == "Admin"))
            {
                Administrator admin = new Administrator { UserName = "Admin", Email = "admin@admin.com" };
                userManager.Create(admin, "Admin@123");
                userManager.AddToRole(admin.Id, Role.Administrator);
            }
            if(!context.Users.Any(u => u.UserName == "TestStudent"))
            {
                Student student = new Student { UserName = "TestStudent", Email = "teststudent@test.com" };
                userManager.Create(student, "Test@123.com");
                userManager.AddToRole(student.Id, Role.Student);
            }
            if(!context.Users.Any(u => u.UserName == "TestTeacher"))
            {
                Teacher teacher = new Teacher { UserName = "TestTeacher", Email = "testteacher@test.com" };
                userManager.Create(teacher, "Test@123.com");
                userManager.AddToRole(teacher.Id, Role.Teacher);
            }
        }
    }
}
