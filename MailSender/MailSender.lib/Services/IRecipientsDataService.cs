using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailSender.lib.Data.Linq2SQL;

namespace MailSender.lib.Services
{
    /// <summary>Сервис работы с данными получателей почты</summary>
    public interface IRecipientsDataService
    {
        /// <summary>Извлечь всех получателей</summary>
        /// <returns>Перечисление всех получателей, известных сервису</returns>
        IEnumerable<Recipient> GetAll();

        /// <summary>Создать (зарегистрировать) нового получателя почты</summary>
        /// <param name="item">Создаваемый новый получатель</param>
        void Create(Recipient item);

        /// <summary>Обновить данные получателя</summary>
        /// <param name="item">Получатель почты, данные которого требуется обновить</param>
        void Update(Recipient item);

        /// <summary>Удалить получателя</summary>
        /// <param name="item">Получатель почты, которого требуется удалить</param>
        void Delete(Recipient item);
    }
}