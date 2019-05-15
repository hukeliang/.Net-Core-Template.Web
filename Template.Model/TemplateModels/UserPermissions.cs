using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Template.Model.TemplateModels
{
    [Serializable]
    [Table("UserPermissionsTB")]
    public class UserPermissions : BaseModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DisplayName("用户ID")]
        public long UserID { get; set; }

        /// <summary>
        /// 权限ID
        /// </summary>
        [DisplayName("权限ID")]
        public long PermissionsID { get; set; }
    }
}
