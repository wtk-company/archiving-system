using ArchiveProject2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.HelperClasses
{
    public class UserRelatedDocumentsId
    {

        public static IEnumerable<int> UserCreatedDocument(string UserID)
        {


            ApplicationDbContext db = new ApplicationDbContext();

            IEnumerable<int> MyDoc = db.Documents.Where(a => a.CreatedById.Equals(UserID)).Select(a => a.Id);
            return MyDoc;

        }


        public static IEnumerable<int> UserDepartmentDocument(string UserID)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            int _DepId = db.Users.Find(UserID).DepartmentId.HasValue ? db.Users.Find(UserID).DepartmentId.Value : -1;
            IEnumerable<int> myDoc = db.DocumentDepartments.Where(a => a.DepartmentId == _DepId &&a.EnableRelate==true).Select(a => a.DocumentId);
            return myDoc;

        }


        public static IEnumerable<int> UserDocumentGroups(string UserID)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            IEnumerable<int> GroupId = db.UsersGroups.Where(a => a.UserId.Equals(UserID)).Select(a => a.GroupId);
            IEnumerable<int> myDoc = db.DocumentGroups.Where(a => GroupId.Contains(a.GroupId
                )&&a.EnableRelate==true).Select(a => a.DocumentId);
            return myDoc;

        }




        public static IEnumerable<int> UserAllDocuments(string UserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            IEnumerable<int> Doc1 = UserCreatedDocument(UserId);
            IEnumerable<int> Doc2 = UserDepartmentDocument(UserId);
            IEnumerable<int> Doc3 = UserDocumentGroups(UserId);


            IEnumerable<int> AllDoc = Doc1.Union(Doc2).Union(Doc3);


            IEnumerable<int> RelateDocument = db.RelatedDocuments.Select(a => a.RelatedDocId).ToList();
            AllDoc = AllDoc.Except(RelateDocument).ToList(); ;
            return AllDoc;



        }



   
    }
}