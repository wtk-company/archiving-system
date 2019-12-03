using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.ViewModel
{
    public class RelatedDocumentViewModel
    {
        public int Id { get; set; }
        [Display(Name ="النموذج")]
        public string Form { get; set; }
        [Display(Name ="رقم الوثيقة")]


        public string Number { get; set; }
        [Display(Name = "الموضوع")]


        public string Subject { get; set; }
        [Display(Name = "ربط؟")]


        public bool IsRelated { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "تاريخ الإنشاء")]
        public string CreatedAt { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "تاريخ الوثيقة")]
        public string DocumentData { get; set; }


    }
}