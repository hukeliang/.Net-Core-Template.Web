using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Template.Model.Other;

namespace Template.Model.TemplateModels
{
    [Serializable]
    [Table("UserTB")]
    public class User : BaseModel
    {
        /// <summary>
        /// 登陆用户名
        /// </summary>
        [DisplayName("用户名")]  //[Display(Name = "fasd")]
        [Required(ErrorMessage = "用户名不能为空")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "用户名长度为{1}至{0}个字符")]
        public string UserName { get; set; }

        /// <summary>
        /// 登陆密码
        /// </summary>
        [DisplayName("密码")]
        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "密码长度为{0}至{1}个字符")]
        public string PassWord { get; set; }

        /// <summary>
        /// 确认登陆密码
        /// </summary>
        [DisplayName("确认密码")]
        [Compare(nameof(PassWord), ErrorMessage = "两次密码输入不一致")]
        [NotMapped]
        public string ConfirmPassWord { get; set; }

        /// <summary>
        /// 登陆邮箱
        /// </summary>
        [DisplayName("邮箱")]
        [Required(ErrorMessage = "邮箱不能为空！")]
        [RegularExpression(@"/^[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?$/", ErrorMessage = "邮箱格式错误")]
        [MaxLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [DisplayName("年龄")]
        [Range(0, 150, ErrorMessage = "年龄的范围在{0}岁至{1}岁之间")]
        public int Age { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [DisplayName("性别")]
        public SexEnum Sex { get; set; }
        
    }
}
