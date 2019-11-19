using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class TypeMail
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "نوع البريد")]
        [Required(ErrorMessage = "يجب إدخال الاسم ")]
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "يجب أن يكون طول الاسم أكبر من 2")]
        public string Name { get; set; }


        public int Type { set; get; }
        
        [Display(Name = "تاريخ الإنشاء")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string CreatedAt { get; set; }
        [Display(Name = "تم الإنشاء بواسطة ")]
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



        public ICollection<Document> Documents { set; get; }

    }
}