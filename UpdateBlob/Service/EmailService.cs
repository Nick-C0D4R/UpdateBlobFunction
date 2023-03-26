using System.Net.Http;
using System.Net.Mail;

namespace FileUploadManager.Data
{
    public class EmailService
    {
        private SmtpClient _client;

        public EmailService(SmtpClient client)
        {
            _client = client;
            _client.UseDefaultCredentials = false;
            _client.Credentials = new System.Net.NetworkCredential("File.upload.6.11@gmail.com", "pwhmrldpaqydqflu");
            _client.EnableSsl = true;
        }

        public void Send(MailMessage letter)
        {
            _client.Send(letter);
        }
    }
}
