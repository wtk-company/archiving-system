using ArchiveProject2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.ViewModel
{
    public class DocumentFieldsValuesViewModel
    {
        public int DocId { get; set; }
        public Document Document { get; set; }
        public FieldsValuesViewModel FieldsValues{set;get;}
    }
}