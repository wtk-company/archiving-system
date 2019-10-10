using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class Kind
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "يجب إدخال نوع الوثيقة")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "يجب أن يكون طول الاسم أكبر من 2")]

        [Display(Name = "نوع الوثيقة")]
        public string Name { get; set; }
        
        
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "تاريخ الإنشاء")]
        public string CreatedAt { get; set; }


        // Relate with User Table For Create By.
        [Display(Name = " أنشأ بواسطة ")]
        public string CreatedById { set; get; }
        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }


        [Display(Name = "تاريخ آخر تعديل ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string UpdatedAt { get; set; }
       
        //public ICollection<Document> Documents { set; get; }
    }
}