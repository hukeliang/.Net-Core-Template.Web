using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper.Core.EmailExpand
{
    public interface ISendEmail
    {
        /// <summary>
        /// 异步发送邮件
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="address">邮箱地址</param>
        /// <returns></returns>
        Task SendAsync(string subject, string body, params string[] address);

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="address">邮箱地址</param>
        /// <returns></returns>
        void Send(string subject, string body, params string[] address);

        /// <summary>
        /// 异步发送带附件的邮件
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="addressList">邮箱地址</param>
        /// <param name="attachmentList">附件地址</param>
        Task SendAsync(string subject, string body, List<string> addressList, List<string> attachmentList);

        /// <summary>
        /// 发送带附件的邮件
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="addressList">邮箱地址</param>
        /// <param name="attachmentList">附件地址</param>
        void Send(string subject, string body, List<string> addressList, List<string> attachmentList);
    }
}
