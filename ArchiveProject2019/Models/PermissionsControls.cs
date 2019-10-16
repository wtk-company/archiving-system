using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class PermissionsControls
    {
        public string Name { set; get; }
        public string ActionName { set; get; }
        public bool Type { set; get; }
        public static List<PermissionsControls> PermissionsStartUp() {

            //List Of Permission
            return new List<PermissionsControls>() {

                new PermissionsControls() {Name="قائمة الجهات المعنية",ActionName="ConcernedPartiesIndex",Type=false },
                new PermissionsControls() {Name="إضافة جهة جديدة",ActionName="ConcernedPartiesCreate",Type=false  },
                new PermissionsControls() {Name="تعديل جهة معينة",ActionName="ConcernedPartiesEdit",Type=false  },
                new PermissionsControls() {Name=" حذف جهة معينة",ActionName="ConcernedPartiesDelete" ,Type=false },






                new PermissionsControls() {Name="قائمة الأقسام ",ActionName="DepartmentsIndex",Type=false  },
                new PermissionsControls() {Name="إضافة قسم معين",ActionName="DepartmentsCreate",Type=false  },
                new PermissionsControls() {Name="حذف قسم",ActionName="DepartmentsDelete" ,Type=false },
                new PermissionsControls() {Name=" تعديل قسم",ActionName="DepartmentsEdit",Type=false  },
                new PermissionsControls() {Name="قائمة مستخدمين مجموعة معينة-الأقسام ",ActionName="DepartmentsUsers",Type=false  },






                new PermissionsControls() {Name="قائمة الأقسام الخاصة للوثيقة",ActionName="DocumentDepartmentsIndex",Type=false  },
                new PermissionsControls() {Name="  إضافة الاقسام للوثيقة",ActionName="DocumentDepartmentsCreate",Type=false  },
                new PermissionsControls() {Name="  إزالة الأقسام من الوثيقة",ActionName="DocumentDepartmentsDelete" ,Type=false },






                new PermissionsControls() {Name="قائمة المجموعات الخاصة بالوثيقة ",ActionName="DocumentGroups",Type=false  },
                new PermissionsControls() {Name="إضافة مجموعات للوثيقة ",ActionName="DocumentGroupsCreate" ,Type=false },
                new PermissionsControls() {Name=" إزالة مجموعات من الوثيقة",ActionName="DocumentGroupsDelete" ,Type=false },






                new PermissionsControls() {Name="قائمة أنواع الوثائق",ActionName="DocumentKindsIndex",Type=false  },
                new PermissionsControls() {Name="إضافة نوع وثيقة جديد",ActionName="DocumentKindsCreate" ,Type=false },
                new PermissionsControls() {Name="تعديل نوع وثيقة",ActionName="DocumentKindsEdit",Type=false  },
                new PermissionsControls() {Name="حذف نوع وثيقة",ActionName="DocumentKindsDelete",Type=false  },


               



                new PermissionsControls() {Name="قائمة حقول النموذج",ActionName="FieldsIndex",Type=false  },
                new PermissionsControls() {Name="إضافة حقول جديدة للنموذج",ActionName="FieldsCreate",Type=false  },
                new PermissionsControls() {Name="تعديل حقل النموذج",ActionName="FieldsEdit",Type=false  },
                new PermissionsControls() {Name="حذف حقول النموذج",ActionName="FieldsDelete" ,Type=false },
                new PermissionsControls() {Name="تفاصيل حقول النموذج",ActionName="FieldsDetails",Type=false  },





                

                new PermissionsControls() {Name="قائمة الأقسام الخاصة بالنموذج",ActionName="FormDepartmentsIndex",Type=false  },
                new PermissionsControls() {Name="إضافة أقسام للنماذج",ActionName="FormDepartmentsCreate",Type=false  },
                new PermissionsControls() {Name="إزالة أقسام من النماذج",ActionName="FormDepartmentsDelete",Type=false  },
                new PermissionsControls() {Name="تعديل حالة تفعيل الاقسام للنماذج",ActionName="FormDepartmentsEdit" ,Type=false } ,
                new PermissionsControls() {Name="تفاصيل أقسام  الخاصةبالنماذج",ActionName="FormDepartmentsDetails",Type=false  },




                
                new PermissionsControls() {Name="قائمة المجموعات الخاصة بالنماذج",ActionName="FormGroupsIndex",Type=false  },
                new PermissionsControls() {Name="إضافة المجموعات للنماذج",ActionName="FormGroupsCreate",Type=false  },
                new PermissionsControls() {Name="تفاصيل المجموعات الخاصة بالنماذج",ActionName="FormGroupsDetails",Type=false  },
                new PermissionsControls() {Name="تعديل حالة تفعيل مجموعات النماذج",ActionName="FormGroupsEdit",Type=false  },
                new PermissionsControls() {Name="إزالة المجموعات من النماذج",ActionName="FormGroupsDelete" ,Type=false },





                new PermissionsControls() {Name="قائمة النماذج",ActionName="FormsIndex",Type=false  },
                new PermissionsControls() {Name="إضافة نموذج",ActionName="FormsCreate",Type=false  },
                new PermissionsControls() {Name="حذف نموذج",ActionName="FormsDelete",Type=false  },
                new PermissionsControls() {Name="تعديل نموذج",ActionName="FormsEdit",Type=false  },




                new PermissionsControls() {Name="قائمة المجموعات",ActionName="GroupsIndex" ,Type=false },
                new PermissionsControls() {Name="إضافة مجموعة ",ActionName="GroupsCreate",Type=false  },
                new PermissionsControls() {Name=" تعديل مجموعة",ActionName="GroupsEdit",Type=false  },
                new PermissionsControls() {Name=" حذف مجموعة",ActionName="GroupsDelete",Type=false  },
                new PermissionsControls() {Name="تفاصيل مجموعة",ActionName="GroupsDetails",Type=false  },






                
                new PermissionsControls() {Name="قائمة المسميات الوظيفية",ActionName="JobTitlesIndex",Type=false  },
                new PermissionsControls() {Name="إضافة مسمى وظيفي",ActionName="JobTitlesCreate",Type=false  },
                new PermissionsControls() {Name="تعديل مسمى وظيفي",ActionName="JobTitlesEdit",Type=false  },
                new PermissionsControls() {Name="تفاصيل مسمى وظيفي",ActionName="JobTitlesDetails",Type=false  },
                new PermissionsControls() {Name="حذف مسمى وظيفي",ActionName="JobTitlesDelete",Type=false  },



                
                new PermissionsControls() {Name="قائمة صلاحيات الادوار",ActionName="PermissionRolesIndex",Type=false  },
                new PermissionsControls() {Name="إضافة صلاحيات",ActionName="PermissionRolesCreate",Type=false  },
                new PermissionsControls() {Name="تعديل حالة تفعيل الصلاحيات للدور",ActionName="PermissionRolesActive",Type=false  },
                new PermissionsControls() {Name="تفاصيل صلاحيات الدور",ActionName="PermissionRolesDetails",Type=false  },
                new PermissionsControls() {Name="إزالة صلاحيات من الادوار",ActionName="PermissionRolesDelete",Type=false  },



                
                new PermissionsControls() {Name="صلاحيات المستخدمين مع التعديل",ActionName="PermissionsUsersIndex",Type=false  },



                new PermissionsControls() {Name="قائمة الأدوار",ActionName="RolesIndex",Type=false  },
                new PermissionsControls() {Name="إضافة الأدوار",ActionName="RolesCreate",Type=false  },
                new PermissionsControls() {Name="حذف الأدوار",ActionName="RolesDelete" ,Type=false },
                new PermissionsControls() {Name="تعديل الأدوار",ActionName="RolesEdit",Type=false  },



                

                new PermissionsControls() {Name="قائمة أنواع البريد",ActionName="TypeMailsIndex",Type=false  },
                new PermissionsControls() {Name="إضافة نوع بريد",ActionName="TypeMailsCreate",Type=false  },
                new PermissionsControls() {Name="تعديل نوع بريد",ActionName="TypeMailsEdit",Type=false  },
                new PermissionsControls() {Name="حذف نوع بريد",ActionName="TypeMailsDelete",Type=false  },





                new PermissionsControls() {Name="تسجيل مستخدم جديد",ActionName="UsersRegister",Type=false  },
                new PermissionsControls() {Name="تعديل كلمة سر الحساب",ActionName="UsersChangeProfile",Type=true  },
                new PermissionsControls() {Name="قائمة المستخدمين",ActionName="UsersIndex",Type=false  },
                new PermissionsControls() {Name="تفاصيل المستخدمين",ActionName="UsersDetails",Type=false  },
                new PermissionsControls() {Name="تعديل المستخدمين",ActionName="UsersEdit",Type=false  },
                new PermissionsControls() {Name="حذف المستخدمين",ActionName="UsersDelete",Type=false  },
                new PermissionsControls() {Name="تعديل حالة القفل للمستخدمين",ActionName="UsersLockOut",Type=false  },
                new PermissionsControls() {Name="إضافة مدير جديد",ActionName="UsersRegisterMasterUser",Type=false  },




                new PermissionsControls() {Name="إضافة  المستخدمين للمجموعة",ActionName="UsersGroupsIndex",Type=false  },
                new PermissionsControls() {Name="قائمة مستخدمي المجموعة",ActionName="UsersGroupsShowUsersGroup",Type=false  },
              
                new PermissionsControls() {Name="إزالة المستخدمين من المجموعات",ActionName="UsersGroupsDelete",Type=false  },












                //Others Permission:


            };
        }
    }
}