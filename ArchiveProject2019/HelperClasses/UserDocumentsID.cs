using ArchiveProject2019.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.HelperClasses
{
    public class UserDocumentsID
    {

        public static IEnumerable<int>UserCreatedDocument(string UserID)
        {


            ApplicationDbContext db = new ApplicationDbContext();

            IEnumerable<int> MyDoc = db.Documents.Where(a => a.CreatedById.Equals(UserID)).Select(a=>a.Id);
            return MyDoc;

        }


        public static IEnumerable<int>UserDepartmentDocument(string UserID)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            int _DepId = db.Users.Find(UserID).DepartmentId.HasValue?db.Users.Find(UserID).DepartmentId.Value:-1;
            IEnumerable<int> myDoc = db.DocumentDepartments.Where(a => a.DepartmentId == _DepId).Select(a => a.DocumentId);
            return myDoc;

        }


        public static IEnumerable<int> UserDocumentGroups(string UserID)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            IEnumerable<int> GroupId = db.UsersGroups.Where(a => a.UserId.Equals(UserID)).Select(a => a.GroupId);
            IEnumerable<int> myDoc = db.DocumentGroups.Where(a => GroupId.Contains(a.GroupId
                )).Select(a => a.DocumentId);
            return myDoc;

        }


        public static IEnumerable<int> UserDeocumentNotification (string UserID)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            IEnumerable<int> myDoc = db.Documents.Where(a => a.NotificationUserId.Equals(UserID) && a.NotificationDate != null).Select(a=>a.Id);
            return myDoc;

        }



        public static IEnumerable<int> UserAllDocuments(string UserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            IEnumerable<int> Doc1 = UserCreatedDocument(UserId);
            IEnumerable<int> Doc2 = UserDepartmentDocument(UserId);
            IEnumerable<int> Doc3 = UserDocumentGroups(UserId);
            IEnumerable<int> Doc4 = UserDeocumentNotification(UserId);

            IEnumerable<int> AllDoc = Doc1.Union(Doc2).Union(Doc3).Union(Doc4);
            return AllDoc;



        }



        //Related Document:::
        public static IEnumerable<int> UserRelateDocument(string UserID,int DocId)
        {


            ApplicationDbContext db = new ApplicationDbContext();
            List<int> RelateDocumentId = db.RelatedDocuments.Where(a => a.Document_id == DocId).Select(a=>a.RelatedDocId).ToList();
            IEnumerable<int> MyDoc1 = db.Documents.Where(a =>RelateDocumentId.Contains(a.Id)&& a.CreatedById.Equals(UserID)).Select(a => a.Id);


            //Department:
            int _DepId = db.Users.Find(UserID).DepartmentId.HasValue ? db.Users.Find(UserID).DepartmentId.Value : -1;
            IEnumerable<int> myDoc2 = db.DocumentDepartments.Where(a => RelateDocumentId.Contains(a.Id)&& a.DepartmentId == _DepId).Select(a => a.DocumentId);


            //Groups :

            IEnumerable<int> GroupId = db.UsersGroups.Where(a => a.UserId.Equals(UserID)).Select(a => a.GroupId);
            IEnumerable<int> myDoc3 = db.DocumentGroups.Where(a =>RelateDocumentId.Contains(a.Id)&& GroupId.Contains(a.GroupId
                )).Select(a => a.DocumentId);


            List<int> MyDoc = MyDoc1.Union(myDoc2).Union(myDoc3).ToList();
            return MyDoc;

        }



        //Related Document:::
        public static IEnumerable<int> UserReplayDocument(string UserID, int DocId)
        {


            ApplicationDbContext db = new ApplicationDbContext();
            List<int> ReplayDocumentId = db.ReplayDocuments.Where(a => a.Document_id == DocId).Select(a => a.ReplayDocId).ToList();
            IEnumerable<int> MyDoc1 = db.Documents.Where(a => ReplayDocumentId.Contains(a.Id) && a.CreatedById.Equals(UserID)).Select(a => a.Id);


            //Department:
            int _DepId = db.Users.Find(UserID).DepartmentId.HasValue ? db.Users.Find(UserID).DepartmentId.Value : -1;
            IEnumerable<int> myDoc2 = db.DocumentDepartments.Where(a => ReplayDocumentId.Contains(a.Id) && a.DepartmentId == _DepId).Select(a => a.DocumentId);


            //Groups :

            IEnumerable<int> GroupId = db.UsersGroups.Where(a => a.UserId.Equals(UserID)).Select(a => a.GroupId);
            IEnumerable<int> myDoc3 = db.DocumentGroups.Where(a => ReplayDocumentId.Contains(a.Id) && GroupId.Contains(a.GroupId
                )).Select(a => a.DocumentId);


            List<int> MyDoc = MyDoc1.Union(myDoc2).Union(myDoc3).ToList();
            return MyDoc;

        }

    }
}