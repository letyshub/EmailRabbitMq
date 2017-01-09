namespace EmailRabbitMq.Shared
{
    public class EmailQueueMessage
    {
        public string RecipientEmail { get; set; }
        public string RecipientName { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
