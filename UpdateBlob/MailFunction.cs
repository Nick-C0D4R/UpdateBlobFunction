using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using FileUploadManager.Data;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace UpdateBlob
{
    [StorageAccount("ConnectionString")]
    public class MailFunction
    {
        [FunctionName("MailFunction")]
        public void Run([BlobTrigger("file-upload/{name}")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            if(name.StartsWith('['))
            {
                int firstPos = 0, lastPos = name.IndexOf(']');
                string email = name.Substring(firstPos + 1, lastPos - 1);

                EmailService emailService = new EmailService(new System.Net.Mail.SmtpClient("smtp.gmail.com", 587));

                using (MailMessage letter = new MailMessage())
                {
                    letter.From = new MailAddress("File.upload.6.11@gmail.com");
                    letter.To.Add(email);
                    letter.Subject = "File upload successfuly";
                    letter.Body = $@"Your .docx file was added to container successfully. To check it, you can follow the link
                          https://palanyaca.blob.core.windows.net/file-upload/{name}";
                    try
                    {
                        emailService.Send(letter);
                    }
                    catch (SmtpException ex)
                    {
                        log.LogError($"[ERR] - {ex.Message}");
                    }
                }
            }

        }
    }
}
