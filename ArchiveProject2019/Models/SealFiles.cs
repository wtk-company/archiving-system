using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class SealFiles
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "اسم الملف")]
        public string FileName { get; set; }


        [Display(Name = "الملف")]
        public byte[] File { get; set; }

        [Display(Name = "الملف")]
        public string FileUrl { get; set; }

        // Relate with SealDocument Table
        [Display(Name = "اسم الوثيقة")]
        [Required(ErrorMessage = "يجب إختيار الوثيقة")]
        public int SealId { get; set; }
        [ForeignKey("SealId")]
        public SealDocument Seal { get; set; }
    }
}