namespace TheWorld.Services.MailService
{
    using System.Diagnostics;

    public class DebugMailService : IMailService
    {
        public bool SendMail(string to, string from, string subject, string body)
        {
            Debug.WriteLine($"Sending Mail: To: {to}, from: {from}, subject: {subject}, body: {body}");
            return true;
        }
    }
}
