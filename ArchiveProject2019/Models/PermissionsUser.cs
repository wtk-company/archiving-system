using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ArchiveProject2019.Resources;

namespace ArchiveProject2019.Models
{
    public class PermissionsUser
    {
        [Key]
        public int Id { get; set; }



        [Display(Name = "Is_Active", ResourceType = typeof(main_lang))]
        public bool Is_Active { get; set; }



        [Display(Name = "PermissionId", ResourceType = typeof(main_lang))]
        public int PermissionId { get; set; }
        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }



        [Display(Name = "UserId", ResourceType = typeof(main_lang))]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }



        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "CreatedAt", ResourceType = typeof(main_lang))]
        public string CreatedAt { get; set; }
    }
}