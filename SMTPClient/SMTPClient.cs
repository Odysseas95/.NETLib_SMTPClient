using MailKit.Net.Imap;
using System.Collections.Specialized;
using static SMTPClient.SMTPHelper;

namespace SMTPClient
{
    
    public class SMTPClient
    {
        public static ImapClient EmailClient;
        public static List<MimeKit.MimeMessage> MessageList;
        public static List<EmailListEntity> EmailListReceived;
        public static List<EmailListEntity> AnswerEmailList;

        public static StringCollection EmailListReceivedAvailability = Settings.Default.EmailListReceivedAvailability;
        public static StringCollection EmailListToAddresses = Settings.Default.EmailListToAddresses;

        public SMTPClient() 
        {
            //Email Client init
            EmailClient = new ImapClient();
            MessageList = new List<MimeKit.MimeMessage>();
            AnswerEmailList = new List<EmailListEntity>();

            //Memory Init
            //Last received message memory
            string[] adressesArray;
            foreach (var address in EmailListReceivedAvailability)
            {
                adressesArray = address.Split(',');
                EmailListReceived.Add(new EmailListEntity() { obj1 = adressesArray[0].Trim(), obj2 = adressesArray[1].Trim() });
            }

            string[] emailAnswerArray;
            foreach (var code in EmailListToAddresses)
            {
                emailAnswerArray = code.Split(',');
                AnswerEmailList.Add(new EmailListEntity() { obj1 = emailAnswerArray[0].Trim(), obj2 = emailAnswerArray[1].Trim() });
            }

            if (MessageList.Count != 0)
            {

                foreach (var message in MessageList)
                {
                    // Iterate through received messages
                }

            }

            SendEmail("This is a test email to send");
        }

    }
}