using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class DocumentGroup
    {



        [Key]
        public int Id { get; set; }
  

        [Display(Name = "اسم الوثيقة")]
        public int DocumentId { get; set; }

        [ForeignKey("DocumentId")]

        public virtual Document document { get; set; }

        [Display(Name = "اسم المجموعة")]
        public int GroupId { set; get; }

        [ForeignKey("GroupId")]

        public virtual Group Group { get; set; }


        
   


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "تاريخ الإنشاء")]
        public string CreatedAt { get; set; }

        //Users Control:
        [Display(Name = " اسم الشخص المنشىء ")]
        public string CreatedById { set; get; }

        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }


        [Display(Name = "امكانية التعديل")]

        public bool EnableEdit { set; get; }

        [Display(Name = "امكانية الرد")]

        public bool EnableReplay { set; get; }

        [Display(Name = "امكانية التسديد")]

        public bool EnableSeal { set; get; }


        [Display(Name = "امكانية الربط")]

        public bool EnableRelate { set; get; }



        [Display(Name = "تاريخ آخر تعديل ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string UpdatedAt { get; set; }
    }
}