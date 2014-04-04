using HalloDal.Models.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hallo.ViewModels {
    public class UserGroupViewModel {

        public UserGroupViewModel() {}

        public UserGroupViewModel(UserGroup usrGroup) {
            UserGroupId = usrGroup.UserGroupId;
            GroupName = usrGroup.GroupName;
            Sql = usrGroup.Sql;
            GroupType = usrGroup.GroupType;            
            GroupImage = new ImageViewModel(usrGroup.UserGroupId, usrGroup.GroupImage);
            Users = usrGroup.Users;
            Administrators = usrGroup.Administrators;
        }

        public int UserGroupId { get; set; }

        [Display(Name = "GroupName", ResourceType = typeof(HalloDal.Resources.DisplayNames))]
        [StringLength(50)]
        public string GroupName { get; set; }

        [StringLength(2000)]
        public string Sql { get; set; }

        public GroupType GroupType {get;set;}

        public ImageViewModel GroupImage { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<User> Administrators { get; set; }

        public AcPlanViewModel NextActivity { get; set; }
    }
}