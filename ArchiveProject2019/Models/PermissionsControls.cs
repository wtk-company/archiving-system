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
        public bool TypeMaster { set; get; }
        public static List<PermissionsControls> PermissionsStartUp() {

            //List Of Permission
            return new List<PermissionsControls>() {

                new PermissionsControls() {Name="قائمة الجهات المعنية",ActionName="ConcernedPartiesIndex",Type=false,TypeMaster=true },
                new PermissionsControls() {Name="إضافة جهة جديدة",ActionName="ConcernedPartiesCreate",Type=false ,TypeMaster=true },
                new PermissionsControls() {Name="تعديل جهة معينة",ActionName="ConcernedPartiesEdit",Type=false ,TypeMaster=true },
                new PermissionsControls() {Name=" حذف جهة معينة",ActionName="ConcernedPartiesDelete" ,Type=false,TypeMaster=true },
                new PermissionsControls() {Name="تفاصيل جهة معينة",ActionName="ConcernedPartiesDetails" ,Type=false,TypeMaster=true },

                




                //Departments:


                new PermissionsControls() {Name="قائمة الأقسام ",ActionName="DepartmentsIndex",Type=false,TypeMaster=true  },
                new PermissionsControls() {Name="إضافة قسم معين",ActionName="DepartmentsCreate",Type=false ,TypeMaster=true },
                new PermissionsControls() {Name="حذف قسم",ActionName="DepartmentsDelete" ,Type=false,TypeMaster=true },
                new PermissionsControls() {Name=" تعديل قسم",ActionName="DepartmentsEdit",Type=false ,TypeMaster=true },
                new PermissionsControls() {Name="قائمة مستخدمين  قسم معين ",ActionName="DepartmentsUsers",Type=false,TypeMaster=true  },






                new PermissionsControls() {Name="قائمة الأقسام الخاصة للوثيقة",ActionName="DocumentDepartmentsIndex",Type=true ,TypeMaster=false },
                new PermissionsControls() {Name="إضافةالوثيقة للاقسام",ActionName="DocumentDepartmentsCreate",Type=true ,TypeMaster=false },
                new PermissionsControls() {Name="  إزالة الوثيقة من الاقسام",ActionName="DocumentDepartmentsDelete" ,Type=true,TypeMaster=false },
                new PermissionsControls() {Name=" تفاصيل اسناد الوثيقة للقسم",ActionName="DocumentDepartmentsDetails" ,Type=true,TypeMaster=false },

                new PermissionsControls() {Name="تعديل إمكانية التعديل للوثيقة ضمن الاقسام",ActionName="DocumentDepartmentsActiveNonActiveEdit" ,Type=true,TypeMaster=false },
                new PermissionsControls() {Name="تعديل إمكانية الرد للوثيقة ضمن الاقسام",ActionName="DocumentDepartmentsIs_ActiveNonIs_ActiveReplay" ,Type=true,TypeMaster=false },
                new PermissionsControls() {Name="تعديل إمكانية التسديد للوثيقة ضمن الاقسام",ActionName="DocumentDepartmentsIs_ActiveNonIs_ActiveSeal" ,Type=true,TypeMaster=false },
                new PermissionsControls() {Name=" تعديل إمكانية الربط للوثيقة ضمن الاقسام",ActionName="DocumentDepartmentsIs_ActiveNonIs_ActiveRelate" ,Type=true,TypeMaster=false },








                new PermissionsControls() {Name="قائمة المستخدمين المسندين للوثيقة",ActionName="DocumentUserIndex",Type=true ,TypeMaster=false },
                new PermissionsControls() {Name="إضافةالوثيقة للمستخدمين",ActionName="DocumentUserCreate",Type=true ,TypeMaster=false },
                new PermissionsControls() {Name="  إزالة الوثيقة من المستخدم",ActionName="DocumentUserDelete" ,Type=true,TypeMaster=false },
                new PermissionsControls() {Name=" تفاصيل اسناد الوثيقة للمستخدم",ActionName="DocumentUserDelails" ,Type=true,TypeMaster=false },

                new PermissionsControls() {Name="تعديل إمكانية التعديل للوثيقة ضمن المستخدمين",ActionName="DocumentUserIs_ActiveNonIs_ActiveEdit" ,Type=true,TypeMaster=false },
                new PermissionsControls() {Name="تعديل إمكانية الرد للوثيقة ضمن المستخدمين",ActionName="DocumentUserIs_ActiveNonIs_ActiveReplay" ,Type=true,TypeMaster=false },
                new PermissionsControls() {Name="تعديل إمكانية التسديد للوثيقة ضمن المستخدمين",ActionName="DocumentUserIs_ActiveNonIs_ActiveSeal" ,Type=true,TypeMaster=false },
                new PermissionsControls() {Name=" تعديل إمكانية الربط للوثيقة ضمن المستخدمين",ActionName="DocumentUserIs_ActiveNonIs_ActiveRelate" ,Type=true,TypeMaster=false },







                new PermissionsControls() {Name="قائمة المجموعات الخاصة بالوثيقة ",ActionName="DocumentGroups",Type=true  ,TypeMaster=false  },
                new PermissionsControls() {Name="إضافة  وثيقة للمجموعات ",ActionName="DocumentGroupsCreate" ,Type=true  ,TypeMaster=false },
                new PermissionsControls() {Name=" إزالة   وثيقة من المجموعات",ActionName="DocumentGroupsDelete" ,Type=true  ,TypeMaster=false },
                new PermissionsControls() {Name="تفاصيل اسناد الوثيقة للمجموعات",ActionName="DocumentGroupsDetails" ,Type=true  ,TypeMaster=false },

                new PermissionsControls() {Name="تعديل إمكانية التعديل للوثيقة ضمن المجموعات",ActionName="DocumentGroupsIs_ActiveNonIs_ActiveEdit" ,Type=true,TypeMaster=false },
                new PermissionsControls() {Name="تعديل إمكانية الرد للوثيقة ضمن المجموعات",ActionName="DocumentGroupsIs_ActiveNonIs_ActiveReplay" ,Type=true,TypeMaster=false },
                new PermissionsControls() {Name="تعديل إمكانية التسديد للوثيقة ضمن المجموعات",ActionName="DocumentGroupsIs_ActiveNonIs_ActiveSeal" ,Type=true,TypeMaster=false },
                new PermissionsControls() {Name=" تعديل إمكانية الربط للوثيقة ضمن المجموعات",ActionName="DocumentGroupsIs_ActiveNonIs_ActiveRelate" ,Type=true,TypeMaster=false },













                new PermissionsControls() {Name="قائمة أنواع الوثائق",ActionName="DocumentKindsIndex",Type=false ,TypeMaster=true },
                new PermissionsControls() {Name="إضافة نوع وثيقة جديد",ActionName="DocumentKindsCreate" ,Type=false,TypeMaster=true },
                new PermissionsControls() {Name="تعديل نوع وثيقة",ActionName="DocumentKindsEdit",Type=false ,TypeMaster=true },
                new PermissionsControls() {Name="حذف نوع وثيقة",ActionName="DocumentKindsDelete",Type=false ,TypeMaster=true },
                new PermissionsControls() {Name="تفاصيل نوع وثيقة",ActionName="DocumentKindsDetails",Type=false,TypeMaster=true  },







                new PermissionsControls() {Name="قائمة حقول النموذج",ActionName="FieldsIndex",Type=false ,TypeMaster=true },
                new PermissionsControls() {Name="إضافة حقول جديدة للنموذج",ActionName="FieldsCreate",Type=false ,TypeMaster=true },
                new PermissionsControls() {Name="تعديل حقل النموذج",ActionName="FieldsEdit",Type=false,TypeMaster=true  },
                new PermissionsControls() {Name="حذف حقول النموذج",ActionName="FieldsDelete" ,Type=false,TypeMaster=true },
                new PermissionsControls() {Name="تفاصيل حقول النموذج",ActionName="FieldsDetails",Type=false ,TypeMaster=true },





                

                new PermissionsControls() {Name="قائمة الأقسام الخاصة بالنموذج",ActionName="FormDepartmentsIndex",Type=false ,TypeMaster=true },
                new PermissionsControls() {Name="إضافة أقسام للنماذج",ActionName="FormDepartmentsCreate",Type=false ,TypeMaster=true },
                new PermissionsControls() {Name="إزالة أقسام من النماذج",ActionName="FormDepartmentsDelete",Type=false,TypeMaster=true  },
                new PermissionsControls() {Name="تعديل حالة تفعيل الاقسام للنماذج",ActionName="FormDepartmentsEdit" ,Type=false,TypeMaster=true } ,
                new PermissionsControls() {Name="تفاصيل أقسام  الخاصةبالنماذج",ActionName="FormDepartmentsDetails",Type=false ,TypeMaster=true },




                
                new PermissionsControls() {Name="قائمة المجموعات الخاصة بالنماذج",ActionName="FormGroupsIndex",Type=false,TypeMaster=true  },
                new PermissionsControls() {Name="إضافة المجموعات للنماذج",ActionName="FormGroupsCreate",Type=false ,TypeMaster=true  },
                new PermissionsControls() {Name="تفاصيل المجموعات الخاصة بالنماذج",ActionName="FormGroupsDetails",Type=false,TypeMaster=true   },
                new PermissionsControls() {Name="تعديل حالة تفعيل مجموعات النماذج",ActionName="FormGroupsEdit",Type=false,TypeMaster=true   },
                new PermissionsControls() {Name="إزالة المجموعات من النماذج",ActionName="FormGroupsDelete" ,Type=false,TypeMaster=true  },





                new PermissionsControls() {Name="قائمة النماذج",ActionName="FormsIndex",Type=false ,TypeMaster=true },
                new PermissionsControls() {Name="إضافة نموذج",ActionName="FormsCreate",Type=false ,TypeMaster=true },
                new PermissionsControls() {Name="حذف نموذج",ActionName="FormsDelete",Type=false ,TypeMaster=true },
                new PermissionsControls() {Name="تعديل نموذج",ActionName="FormsEdit",Type=false ,TypeMaster=true },
                new PermissionsControls() {Name=" تفاصيل نموذج",ActionName="FormsDetails",Type=false,TypeMaster=true  },








                new PermissionsControls() {Name="قائمة المجموعات",ActionName="GroupsIndex" ,Type=false,TypeMaster=true },
                new PermissionsControls() {Name="إضافة مجموعة ",ActionName="GroupsCreate",Type=false ,TypeMaster=true  },
                new PermissionsControls() {Name=" تعديل مجموعة",ActionName="GroupsEdit",Type=false ,TypeMaster=true  },
                new PermissionsControls() {Name=" حذف مجموعة",ActionName="GroupsDelete",Type=false,TypeMaster=true   },
                new PermissionsControls() {Name="تفاصيل مجموعة",ActionName="GroupsDetails",Type=false,TypeMaster=true   },






                
                new PermissionsControls() {Name="قائمة المسميات الوظيفية",ActionName="JobTitlesIndex",Type=false,TypeMaster=true   },
                new PermissionsControls() {Name="إضافة مسمى وظيفي",ActionName="JobTitlesCreate",Type=false ,TypeMaster=true  },
                new PermissionsControls() {Name="تعديل مسمى وظيفي",ActionName="JobTitlesEdit",Type=false ,TypeMaster=true  },
                new PermissionsControls() {Name="تفاصيل مسمى وظيفي",ActionName="JobTitlesDetails",Type=false ,TypeMaster=true  },
                new PermissionsControls() {Name="حذف مسمى وظيفي",ActionName="JobTitlesDelete",Type=false ,TypeMaster=true  },



                
                new PermissionsControls() {Name="قائمة صلاحيات الادوار",ActionName="PermissionRolesIndex",Type=false,TypeMaster=true   },
                new PermissionsControls() {Name="إضافة صلاحيات",ActionName="PermissionRolesCreate",Type=false ,TypeMaster=true  },
                new PermissionsControls() {Name="تعديل حالة تفعيل الصلاحيات للدور",ActionName="PermissionRolesIs_Active",Type=false,TypeMaster=true   },
                new PermissionsControls() {Name="تفاصيل صلاحيات الدور",ActionName="PermissionRolesDetails",Type=false,TypeMaster=true   },
                new PermissionsControls() {Name="إزالة صلاحيات من الادوار",ActionName="PermissionRolesDelete",Type=false ,TypeMaster=true  },



                
                new PermissionsControls() {Name="صلاحيات المستخدمين مع التعديل",ActionName="PermissionsUsersIndex",Type=false  ,TypeMaster=true },



                new PermissionsControls() {Name="قائمة الأدوار",ActionName="RolesIndex",Type=false ,TypeMaster=true  },
                new PermissionsControls() {Name="إضافة الأدوار",ActionName="RolesCreate",Type=false ,TypeMaster=true  },
                new PermissionsControls() {Name="حذف الأدوار",ActionName="RolesDelete" ,Type=false ,TypeMaster=true },
                new PermissionsControls() {Name="تعديل الأدوار",ActionName="RolesEdit",Type=false,TypeMaster=true   },
                new PermissionsControls() {Name="تفاصيل الأدوار",ActionName="RolesDetails",Type=false,TypeMaster=true   },








                new PermissionsControls() {Name="قائمة أنواع البريد",ActionName="TypeMailsIndex",Type=false ,TypeMaster=true  },
                new PermissionsControls() {Name="إضافة نوع بريد",ActionName="TypeMailsCreate",Type=false ,TypeMaster=true  },
                new PermissionsControls() {Name="تعديل نوع بريد",ActionName="TypeMailsEdit",Type=false  ,TypeMaster=true },
                new PermissionsControls() {Name="حذف نوع بريد",ActionName="TypeMailsDelete",Type=false ,TypeMaster=true  },
                new PermissionsControls() {Name="تفاصيل نوع بريد",ActionName="TypeMailsDetails",Type=false  ,TypeMaster=true },




                new PermissionsControls() {Name="قائمة حالات الوثائق",ActionName="DocumentStatusIndex",Type=false  ,TypeMaster=true },
                new PermissionsControls() {Name="إضافة حالة وثيقة",ActionName="DocumentStatusCreate",Type=false  ,TypeMaster=true },
                new PermissionsControls() {Name="تعديل حالة وثيقة",ActionName="DocumentStatusEdit",Type=false  ,TypeMaster=true },
                new PermissionsControls() {Name="حذف حالة وثيقة",ActionName="DocumentStatusDelete",Type=false  ,TypeMaster=true },
                new PermissionsControls() {Name="تفاصيل حالة وثيقة",ActionName="DocumentStatusDetails",Type=false  ,TypeMaster=true },










                new PermissionsControls() {Name="تسجيل مستخدم جديد",ActionName="UsersRegister",Type=false,TypeMaster=true  },
                new PermissionsControls() {Name="تعديل كلمة سر الحساب",ActionName="UsersChangeProfile",Type=true ,TypeMaster=true },
                new PermissionsControls() {Name="قائمة المستخدمين",ActionName="UsersIndex",Type=false,TypeMaster=true  },
                new PermissionsControls() {Name="تفاصيل المستخدمين",ActionName="UsersDetails",Type=false ,TypeMaster=true },
                new PermissionsControls() {Name="تعديل المستخدمين",ActionName="UsersEdit",Type=false ,TypeMaster=true },
                new PermissionsControls() {Name="حذف المستخدمين",ActionName="UsersDelete",Type=false,TypeMaster=true  },
                new PermissionsControls() {Name="تعديل حالة القفل للمستخدمين",ActionName="UsersLockOut",Type=false,TypeMaster=true  },
                new PermissionsControls() {Name="إضافة مدير جديد",ActionName="UsersRegisterMasterUser",Type=false,TypeMaster=true  },






                new PermissionsControls() {Name="إضافة  المستخدمين للمجموعة",ActionName="UsersGroupsIndex",Type=false ,TypeMaster=true },
                new PermissionsControls() {Name="قائمة مستخدمي المجموعة",ActionName="UsersGroupsShowUsersGroup",Type=false,TypeMaster=true   },
              
                new PermissionsControls() {Name="إزالة المستخدمين من المجموعات",ActionName="UsersGroupsDelete",Type=false  ,TypeMaster=true },




                new PermissionsControls() {Name="قائمة النسخ الإحتياطي والإسترجاع",ActionName="BackupRestoreIndex",Type=false  ,TypeMaster=true },
                new PermissionsControls() {Name="النسخ الاحتياطي لقاعدة البيانات",ActionName="BackupRestoreDownloadDB",Type=false  ,TypeMaster=true },
                new PermissionsControls() {Name="الاسترجاع لقاعدة المعطيات",ActionName="BackupRestoreRestoreDB",Type=false  ,TypeMaster=true },
                new PermissionsControls() {Name="النسخ الاحتياطي للملفات",ActionName="BackupRestoreDownloadFiles",Type=false  ,TypeMaster=true },
                new PermissionsControls() {Name="استرجاع الملفات ",ActionName="BackupRestoreRestoreFiles",Type=false  ,TypeMaster=true },





                new PermissionsControls() {Name="مجموعات المستخدم",ActionName="InformationUserGroups",Type=false  ,TypeMaster=true },
                new PermissionsControls() {Name=" نماذج المستخدم",ActionName="InformationUserForms",Type=false  ,TypeMaster=true },
                new PermissionsControls() {Name="اضافة نماذج مفضلة",ActionName="InformationUserAddFavoriteForms",Type=false  ,TypeMaster=true },
                new PermissionsControls() {Name="حذف نماذج مفضلة",ActionName="InformationUserDeleteFavoriteForms",Type=false  ,TypeMaster=true },


                
                new PermissionsControls() {Name="تهيئة معلومات المؤسسة",ActionName="CompanyCreate",Type=false  ,TypeMaster=true },








                
              new PermissionsControls() {Name="قائمة الإشعارات",ActionName="NonSeenNotificationList",Type=true  ,TypeMaster=true },
                new PermissionsControls() {Name="إزالة الإشعارات المحددة",ActionName="NonSeenNotificationListPost",Type=true  ,TypeMaster=true },

                new PermissionsControls() {Name="إزالة كل الإشعارات ",ActionName="NonSeenNotificationListAllPost",Type=true  ,TypeMaster=true },




                new PermissionsControls() {Name="قائمة الجهات الخاصة بالوثيقة",ActionName="DocumentPartyIndex",Type=true  ,TypeMaster=false },
                new PermissionsControls() {Name="إضافة جهات خارجية للوثيقة",ActionName="DocumentPartyCrete",Type=true  ,TypeMaster=false },
                new PermissionsControls() {Name="إزالة جهة من الوثيقة",ActionName="DocumentPartyDelete",Type=true  ,TypeMaster=false },




                new PermissionsControls() {Name="قائمة التسديدات للوثيقة",ActionName="DocumentSealsIndex",Type=true  ,TypeMaster=false },
                new PermissionsControls() {Name="تفاصيل تسديد للوثيقة",ActionName="DocumentSealsDetails",Type=true  ,TypeMaster=false },
                new PermissionsControls() {Name="إضافة تسديد للوثيقة",ActionName="DocumentSealsCreate",Type=true  ,TypeMaster=false },
                new PermissionsControls() {Name="تعديل تسديد للوثيقة",ActionName="DocumentSealsEdit",Type=true  ,TypeMaster=false },
                new PermissionsControls() {Name="إزالة تسديد من الوثيقة",ActionName="DocumentSealsDelete",Type=true  ,TypeMaster=false },





                //Documents:


              new PermissionsControls() {Name="انشاء اي نوع وثيقة",ActionName="DocumentCreate",Type=true  ,TypeMaster=false },
              new PermissionsControls() {Name="انشاء وثيقة مرتبطة",ActionName="RelateDocumentCreate",Type=true  ,TypeMaster=false },
              new PermissionsControls() {Name="انشاء وثيقة رد",ActionName="ReplayDocumentCreate",Type=true  ,TypeMaster=false },
              new PermissionsControls() {Name="تعديل وثيقة",ActionName="DocumentEdit",Type=true  ,TypeMaster=false },
              new PermissionsControls() {Name="قائمة الوثائق",ActionName="DocumentIndex",Type=true  ,TypeMaster=false },
              new PermissionsControls() {Name="ربط الوثيق مع وثائق متوفرة",ActionName="RelateDocumentCreateList",Type=true  ,TypeMaster=false },
              new PermissionsControls() {Name="حذف وثيقة",ActionName="DocumentDelete",Type=true  ,TypeMaster=false },
                new PermissionsControls() {Name="تفاصيل وثيقة",ActionName="DocumentDetails",Type=true  ,TypeMaster=false },

                new PermissionsControls() {Name="تحرير ارتباط وثيقة للأدنى",ActionName="DocumentRemoveChildRate",Type=true  ,TypeMaster=false },
                new PermissionsControls() {Name="تحرير ارتباط وثيقة للأعلى",ActionName="DocumentRemoveParentRate",Type=true  ,TypeMaster=false }



                
            };
        }
    }
}