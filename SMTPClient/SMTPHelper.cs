using MailKit.Security;
using MailKit;
using System.Net;
using System.Net.Mail;
using System.Collections.Specialized;
using static SMTPClient.SMTPClient;
using MailKit.Search;

namespace SMTPClient
{
    public class SMTPHelper
    {
        public static MailMessage BackupMsg = new MailMessage();
        public static string ClientReceiveUsername = Settings.Default.ClientReceiveUsername;
        public static string ClientReceivePassword = Settings.Default.ClientReceivePassword;
        public static string ClientSendUsername = Settings.Default.ClientSendUsername;
        public static string ClientSendPassword = Settings.Default.ClientSendPassword;
        public static string BackupClientSendUsername = Settings.Default.BackupClientSendUsername;
        public static string BackupClientSendPassword = Settings.Default.BackupClientSendPassword;
        public static StringCollection LogEmailList = Settings.Default.LogEmailList;
        public static int GetEmailsHrBack = Settings.Default.GetEmailsHrBack;
        public class EmailListEntity : IEquatable<EmailListEntity>
        {
            public string obj1 { get; set; }
            public string obj2 { get; set; }

            public bool Equals(EmailListEntity other)
            {
                if (other == null) return false;
                return obj2.Equals(other.obj2);
            }
            public override string ToString()
            {
                return obj1;
            }

        }

        public static int SMTPSend(MailMessage msg, string username, string pass)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(username, pass),
                    EnableSsl = true,

                };

                msg.From = new MailAddress(ClientSendUsername);
                BackupMsg = msg;
                smtpClient.Send(msg);

                return 1;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Mailbox unavailable"))
                {
                    SMTPSend(BackupMsg, BackupClientSendUsername, BackupClientSendPassword);
                    return 2;
                }
                return 0;
            }

        }
        public static void SMTPLogSend(MailMessage msg)
        {

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(ClientSendUsername, ClientSendPassword),
                EnableSsl = true,

            };

            foreach (var email in LogEmailList)
            {
                msg.To.Add(email);
            }

            msg.From = new MailAddress(ClientSendUsername);
            msg.Subject = $"Email_Parser {DateTime.Now.ToString("dd-MM-yyyy | HH:mm:ss")} | {msg.Subject}";
            smtpClient.Send(msg);

        }

        public static bool ClientConnect()
        {
            if (!EmailClient.IsConnected)
            {
                EmailClient.CheckCertificateRevocation = false;
                EmailClient.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
                EmailClient.Authenticate(ClientReceiveUsername, ClientReceivePassword);
                EmailClient.Inbox.Open(FolderAccess.ReadWrite);
            }

            if (EmailClient.IsConnected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void GetMessageList()
        {
            lock (this)
            {
                ClientConnect();

                var uids = EmailClient.Inbox.Search(SearchQuery.NotSeen.And(SearchQuery.DeliveredAfter(DateTime.Now.AddHours(-GetEmailsHrBack))));

                foreach (var uid in uids)
                {
                    var message = EmailClient.Inbox.GetMessage(uid);

                    if (EmailListReceived.Contains(new EmailListEntity { obj2 = message.From.Mailboxes.ElementAt(0).Address }))
                    {
                        MessageList.Add(message);

                    }
                }

                EmailClient.Inbox.AddFlags(uids, MessageFlags.Seen, true);
            }
            EmailClient.Disconnect(true);
        }


        public static void SendEmail(string messageStr)
        {

            string fileName;
            MailMessage msg = new MailMessage();
            BackupMsg = new MailMessage();


           msg.Subject = $"My subject";
           fileName = "Template.html";


            var template = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\EmailTemplates\{fileName}");


            //FOR PRODUCTION            
            foreach (var contact in AnswerEmailList)
            {
                msg.To.Add(contact.obj1);

            }

            template = template
                .Replace("{test1}", messageStr)
                .Replace("{test2}", messageStr ?? "Test2");


            msg.Body = template;
            msg.IsBodyHtml = true;


            SMTPSend(msg, ClientSendUsername, ClientSendPassword);

            msg.To.Clear();
         
        }

    }
}
