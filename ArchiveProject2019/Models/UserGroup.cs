using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class UserGroup
    {
        [Key]
        public int Id { get; set; }


   


        [Display(Name = "تاريخ التضمين")]
        public string CreatedAt { get; set; }


        [Display(Name = " أسند بواسطة ")]
        public string CreatedById { set; get; }
        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }




        [Display(Name = "تاريخ آخر تعديل ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string UpdatedAt { get; set; }
        [Display(Name = "آخر تعديل  بواسطة ")]

        public string UpdatedById { set; get; }

        [ForeignKey("UpdatedById")]
        public ApplicationUser UpdatedBy { set; get; }


        // relate with Group Table
        [Display(Name = "اسم المجموعة")]
        public int GroupId { get; set; }
        public Group Group { get; set; }


        // Relate with User Table
        [Display(Name = "اسم المستخدم الثلاثي ")]
        public string UserId { get; set; }




        [ForeignKey("UserId")]


        public virtual ApplicationUser User { get; set; }
    }
}