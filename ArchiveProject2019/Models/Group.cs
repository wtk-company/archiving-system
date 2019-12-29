using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يجب إدخال اسم المجموعة")]
        [Display(Name = "اسم المجموعة")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "يجب أن يكون طول الاسم أكبر من 2")]

        public string Name { get; set; }

     //   [Required(ErrorMessage = "يجب إدخال الوصف")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "طول الوصف يجب أن يكون على الأقل 3 محارف")]

        [Display(Name = "الوصف")]
        public string Description { get; set; }




        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "تاريخ الإنشاء")]
        public string CreatedAt { get; set; }
        
        [Display(Name = " أنشأ بواسطة ")]
        public string CreatedById { set; get; }

        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }




        //Update Informations:

        [Display(Name = "تاريخ آخر تعديل ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string UpdatedAt { get; set; }
        [Display(Name = "آخر تعديل  بواسطة")]
        public string UpdatedById { set; get; }

        [ForeignKey("UpdatedById")]
        public ApplicationUser UpdatedBy { set; get; }


        public ICollection<UserGroup> UsersGroup { set; get; }
    }
}