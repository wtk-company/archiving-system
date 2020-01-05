using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ArchiveProject2019.Resources;

namespace ArchiveProject2019.Models
{
    public class PermissionRole
    {
        [Key]
        public int Id { get; set; }



        [Display(Name = "Is_Active", ResourceType = typeof(main_lang))]
        public bool Is_Active { get; set; }



        [Display(Name = "PermissionId", ResourceType = typeof(main_lang))]
        public int PermissionId { get; set; }
        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }



        [Display(Name = "RoleId", ResourceType = typeof(main_lang))]
        public string RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual ApplicationRoles Role { get; set; }



        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "CreatedAt", ResourceType = typeof(main_lang))]
        public string CreatedAt { get; set; }



        //Users Control:
        [Display(Name = "CreatedById", ResourceType = typeof(main_lang))]
        public string CreatedById { set; get; }
        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }



        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "UpdatedAt", ResourceType = typeof(main_lang))]
        public string Updatedat { get; set; }



        [Display(Name = "UpdatedById", ResourceType = typeof(main_lang))]
        public string UpdatedById { set; get; }
        [ForeignKey("UpdatedById")]
        public ApplicationUser UpdatedBy { set; get; }
    }
}