using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class Notification
    {
        public int Id { set; get; }
        [Display(Name = "تاريخ الإشعار")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string CreatedAt { get; set; }

        [Display(Name = "الإشعار")]

        public string Message { set; get; }
        public bool Active { set; get; }

        [Display(Name = "المستخدم")]
        public string UserId { set; get; }
        [ForeignKey("UserId")]
        public ApplicationUser User { set; get; }



        [Display(Name = "المنشأ")]
        public string NotificationOwnerId{ set; get; }
        [ForeignKey("NotificationOwnerId")]
        public ApplicationUser NotificationOwner { set; get; }

    }
}