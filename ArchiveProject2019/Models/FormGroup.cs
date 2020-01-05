using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ArchiveProject2019.Resources;

namespace ArchiveProject2019.Models
{
    public class FormGroup
    {

        [Key]
        public int Id { get; set; }


        [Display(Name = "Is_Active", ResourceType = typeof(main_lang))]
        public bool Is_Active { get; set; }


        [Display(Name = "FormId", ResourceType = typeof(main_lang))]
        public int FormId { get; set; }
        [ForeignKey("FormId")]
        public virtual Form Form { get; set; }


        [Display(Name = "GroupId", ResourceType = typeof(main_lang))]
        public int GroupId { set; get; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }

        
        [Display(Name = "CreatedAt", ResourceType = typeof(main_lang))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string CreatedAt { get; set; }


        //Users Control:
        [Display(Name = "CreatedById", ResourceType = typeof(main_lang))]
        public string CreatedById { set; get; }
        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }


        [Display(Name = "UpdatedAt", ResourceType = typeof(main_lang))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string Updatedat { get; set; }


        [Display(Name = "UpdatedById", ResourceType = typeof(main_lang))]
        public string UpdatedById { set; get; }
        [ForeignKey("UpdatedById")]
        public ApplicationUser UpdatedBy { set; get; }
    }
}