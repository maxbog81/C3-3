using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailSender.lib.Data.Linq2SQL;

namespace MailSender.lib.Services.Linq2SQL
{
    /// <summary>Реализация сервиса работы с получателями для источника данных - контекста БД Linq2SQL</summary>
    public class RecipientsDataServiceLinq2SQL : IRecipientsDataService
    {
        /// <summary>Контектс базы данных</summary>
        private readonly MailSenderDBContext _db;

        /// <summary>Инициализация нового сервиса <seealso cref="RecipientsDataServiceLinq2SQL"/></summary>
        /// <param name="db">Контекст БД Linq2SQL</param>
        public RecipientsDataServiceLinq2SQL(MailSenderDBContext db) // Запрашиваем контекст БД из системы внедрения зависимостей
        {
            _db = db; // Сохраняем полученный контекст в приватное поле
        }


        /// <summary>Извлечь всех получателей из контекста БД</summary>
        /// <returns>Перечисление всех получателей, хранимый в контексте БД</returns>
        public IEnumerable<Recipient> GetAll()
        {
            return _db.Recipient.ToArray();
        }


        /// <summary>Создать (зарегистрировать) нового получателя почты в контексте БД</summary>
        /// <param name="item">Создаваемый новый получатель</param>
        public void Create(Recipient item)
        {
            if (item.Id != 0) return;
            _db.Recipient.InsertOnSubmit(item);
            _db.SubmitChanges();
        }

        /// <summary>Обновить данные получателя</summary>
        /// <param name="item">Получатель почты, данные которого требуется обновить</param>
        public void Update(Recipient item)
        {
            _db.SubmitChanges();
        }

        /// <summary>Удалить получателя из БД</summary>
        /// <param name="item">Получатель почты, которого требуется удалить</param>
        public void Delete(Recipient item)
        {
            _db.Recipient.DeleteOnSubmit(item);
            _db.SubmitChanges();
        }
    }
}
