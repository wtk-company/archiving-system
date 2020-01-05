using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ArchiveProject2019.Resources;

namespace ArchiveProject2019.Models
{
    public class RoleViewModel
    {
        public string Id { set; get; }



        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "RoleNameRequired")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "NameLength")]
        [Display(Name = "RoleName", ResourceType = typeof(main_lang))]
        public string Name { set; get; }

        

        [Display(Name = "CreatedAt", ResourceType = typeof(main_lang))]
        public string CreatedAt { set; get; }


        
        [Display(Name = "UpdatedAt", ResourceType = typeof(main_lang))]
        public string UpdatedAt { set; get; }
        


        [Display(Name = "CreatedById", ResourceType = typeof(main_lang))]
        public string CreatedByFullName { set; get; }
        


        [Display(Name = "UpdatedById", ResourceType = typeof(main_lang))]
        public string UpdatedByFullName { set; get; }
    }
}