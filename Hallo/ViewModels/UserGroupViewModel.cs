using HalloDal.Models.Content;
using HalloDal.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace Hallo.ViewModels {
    public class UserGroupViewModel {

        public UserGroupViewModel(UserGroup usrGroup) {
            UserGroupId = usrGroup.UserGroupId;
            GroupName = usrGroup.GroupName;
            Sql = usrGroup.Sql;
        }

        public int UserGroupId { get; set; }

        [Display(Name = "GroupName", ResourceType = typeof(HalloDal.Resources.DisplayNames))]
        [StringLength(50)]
        public string GroupName { get; set; }

        [StringLength(2000)]
        public string Sql { get; set; }

        public Image GroupImage { get; set; }        

    }
}