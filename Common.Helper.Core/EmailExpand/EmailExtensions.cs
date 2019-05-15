using Common.Helper.Core.EmailExpand.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Common.Helper.Core.EmailExpand
{
    public static class EmailExtensions
    {
        public static IServiceCollection AddSendEmail(this IServiceCollection services, Action<SendEmailOptions> options)
        {
            SendEmailOptions sendEmailOptions = new SendEmailOptions();

            options.Invoke(sendEmailOptions);

            if (string.IsNullOrWhiteSpace(sendEmailOptions.MailSmtp) && string.IsNullOrWhiteSpace(sendEmailOptions.MailSmtpIP))
            {
                throw new ArgumentNullException($"{nameof(sendEmailOptions.MailSmtp)} or {nameof(sendEmailOptions.MailSmtpIP)}");
            }

            if (string.IsNullOrWhiteSpace(sendEmailOptions.MailAccount))
            {
                throw new ArgumentNullException(nameof(sendEmailOptions.MailAccount));
            }

            if (string.IsNullOrWhiteSpace(sendEmailOptions.MailPassWord))
            {
                throw new ArgumentNullException(nameof(sendEmailOptions.MailPassWord));
            }

            services.AddTransient<ISendEmail>(srviceProvider => new SendEmail(sendEmailOptions));

            return services;
        }
    }
}
