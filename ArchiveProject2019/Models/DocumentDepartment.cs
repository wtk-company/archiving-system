using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class DocumentDepartment
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "الوثيقة")]
        public int DocumentId { get; set; }

        [ForeignKey("DocumentId")]

        public virtual Document document { get; set; }

        [Display(Name = "اسم القسم")]
        public int DepartmentId { set; get; }

        [ForeignKey("DepartmentId")]

        public virtual Department Department { get; set; }





        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "تاريخ الإنشاء")]
        public string CreatedAt { get; set; }

        //Users Control:
        [Display(Name = " اسم الشخص المنشىء ")]
        public string CreatedById { set; get; }

        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }


      

    }
}