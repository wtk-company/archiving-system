﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        public string Action { set; get; }

        [Display(Name = "اسم الصلاحية")]
        public string Name { set; get; }

        public ICollection<PermissionRole> PermissionRoles { set; get; }
       

    }
}