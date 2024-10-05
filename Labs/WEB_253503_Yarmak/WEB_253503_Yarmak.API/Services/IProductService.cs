using WEB_253503_Yarmak.Domain.Entities;
using WEB_253503_Yarmak.Domain.Models;

namespace WEB_253503_Yarmak.API.Services
{
    public interface IProductService
    {
        /// <summary>
        /// Получение списка всех обхектов
        /// </summary>
        /// <param name="categoryNormalizedName">нормализованное имя категории для фильтрации</param>
        /// <param name="pageNo">номер страницы списка</param>
        /// <param name="pageSize">количество объектов на странице</param>
        /// <returns></returns>
        Task<ResponseData<ProductListModel<Phone>>> GetProductListAsync(
            string? categoryNormalizedName,
            int pageNo = 1,
            int pageSize = 3);

        /// <summary>
        /// Поиск объекта по id
        /// </summary>
        /// <param name="id">идентификатор объекта</param>
        /// <returns></returns>
        Task<ResponseData<Phone>> GetProductByIdAsync(
            int id);
        /// <summary>
        /// Обновление объекта
        /// </summary>
        /// <param name="id">Id изменяемого объекта</param>
        /// <param name="phone">Объект с новыми параметрами</param>
        /// <returns></returns>
        Task UpdateProductAsync(int id, Phone phone);
        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <param name="id">Id удаляемого объекта</param>
        /// <returns></returns>
        Task DeleteproductAsync(int id);
        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="phone">новый объект</param>
        /// <returns>Созданный объект</returns>
        Task<ResponseData<Phone>> CreateProductAsync(Phone phone);
        /// <summary>
        /// Сохранить файл изображения для объекта
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <param name="formFile">файл изображения</param>
        /// <returns>Url к файлу изображения</returns>
        Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
        /// <summary>
        /// Получить все элементы без разделения на страницы
        /// </summary>
        /// <returns></returns>
        Task<ResponseData<ProductListModel<Phone>>> GetProductListWithoutPageAsync(string? categoryNormalizedName);
    }
}
