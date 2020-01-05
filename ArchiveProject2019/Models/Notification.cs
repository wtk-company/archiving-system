using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ArchiveProject2019.Resources;

namespace ArchiveProject2019.Models
{
    public class Notification
    {
        public int Id { set; get; }



        [Display(Name = "CreatedAt", ResourceType = typeof(main_lang))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string CreatedAt { get; set; }


        [Display(Name = "Message", ResourceType = typeof(main_lang))]
        public string Message { set; get; }



        [Display(Name = "Is_Active", ResourceType = typeof(main_lang))]
        public bool Active { set; get; }



        [Display(Name = "UserId", ResourceType = typeof(main_lang))]
        public string UserId { set; get; }
        [ForeignKey("UserId")]
        public ApplicationUser User { set; get; }



        [Display(Name = "NotificationOwnerId", ResourceType = typeof(main_lang))]
        public string NotificationOwnerId{ set; get; }
        [ForeignKey("NotificationOwnerId")]
        public ApplicationUser NotificationOwner { set; get; }
    }
}