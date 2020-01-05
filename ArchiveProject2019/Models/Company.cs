using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ArchiveProject2019.Resources;

namespace ArchiveProject2019.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }



        [Display(Name = "CompanyName", ResourceType = typeof(main_lang))]
        public string Name { get; set; }



        [Display(Name = "CompanyDescription", ResourceType = typeof(main_lang))]
        public string Description { get; set; }



        [Display(Name = "CompanyAddress", ResourceType = typeof(main_lang))]
        public string Address { get; set; }


        [DataType(DataType.PhoneNumber)]
        [Display(Name = "PhoneNumber1", ResourceType = typeof(main_lang))]
        public string PhoneNumber1 { get; set; }



        [Display(Name = "PhoneNumber2", ResourceType = typeof(main_lang))]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber2 { get; set; }



        [Display(Name = "MobileNumber1", ResourceType = typeof(main_lang))]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "رقم الموبايل غير صحيح.")]
        public string MobileNumber1 { get; set; }



        [Display(Name = "MobileNumber2", ResourceType = typeof(main_lang))]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "رقم الموبايل غير صحيح.")]
        public string MobileNumber2 { get; set; }



        [EmailAddress(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "EmailRequired")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email", ResourceType = typeof(main_lang))]
        public string Email { get; set; }



        [Display(Name = "Logo", ResourceType = typeof(main_lang))]
        public byte[] Logo { get; set; }



        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "CompanyDate", ResourceType = typeof(main_lang))]
        public string CompanyDate { get; set; }




        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "CreatedAt", ResourceType = typeof(main_lang))]
        public string CreatedAt { get; set; }



        // Relate with User Table For Create By.
        [Display(Name = "CreateById", ResourceType = typeof(main_lang))]
        public string CreateById { get; set; }
        [ForeignKey("CreateById")]
        public ApplicationUser User { get; set; }
    }
}