using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253503_Yarmak.Domain.Models
{
    /// <summary>
    /// Класс описывает формат данных, получаемых от сервисов
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseData<T>
    {
        //Запрашиваемые данные
        public T? Data { get; set; }
        //Признак успешного завершения запроса
        public bool Successfull { get; set; } = true;
        //Сообщение в случае неуспешного завершения
        public string? ErrorMessage { get; set; }
        /// <summary>
        /// Получить объект успешного ответа
        /// </summary>
        /// <param name="Data">передаваемые данные</param>
        /// <returns></returns>
        public static ResponseData<T> Success(T Data)
        {
            return new ResponseData<T> { Data = Data };
        }
        /// <summary>
        /// Получение объекта ответа с ошибкой
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        /// <param name="data">Передаваемые данные</param>
        /// <returns></returns>
        public static ResponseData<T> Error(string message, T? data = default)
        {
            return new ResponseData<T> { ErrorMessage = message,
            Successfull = false, Data = data };
        }
    }
}
