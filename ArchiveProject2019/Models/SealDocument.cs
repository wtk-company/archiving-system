using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class SealDocument
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "نص التسديد")]
        public string Message { get; set; }


        [Display(Name = "اسم الملف")]
        public string FileName { get; set; }


        [Display(Name = "الملف")]
        public byte[] File { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "تاريخ الإنشاء")]
        public string CreatedAt { get; set; }

        // Relate with User Table
        [Display(Name = " اسم الشخص المنشىء ")]
        public string CreatedById { set; get; }
        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }

        // Relate with Document Table
        [Display(Name = "اسم الوثيقة")]
        [Required(ErrorMessage = "يجب إختيار الوثيقة")]
        public int DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public Document Document { get; set; }
    }
}