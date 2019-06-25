using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace EmailSendService
{
    public class EmailSendServiceClass
    {
        #region vars
        private string strLogin;         // email, c которого будет рассылаться почта
        private string strPassword;  // пароль к email, с которого будет рассылаться почта
        private string strSmtp;               // smtp-server
        private int iSmtpPort;                // порт для smtp-server
        private string strBody;                // текст письма для отправки
        private string strSubject = "Привет из C#";          // тема письма для отправки
        #endregion
        public EmailSendServiceClass(string sLogin, string sPassword, string sSmtp, int iPort, string sBody)
        {
            strLogin = sLogin;
            strPassword = sPassword;
            strSmtp = sSmtp;
            iSmtpPort = iPort;
            strBody = sBody;
        }
        private void SendMail(string mail, string name) // Отправка email конкретному адресату
        {
            using (MailMessage mm = new MailMessage(strLogin, mail))
            {
                mm.Subject = strSubject;
                mm.Body = strBody;
                mm.IsBodyHtml = false;
                SmtpClient sc = new SmtpClient(strSmtp, iSmtpPort);

                sc.EnableSsl = true;
                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                sc.UseDefaultCredentials = false;
                sc.Credentials = new NetworkCredential(strLogin, strPassword);
                try
                {
                    sc.Send(mm);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Невозможно отправить письмо " + ex.ToString());
                }
            }
        }//private void SendMail(string mail, string name)
        public void SendMails(IQueryable<Email> emails)
        {
            foreach (Email email in emails)
            {
                SendMail(email.Value, email.Name);
            }
        }
    }  //private void SendMail(string mail, string name)


}
