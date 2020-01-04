using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class FilesStoredInDb
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "اسم الملف")]
        public string FileName { get; set; }


        [Display(Name = "الملف")]
        public byte[] File { get; set; }


        // Relate with Document Table
        [Display(Name = "اسم الوثيقة")]
        [Required(ErrorMessage = "يجب إختيار الوثيقة")]
        public int DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public Document Document { get; set; }
    }
}