using EmailRabbitMq.Shared;
using MailKit.Net.Smtp;
using MimeKit;

namespace EmailRabbitMq.Sender
{
    public class EmailSender
    {
        private string smtpServer;
        private int port;
        private string user;
        private string password;

        public EmailSender(string smtpServer, string port, string user, string password)
        {
            this.smtpServer = smtpServer;
            this.port = int.Parse(port);
            this.user = user;
            this.password = password;
        }

        public void Send(EmailQueueMessage message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(message.SenderName, message.SenderEmail));
            mimeMessage.To.Add(new MailboxAddress(message.RecipientName, message.RecipientEmail));
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new TextPart("plain")
            {
                Text = message.Body
            };

            using (var client = new SmtpClient())
            {
                client.Connect(this.smtpServer, this.port, false);
                client.Authenticate(this.user, this.password);
                client.Send(mimeMessage);
                client.Disconnect(true);
            }
        }
    }
}