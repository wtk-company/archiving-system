using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ArchiveProject2019.Resources;

namespace ArchiveProject2019.Models
{
    public class DocumentGroup
    {
        [Key]
        public int Id { get; set; }

        
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "CreatedAt", ResourceType = typeof(main_lang))]
        public string CreatedAt { get; set; }


        //Users Control:
        [Display(Name = "CreatedById", ResourceType = typeof(main_lang))]
        public string CreatedById { set; get; }
        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }


        [Display(Name = "EnableEdit", ResourceType = typeof(main_lang))]
        public bool EnableEdit { set; get; }
        [Display(Name = "امكانية الرد")]


        public bool EnableReplay { set; get; }
        [Display(Name = "EnableSeal", ResourceType = typeof(main_lang))]
        public bool EnableSeal { set; get; }


        [Display(Name = "امكانية الربط")]
        public bool EnableRelate { set; get; }


        [Display(Name = "UpdatedAt", ResourceType = typeof(main_lang))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string UpdatedAt { get; set; }


        [Display(Name = "DocumentId", ResourceType = typeof(main_lang))]
        public int DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public virtual Document document { get; set; }


        [Display(Name = "GroupId", ResourceType = typeof(main_lang))]
        public int GroupId { set; get; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
    }
}