using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ArchiveProject2019.Resources;

namespace ArchiveProject2019.Models
{
    public class DocumentParty
    {
        [Key]
        public int Id { get; set; }
        

        [Display(Name = "CreatedAt", ResourceType = typeof(main_lang))]
        public string CreatedAt { get; set; }


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


        // Relate with Party Table
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "PartyRequired")]
        [Display(Name = "PartyId", ResourceType = typeof(main_lang))]
        public int PartyId { get; set; }
        [ForeignKey("PartyId")]
        public Party Party { get; set; }
    }
}