using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ArchiveProject2019.Resources;

namespace ArchiveProject2019.Models
{
    public class SealFiles
    {
        [Key]
        public int Id { get; set; }



        [Display(Name = "FileName", ResourceType = typeof(main_lang))]
        public string FileName { get; set; }




        [Display(Name = "File", ResourceType = typeof(main_lang))]
        public byte[] File { get; set; }



        [Display(Name = "FileUrl", ResourceType = typeof(main_lang))]
        public string FileUrl { get; set; }



        // Relate with SealDocument Table
        [Display(Name = "SealId", ResourceType = typeof(main_lang))]
        [Required(ErrorMessage = "يجب إختيار الوثيقة")]
        public int SealId { get; set; }
        [ForeignKey("SealId")]
        public SealDocument Seal { get; set; }
    }
}