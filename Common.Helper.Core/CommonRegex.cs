using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Helper.Core
{
    public struct CommonRegex
    {
        /// <summary>
        /// 电子邮件
        /// </summary>
        public const string Email = @"^[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?$";

        /// <summary>
        /// 数字
        /// </summary>
        public const string Number = @"^[0-9]*$";
        /// <summary>
        /// 至少6位的数字
        /// </summary>
        public const string AtLeastNumber = @"^\d{5,}$";


       

    }
}
