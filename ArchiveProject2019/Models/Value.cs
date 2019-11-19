using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class Value
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "قيمة الحقل")]
        public string FieldValue { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        public string CreatedAt { get; set; }

        [Display(Name = " أنشأ بواسطة ")]
        public string CreatedById { set; get; }
        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }



        // relate with Field Table
        [Display(Name = "اسم الحقل")]
        public int FieldId { get; set; }
        public Field Field { get; set; }

        
        // Relate with Document Table
        [Display(Name = "اسم الوثيقة")]
        public int Document_id { get; set; }
        [ForeignKey("Document_id")]
        public Document Docuemnt { get; set; }
    }
}