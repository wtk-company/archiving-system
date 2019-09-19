using ArchiveProject2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.ViewModel
{
    public class DocumentDocumentsViewModel
    {
        public int DocId { get; set; }
        public List<Document> Documents { get; set; }
    }
}