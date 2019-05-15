using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Template.Model
{
    public partial class BaseModel
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [DisplayName("ID")]
        [Key]
        public long ID { get; set; }


        
    }
    
    public partial class BaseModel
    {

        /// <summary>
        /// 添加时间
        /// </summary>
        [DisplayName("添加时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AddTime { get; private set; } = DateTime.Now;


        /// <summary>
        /// 数据的状态 软删除
        /// </summary>
        public int StatusInfo { get; private set; } = -1;
    }

}
