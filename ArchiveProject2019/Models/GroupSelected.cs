using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class GroupSelected
    {

        public int Id { set; get; }
        public string Name { set; get; }
        public bool Selected { set; get; }

        public static List<GroupSelected> UserGroups(string id)

        {

            List<GroupSelected> GroupsSelected = new List<GroupSelected>();

            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<int> User_Group_Id = db.UsersGroups.Where(a => a.UserId.Equals(id)).Select(a => a.GroupId);

            IEnumerable<Group> Groups = db.Groups.ToList();
            foreach (Group G in Groups)
            {
                var Gs = new GroupSelected()
                {

                    Id = G.Id,
                    Name = G.Name,
                    Selected = User_Group_Id.Any(a => a == G.Id) ? true : false
                };

                GroupsSelected.Add(Gs);
            }

            return GroupsSelected;
        }
       
        }
    }
