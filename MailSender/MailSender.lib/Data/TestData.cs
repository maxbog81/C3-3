using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.lib.Data
{
    public static class TestData
    {
        public static Server[] Servers { get; } = Enumerable.Range(1, 10)
           .Select(i => new Server
            {
                Name = $"Server {i}",
                Address = $"smtp.server{i}.ru",
                Port = 25,
                UserName = $"Server {i} user",
                Password = $"Password {i}"
            })
           .ToArray();

        public static Sender[] Senders { get; } = Enumerable.Range(1, 10)
           .Select(i => new Sender
            {
                Name = $"Sender {i}",
                Address = $"sender{i}@server.ru"
            })
           .ToArray();

        public static MailMessage[] Messages { get; } = Enumerable.Range(1, 10)
           .Select(i => new MailMessage
            {
                Subject = $"Message {i}",
                Body = $"Message {i} body"
            })
           .ToArray();
    }
}
