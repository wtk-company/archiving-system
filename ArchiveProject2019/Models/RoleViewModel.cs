using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class RoleViewModel
    {

        public string Id { set; get; }
        [Required(ErrorMessage = "يجب إدخال اسم الدور")]
        [Display(Name = "اسم الدور ")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "طول الدور يجب أن يكون بين 3 و 50 محرف")]

        public string Name { set; get; }
        
        [Display(Name = "تاريخ الإنشاء ")]

        public string CreatedAt { set; get; }
        [Display(Name = " آخر تاريخ تحديث ")]

        public string UpdatedAt { set; get; }
        [Display(Name = "اسم المنشىء ")]

        public string CreatedByFullName { set; get; }
        [Display(Name = " آخر تحديث بواسطة")]

        public string UpdatedByFullName { set; get; }
    }
}