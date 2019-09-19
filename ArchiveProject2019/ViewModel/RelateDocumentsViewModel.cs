using ArchiveProject2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.ViewModel
{
    public class RelateDocumentsViewModel
    {
        public int DocId { get; set; }
        public List<RelatedDocumentViewModel> RelatedDocuments { get; set; }
    }
}