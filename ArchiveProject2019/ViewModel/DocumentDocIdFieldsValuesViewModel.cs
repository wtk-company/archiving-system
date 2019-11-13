using ArchiveProject2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.ViewModel
{
    public class DocumentDocIdFieldsValuesViewModel
    {
        public int DocId { get; set; }
        public Document Document { get; set; }
        public FieldsValuesViewModel FieldsValues{set;get;}
        public List<bool> ExistFiles{ get; set; }
        public List<FilesStoredInDb> FilesStoredInDbs { get; set; }
        public int TypeMail { get; set; }
        public bool IsSaveInDb { get; set; }
        public bool IsReplay { get; set; }
    }
}