using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ArchiveProject2019.Resources;

namespace ArchiveProject2019.Models
{
    public class SealDocument
    {
        [Key]
        public int Id { get; set; }



        [Display(Name = "Message", ResourceType = typeof(main_lang))]
        [Required]
        public string Message { get; set; }



        [Display(Name = "FileName", ResourceType = typeof(main_lang))]
        public string FileName { get; set; }
        


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "CreatedAt", ResourceType = typeof(main_lang))]
        public string CreatedAt { get; set; }



        // Relate with User Table
        [Display(Name = "CreatedById", ResourceType = typeof(main_lang))]
        public string CreatedById { set; get; }
        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }



        // Relate with Document Table
        [Display(Name = "DocumentId", ResourceType = typeof(main_lang))]
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "DocumentRequired")]
        public int DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public Document Document { get; set; }


        
        // Files Collection
        public ICollection<SealFiles> Files { set; get; }
    }
}