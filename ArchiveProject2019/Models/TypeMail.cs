using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ArchiveProject2019.Resources;

namespace ArchiveProject2019.Models
{
    public class TypeMail
    {
        [Key]
        public int Id { get; set; }



        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "TypeMailNameRequired")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "NameLength")]
        [Display(Name = "TypeMailName", ResourceType = typeof(main_lang))]
        public string Name { get; set; }



        [Display(Name = "Type", ResourceType = typeof(main_lang))]
        public int Type { set; get; }



        [Display(Name = "CreatedAt", ResourceType = typeof(main_lang))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string CreatedAt { get; set; }



        [Display(Name = "CreatedById", ResourceType = typeof(main_lang))]
        public string CreatedById { set; get; }
        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }



        //Update Informations:
        [Display(Name = "UpdatedAt", ResourceType = typeof(main_lang))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string UpdatedAt { get; set; }



        [Display(Name = "UpdatedById", ResourceType = typeof(main_lang))]
        public string UpdatedById { set; get; }
        [ForeignKey("UpdatedById")]
        public ApplicationUser UpdatedBy { set; get; }



        /// 
        /// Collections:
        /// 
        public ICollection<Document> Documents { set; get; }
    }
}