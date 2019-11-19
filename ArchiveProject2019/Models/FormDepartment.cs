using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class FormDepartment
    {

        [Key]
        public int Id { get; set; }
        [Display(Name = "حالة التّفعيل")]
        public bool Is_Active { get; set; }

        [Display(Name = "اسم النموذج")]
        public int FormId { get; set; }

        [ForeignKey("FormId")]

        public virtual Form Form { get; set; }

        [Display(Name ="اسم القسم")]
        public int DepartmentId { set; get; }

        [ForeignKey("DepartmentId")]

        public virtual Department Department { get; set; }





        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "تاريخ الإنشاء")]
        public string CreatedAt { get; set; }

        //Users Control:
        [Display(Name = " اسم الشخص المنشىء ")]
        public string CreatedById { set; get; }

        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }



        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "تاريخ أخر تحديث")]
        public string Updatedat { get; set; }
        [Display(Name = "آخر تعديل  بواسطة")]
        public string UpdatedById { set; get; }

        [ForeignKey("UpdatedById")]
        public ApplicationUser UpdatedBy { set; get; }

    }
}