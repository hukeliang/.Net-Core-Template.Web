namespace Common.Helper.Core.EmailExpand.Implementation
{
    public class SendEmailOptions
    {

        /// <summary>
        /// 默认标题
        /// </summary>
        public string MailTitel { get; set; }

        /// <summary>
        /// 邮箱服务器地址 优先使用域名
        /// </summary>
        public string MailSmtp { get; set; }

        /// <summary>
        /// 如果没有邮箱服务器域名 使用IP + 端口
        /// </summary>
        public string MailSmtpIP { get; set; }

        /// <summary>
        /// 如果没有邮箱服务器域名 使用IP + 端口
        /// </summary>
        public int MailSmtpPort { get; set; }

        /// <summary>
        /// 发送邮件账号
        /// </summary>
        public string MailAccount { get; set; }

        /// <summary>
        /// 发送邮件密码
        /// </summary>
        public string MailPassWord { get; set; }

    }
}
