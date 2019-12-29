using ArchiveProject2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.HelperClasses
{
    public class DocumentOperation
    {



        public static bool DocumentHasChild( int DocumentId)
        {


            ApplicationDbContext db = new ApplicationDbContext();


            if (db.RelatedDocuments.Any(a => a.Document_id == DocumentId))
            {
                return true;
            }

            return false;
        }


        public static bool DocumentHasParent(int DocumentId)
        {


            ApplicationDbContext db = new ApplicationDbContext();


            if (db.RelatedDocuments.Any(a => a.RelatedDocId == DocumentId))
            {
                return true;
            }

            return false;
        }


        public static bool DocumentRemoveChild(string UID,int DocumentId)
        {


            if (DocumentCanRelate(UID, DocumentId) == false)
                return false;

            if (DocumentHasChild(DocumentId) == false)
            {
                return false;
            }



            return true;
        }


        public static bool DocumentRemoveParent(string UID,int DocumentId)
        {

            if (DocumentCanRelate(UID, DocumentId) == false)
            {

                return false;
            }

            if(DocumentHasParent(DocumentId)==false)
            {
                return false;
            }

           

            return true;
        }



        public static bool DocumentCanDelete(string UserId, int DocumentId)
        {

            //Create || Response:
            ApplicationDbContext db = new ApplicationDbContext();
            if (!db.Documents.Find(DocumentId).ResponsibleUserId.Equals(UserId))
            {
                return false;
            }

            //Related Documents:

            if(db.RelatedDocuments.Any(a=>a.Document_id==DocumentId))
            {
                return false;
            }

            if (db.ReplayDocuments.Any(a => a.Document_id == DocumentId))
            {
                return false;
            }



            return true;
        }
        public static bool DocumentCanEdit(string UserId,int DocumentId)
        {

            //Create || Response:
            ApplicationDbContext db = new ApplicationDbContext();
            if (db.Documents.Find(DocumentId).CreatedById.Equals(UserId))
            {
                return true;
            }

            if (!string.IsNullOrEmpty(db.Documents.Find(DocumentId).ResponsibleUserId))
            {
                if (db.Documents.Find(DocumentId).ResponsibleUserId.Equals(UserId))
                {


                    return true;

                }
            }


            //DocumentUser:
            if(db.DocumentUsers.Any(a=>a.UserId.Equals(UserId)&& a.DocumentId==DocumentId&&a.EnableEdit==true))
            {
                return true;
            }

            //Document Departmnent:
            int Dep_Id = db.Users.Find(UserId).DepartmentId.HasValue ? db.Users.Find(UserId).DepartmentId.Value : -1;

            if(db.DocumentDepartments.Any(a => a.DepartmentId==Dep_Id && a.DocumentId == DocumentId && a.EnableEdit == true))
            {
                return true;
            }

            //document Groups 

            List<int> GroupsId = new List<int>();
            GroupsId = db.UsersGroups.Where(a => a.UserId.Equals(UserId)).Select(a => a.GroupId).ToList();
            if (db.DocumentGroups.Any(a => GroupsId.Contains(a.GroupId)  && a.DocumentId == DocumentId && a.EnableEdit == true))
            {

                return true;
            }

            return false;
        }


        public static bool DocumentCanSeal(string UserId, int DocumentId)
        {

            //Create || Response:
            ApplicationDbContext db = new ApplicationDbContext();
            if (db.Documents.Find(DocumentId).CreatedById.Equals(UserId))
            {
                return true;
            }

            if (!string.IsNullOrEmpty(db.Documents.Find(DocumentId).ResponsibleUserId))
            {
                if (db.Documents.Find(DocumentId).ResponsibleUserId.Equals(UserId))
                {


                    return true;

                }
            }


            //DocumentUser:
            if (db.DocumentUsers.Any(a => a.UserId.Equals(UserId) && a.DocumentId == DocumentId && a.EnableSeal == true))
            {
                return true;
            }

            //Document Departmnent:
            int Dep_Id = db.Users.Find(UserId).DepartmentId.HasValue ? db.Users.Find(UserId).DepartmentId.Value : -1;

            if (db.DocumentDepartments.Any(a => a.DepartmentId == Dep_Id && a.DocumentId == DocumentId && a.EnableSeal == true))
            {
                return true;
            }

            //document Groups 

            List<int> GroupsId = new List<int>();
            GroupsId = db.UsersGroups.Where(a => a.UserId.Equals(UserId)).Select(a => a.GroupId).ToList();
            if (db.DocumentGroups.Any(a => GroupsId.Contains(a.GroupId) && a.DocumentId == DocumentId && a.EnableSeal == true))
            {

                return true;
            }

            return false;
        }



        public static bool DocumentCanReplay(string UserId, int DocumentId)
        {

            //Create || Response:
            ApplicationDbContext db = new ApplicationDbContext();
            if (db.Documents.Find(DocumentId).CreatedById.Equals(UserId))
            {
                return true;
            }

            if (!string.IsNullOrEmpty(db.Documents.Find(DocumentId).ResponsibleUserId))
            {
                if (db.Documents.Find(DocumentId).ResponsibleUserId.Equals(UserId))
                {


                    return true;

                }
            }


            //DocumentUser:
            if (db.DocumentUsers.Any(a => a.UserId.Equals(UserId) && a.DocumentId == DocumentId && a.EnableReplay == true))
            {
                return true;
            }

            //Document Departmnent:
            int Dep_Id = db.Users.Find(UserId).DepartmentId.HasValue ? db.Users.Find(UserId).DepartmentId.Value : -1;

            if (db.DocumentDepartments.Any(a => a.DepartmentId == Dep_Id && a.DocumentId == DocumentId && a.EnableReplay == true))
            {
                return true;
            }

            //document Groups 

            List<int> GroupsId = new List<int>();
            GroupsId = db.UsersGroups.Where(a => a.UserId.Equals(UserId)).Select(a => a.GroupId).ToList();
            if (db.DocumentGroups.Any(a => GroupsId.Contains(a.GroupId) && a.DocumentId == DocumentId && a.EnableReplay == true))
            {

                return true;
            }

            return false;
        }


        public static bool DocumentCanRelate(string UserId, int DocumentId)
        {

            //Create || Response:
            ApplicationDbContext db = new ApplicationDbContext();
            if (db.Documents.Find(DocumentId).CreatedById.Equals(UserId))
            {
                return true;
            }

            if (!string.IsNullOrEmpty(db.Documents.Find(DocumentId).ResponsibleUserId))
            {
                if (db.Documents.Find(DocumentId).ResponsibleUserId.Equals(UserId))
                {


                    return true;

                }
            }


            //DocumentUser:
            if (db.DocumentUsers.Any(a => a.UserId.Equals(UserId) && a.DocumentId == DocumentId && a.EnableRelate == true))
            {
                return true;
            }

            //Document Departmnent:
            int Dep_Id = db.Users.Find(UserId).DepartmentId.HasValue ? db.Users.Find(UserId).DepartmentId.Value : -1;

            if (db.DocumentDepartments.Any(a => a.DepartmentId == Dep_Id && a.DocumentId == DocumentId && a.EnableRelate == true))
            {
                return true;
            }

            //document Groups 

            List<int> GroupsId = new List<int>();
            GroupsId = db.UsersGroups.Where(a => a.UserId.Equals(UserId)).Select(a => a.GroupId).ToList();
            if (db.DocumentGroups.Any(a => GroupsId.Contains(a.GroupId) && a.DocumentId == DocumentId && a.EnableRelate == true))
            {

                return true;
            }

            return false;
        }

    }
}