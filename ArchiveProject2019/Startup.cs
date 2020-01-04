using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArchiveProject2019.Models;
using System;
using System.Collections.Generic;
using ArchiveProject2019.HelperClasses;

[assembly: OwinStartupAttribute(typeof(ArchiveProject2019.Startup))]
namespace ArchiveProject2019
{
    public partial class Startup
    {
        //Connection
        ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
            // createrole();
            createPermissions();

            CreateMasterRole();

            CreateMasterUser();
            CreateTypeOfMails();
            CreateDocumentStatus();
            CreateForm();

        }

        public void CreateMasterRole()
        {
            RoleManager<ApplicationRoles> roleManager = new RoleManager<ApplicationRoles>(new RoleStore<ApplicationRoles>(db));

            ApplicationRoles role=new ApplicationRoles();
            if(!roleManager.RoleExists("Master"))
            {
                role. Name = "Master";
                    
                

                roleManager.Create(role);

                PermissionRole prole;
                //Add All Permission to Super Admin
                IEnumerable<Permission> Permissions = db.Permissions.Where(a=>a.TypeMaster==true);
                foreach (Permission myPermission in Permissions)
                {

                    prole = new PermissionRole()
                    {

                        RoleId = role.Id,
                        PermissionId = myPermission.Id,
                        Is_Active = true
                    };

                    db.PermissionRoles.Add(prole);

                }
            }
        

          
            db.SaveChanges();

        }


        public void createPermissions()
        {

            //List Of Permissions Of the project:
            IEnumerable<Permission> PermissionsDb = db.Permissions;
            Permission per;
            IEnumerable<PermissionsControls> Permissions = PermissionsControls.PermissionsStartUp();
            foreach(PermissionsControls myPermission in Permissions)
            {
                //Add Permission To DB:
                if(!PermissionsDb.Any(a=>a.Action.Equals(myPermission.ActionName,StringComparison.OrdinalIgnoreCase)))
                {
                    per = new Permission() { Name=myPermission.Name,Action=myPermission.ActionName,TypeUser=myPermission.Type,
                    TypeMaster=myPermission.TypeMaster};

                    db.Permissions.Add(per);

                }

            }
            db.SaveChanges();

           
        }


        public void CreateMasterUser() {
            //Create User
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser user = new ApplicationUser() {
                FullName = "Super Admin",
                //IdentityNumber = "123456789123456",
                Gender = "Male",
                CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                UserName = "masteruser",
                RoleName = "Master",
                IsDefaultMaster = true
                
               

            };
            //Default User Not found
            if (!db.Users.Any(a=>a.RoleName.Equals("Master",StringComparison.OrdinalIgnoreCase))) {

                var Check = userManager.Create(user, "master123s");
                if (Check.Succeeded)
                {
                    //Add Default User
                    userManager.AddToRole(user.Id, "Master");
                }

            }


        }


        public void CreateTypeOfMails()
        {

            IEnumerable<TypeMail> Mails = TypeOfMailStartup.GetTypes();
            foreach(TypeMail m in Mails)
            {
                if(!db.TypeMails.Any(a=>a.Name.Equals(m.Name)))
                {
                    db.TypeMails.Add(m);
                }

            }

            db.SaveChanges();
        }



        public void CreateDocumentStatus()
        {

            IEnumerable<DocumentStatus> status = DocumentStatusStartUp.DocumentStatusList();
            foreach (DocumentStatus m in status)
            {
                if (!db.DocumentStatuses.Any(a => a.Name.Equals(m.Name)))
                {
                    db.DocumentStatuses.Add(m);
                }

            }

            db.SaveChanges();
        }



        public void CreateForm()
        {

            if(!db.Forms.Any(a=>a.Type==1))
            {
                db.Forms.Add(FormStartUp.CreateFormStartUp());
                db.SaveChanges();
            }
          
        }
    }


   
}
