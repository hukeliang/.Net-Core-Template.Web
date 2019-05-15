using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;


namespace Template.Model.TemplateModels
{
    [Serializable]
    [Table("PermissionsTB")]
    public class Permissions : BaseModel
    {
        /// <summary>
        /// 权限值
        /// </summary>
        [DisplayName("权限值")]
        public string PermissionsValue { get; set; }
    }
}
