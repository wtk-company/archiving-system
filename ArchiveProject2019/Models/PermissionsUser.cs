using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class PermissionsUser
    {


       
        [Key]
        public int Id { get; set; }
        [Display(Name = "حالة التّفعيل")]
        public bool Is_Active { get; set; }
        [Display(Name = "الصلاحية")]
        public int PermissionId { get; set; }
        [Display(Name = "المستخدم")]

        public string UserId { get; set; }

        [ForeignKey("PermissionId")]

        public virtual Permission Permission { get; set; }

        [ForeignKey("UserId")]

        public virtual ApplicationUser User { get; set; }



        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "تاريخ  تعديل التفعيل ")]
        public string CreatedAt { get; set; }

       



    }
}