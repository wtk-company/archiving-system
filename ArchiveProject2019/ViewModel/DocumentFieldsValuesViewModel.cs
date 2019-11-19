using ArchiveProject2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.ViewModel
{
    public class DocumentFieldsValuesViewModel
    {
        public Document Document { get; set; }
        public List<Field> Fields { get; set; }
        public List<Value> Values { get; set; }
        public List<FilesStoredInDb> FilesStoredInDbs { get; set; }
        public bool IsSaveInDb { get; set; }
        public List<SealDocument> Seals { get; set; }
    }
}