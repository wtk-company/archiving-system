using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ArchiveProject2019.Resources;


namespace ArchiveProject2019.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {


        [Display(Name = "FullName", ResourceType = typeof(main_lang))]
        public string FullName { set; get; }


        [Display(Name = "Gender", ResourceType = typeof(main_lang))]
        public string Gender { set; get; }



        [Display(Name = "DepartmentId", ResourceType = typeof(main_lang))]
        public int? DepartmentId { set; get; }
        [ForeignKey("DepartmentId")]
        public virtual Department Department { set; get; }



        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "CreatedAt", ResourceType = typeof(main_lang))]
        public string CreatedAt { get; set; }



        //Users Control:
        [Display(Name = "CreatedById", ResourceType = typeof(main_lang))]
        public string CreatedById { set; get; }
        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }



        [Display(Name = "RoleName", ResourceType = typeof(main_lang))]
        public string RoleName { set; get; }



        [Display(Name = "JobTitleId", ResourceType = typeof(main_lang))]
        public int? JobTitleId { set; get; }
        [ForeignKey("JobTitleId")]
        public virtual JobTitle jobTitle { set; get; }



        [Display(Name = "UpdatedAt", ResourceType = typeof(main_lang))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string UpdatedAt { get; set; }




        [Display(Name = "UpdatedById", ResourceType = typeof(main_lang))]
        public string UpdatedByID { set; get; }


        public bool IsDefaultMaster { set; get; }




        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public class ApplicationRoles : IdentityRole
    {
        //Collection of Permissions and users
        public ICollection<PermissionRole> PermissionRoles { set; get; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "CreatedAt", ResourceType = typeof(main_lang))]
        public string CreatedAt { get; set; }

        //Users Control:
        [Display(Name = "CreatedById", ResourceType = typeof(main_lang))]
        public string CreatedById { set; get; }
        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }



        //Update Details:
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "UpdatedAt", ResourceType = typeof(main_lang))]
        public string UpdatedAt { get; set; }

        [Display(Name = "UpdatedById", ResourceType = typeof(main_lang))]
        public string UpdatedById { set; get; }
        [ForeignKey("UpdatedById")]
        public ApplicationUser UpdatedBy { set; get; }



    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Department> Departments { get; set; }

        public DbSet<Form> Forms { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<Field> Fields { get; set; }

        public DbSet<Value> Values { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<PermissionRole> PermissionRoles { get; set; }
        public DbSet<PermissionsUser> PermissionUsers { get; set; }


        public DbSet<JobTitle> JobTitles { set; get; }
        public DbSet<FavouriteForms> FavouriteForms { set; get; }



        public DbSet<TypeMail> TypeMails { set; get; }
        public DbSet<DocumentStatus> DocumentStatuses { set; get; }


        public DbSet<Group> Groups { set; get; }

        public DbSet<UserGroup> UsersGroups { set; get; }

        public DbSet<Kind> Kinds { set; get; }

        public DbSet<Party> Parties { set; get; }
        public DbSet<RelatedDocument> RelatedDocuments { set; get; }
        public DbSet<FormDepartment> FormDepartments { set; get; }
        public DbSet<FormGroup> FormGroups { set; get; }

        public DbSet<DocumentDepartment> DocumentDepartments { set; get; }
        public DbSet<DocumentGroup> DocumentGroups { set; get; }
        public DbSet<DocumentUser> DocumentUsers { set; get; }


        // Target Parties for Outgoing Mial
        public DbSet<DocumentParty> DocumentParties { set; get; }
        
        public DbSet<Notification> Notifications { set; get; }

        public DbSet<FilesStoredInDb> FilesStoredInDbs { set; get; }


        public DbSet<ReplayDocument> ReplayDocuments { set; get; }

        public DbSet<SealDocument> SealDocuments { set; get; }

        // Company Info 
        public DbSet<Company> Company { set; get; }

        public DbSet<SealFiles> SealFiles { set; get; }
        



        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}