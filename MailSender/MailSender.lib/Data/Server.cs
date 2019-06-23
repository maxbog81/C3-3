namespace MailSender.lib.Data
{
    public class Server
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int Port { get; set; } = 25;
        public bool UseSSL { get; set; } = true;

        public string UserName { get; set; }
        public string Password { get; set; }
    }
}