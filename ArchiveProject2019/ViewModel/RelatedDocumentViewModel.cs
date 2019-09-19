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
        public string Form { get; set; }

        public string Department { get; set; }
        
        public string Number { get; set; }

        public string Subject { get; set; }
        
        public bool IsRelated { get; set; }
    }
}