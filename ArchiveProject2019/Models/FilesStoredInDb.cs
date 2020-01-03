using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ArchiveProject2019.Resources;

namespace ArchiveProject2019.Models
{
    public class FilesStoredInDb
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "FileName", ResourceType = typeof(main_lang))]
        public string FileName { get; set; }


        [Display(Name = "File", ResourceType = typeof(main_lang))]
        public byte[] File { get; set; }


        // Relate with Document Table
        [Display(Name = "DocumentId", ResourceType = typeof(main_lang))]
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "DocumentRequired")]
        public int DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public Document Document { get; set; }
    }
}