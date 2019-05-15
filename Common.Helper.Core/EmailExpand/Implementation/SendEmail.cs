using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper.Core.EmailExpand.Implementation
{
    public sealed class SendEmail : ISendEmail
    {

        /// <summary>
        /// 配置邮箱的服务器 账号 密码
        /// </summary>
        private readonly SendEmailOptions _SendEmailOptions;

        public SendEmail(SendEmailOptions options)
        {
            _SendEmailOptions = options;
        }

        /// <summary>
        /// 异步发送邮件
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="address">邮箱地址</param>
        /// <returns></returns>
        async Task ISendEmail.SendAsync(string subject, string body, params string[] address)
        {
            if (address.Length == 0)
            {
                throw new ArgumentNullException(nameof(address));
            }

            var result = this.CreateEmail(subject, body, address.ToList(), null);

            using (SmtpClient smtpClient = result.smtpClient)
            {
                using (MailMessage mailMessage = result.mailMessage)
                {
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="address">邮箱地址</param>
        /// <returns></returns>
        void ISendEmail.Send(string subject, string body, params string[] address)
        {
            if (address == null || address.Length == 0)
            {
                throw new ArgumentNullException(nameof(address));
            }

            var result = this.CreateEmail(subject, body, address.ToList(), new List<string>());
            
            using (SmtpClient smtpClient = result.smtpClient)
            {
                using (MailMessage mailMessage = result.mailMessage)
                {
                    smtpClient.Send(mailMessage);
                }
            }
           
        }

        /// <summary>
        /// 异步发送带附件的邮件
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="addressList">邮箱地址</param>
        /// <param name="attachmentList">附件地址</param>
        async Task ISendEmail.SendAsync(string subject, string body, List<string> addressList, List<string> attachmentList)
        {
            if (addressList == null || addressList.Count == 0)
            {
                throw new ArgumentNullException(nameof(addressList));
            }

            if (attachmentList == null || attachmentList.Count == 0)
            {
                throw new ArgumentNullException(nameof(attachmentList));
            }

            var result = this.CreateEmail(subject, body, addressList, attachmentList);

            using (SmtpClient smtpClient = result.smtpClient)
            {
                using (MailMessage mailMessage = result.mailMessage)
                {
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }

           
        }

        /// <summary>
        /// 发送带附件的邮件
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="addressList">邮箱地址</param>
        /// <param name="attachmentList">附件地址</param>
        void ISendEmail.Send(string subject, string body, List<string> addressList, List<string> attachmentList)
        {
            if (addressList == null || addressList.Count == 0)
            {
                throw new ArgumentNullException(nameof(addressList));
            }

            if (attachmentList == null || attachmentList.Count == 0)
            {
                throw new ArgumentNullException(nameof(attachmentList));
            }

            var result = this.CreateEmail(subject, body, addressList, attachmentList);

            using (SmtpClient smtpClient = result.smtpClient)
            {
                using (MailMessage mailMessage = result.mailMessage)
                {
                    smtpClient.Send(mailMessage);
                }
            }
        }

        /// <summary>
        /// 创建一个邮件上下文
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="address">邮箱地址</param>
        /// <returns>返回一个值元组 (SmtpClient,MailMessage)</returns>
        private (SmtpClient smtpClient, MailMessage mailMessage) CreateEmail(string subject, string body, List<string> addressList, List<string> attachmentList)
        {
            MailMessage mailMessage = new MailMessage();
            //邮件主题
            mailMessage.Subject = subject;
            //主体内容
            mailMessage.Body = body;

            mailMessage.IsBodyHtml = true;

            mailMessage.BodyEncoding = Encoding.UTF8;

            mailMessage.Priority = MailPriority.Normal;

            //本文以outlook名义发送邮件，不会被当作垃圾邮件            
            mailMessage.Headers.Add("X-Priority", "3");

            mailMessage.Headers.Add("X-MSMail-Priority", "Normal");

            mailMessage.Headers.Add("X-Mailer", "Microsoft Outlook Express 6.00.2900.2869");

            mailMessage.Headers.Add("X-MimeOLE", "Produced By Microsoft MimeOLE V6.00.2900.2869");

            mailMessage.Headers.Add("ReturnReceipt", "1");
            
            //添加收件人
            foreach (string item in addressList)
            {
                mailMessage.To.Add(item.Trim());
            }

            //添加附件
            foreach (string item in attachmentList)
            {
                //实例化附件    
                Attachment attachment = new Attachment(item, MediaTypeNames.Application.Octet);

                ContentDisposition contentDisposition = attachment.ContentDisposition;

                //获取附件的创建日期  
                contentDisposition.CreationDate = File.GetCreationTime(item);

                //获取附件的修改日期  
                contentDisposition.ModificationDate = File.GetLastWriteTime(item);

                //获取附件的读取日期   
                contentDisposition.ReadDate = File.GetLastAccessTime(item);

                mailMessage.Attachments.Add(attachment);//添加到附件中 

                attachment.Dispose();
            }

            mailMessage.From = new MailAddress(_SendEmailOptions.MailAccount, _SendEmailOptions.MailTitel);

            SmtpClient smtpClient = string.IsNullOrWhiteSpace(_SendEmailOptions.MailSmtp)
                ? new SmtpClient(_SendEmailOptions.MailSmtp)
                : new SmtpClient(_SendEmailOptions.MailSmtpIP, _SendEmailOptions.MailSmtpPort);

            //启用SSl
            smtpClient.EnableSsl = true;

            //服务器证书验证回调　
            ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
            {
                return true;
            };

            //随请求一起发送
            smtpClient.UseDefaultCredentials = false;

            //邮件发送方式-网络发送
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            //设置发件人身份的票据   
            smtpClient.Credentials = new NetworkCredential(mailMessage.From.Address, _SendEmailOptions.MailPassWord);

            return (smtpClient, mailMessage);
        }


    }
}
