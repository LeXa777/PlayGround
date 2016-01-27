namespace TheWorld.Services.MailService
{
    using System;

    public interface IMailService
    {
        bool SendMail(string to, string from, string subject, string body);
    }
}
