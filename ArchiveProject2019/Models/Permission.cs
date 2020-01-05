using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ArchiveProject2019.Resources;

namespace ArchiveProject2019.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }



        public string Action { set; get; }



        [Display(Name = "PermissionName", ResourceType = typeof(main_lang))]
        public string Name { set; get; }



        public bool TypeUser { set; get; }



        public bool TypeMaster { set; get; }



        public ICollection<PermissionRole> PermissionRoles { set; get; }
    }
}