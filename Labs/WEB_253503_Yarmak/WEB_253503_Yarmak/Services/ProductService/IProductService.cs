using WEB_253503_Yarmak.Domain.Entities;
using WEB_253503_Yarmak.Domain.Models;

namespace WEB_253503_Yarmak.Services.ProductService
{
    /// <summary>
    /// Описывает функции, необходимые для работы приложения
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// получение списка всех объектов
        /// </summary>
        /// <param name="categoryNormalizedName">нормализованное имя категории
        ///для фильтрации</param>
        /// <param name="pageNo">номер страницы списка</param>
        /// <returns></returns>
        public Task<ResponseData<ProductListModel<Phone>>> GetProductListAsync(
            string? categoryNormalizedName, int pageNo = 1);
        /// <summary>
        /// Поиск объекта по id
        /// </summary>
        /// <param name="id">Идентификаторобъекта</param>
        /// <returns>Найденный объект или null, если объект не найден</returns>
        public Task<ResponseData<Phone>> GetProductByIdAsync(int id);
        /// <summary>
        /// Обновление объекта
        /// </summary>
        /// <param name="id">Id изменяемого объекта</param>
        /// <param name="product">объект с новыми параметрами</param>
        /// <param name="formFile">Файл изображения</param>
        /// <returns></returns>
        public Task UpdateProductasyc(int id, Phone product, IFormFile? formFile);
        /// <summary>
        /// Удалеие объекта
        /// </summary>
        /// <param name="id">Id удаляемого объект</param>
        /// <returns></returns>
        public Task DeleteProductAsync(int id);
        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="product">Новый объект</param>
        /// <param name="formFile">Файл изображения</param>
        /// <returns></returns>
        public Task<ResponseData<Phone>> CreateProductAsync(
            Phone product, IFormFile? formFile);

        /// <summary>
        /// Получить все элементы без разделения на страницы
        /// </summary>
        /// <returns></returns>
        Task<ResponseData<ProductListModel<Phone>>> GetProductListWithoutPageAsync(string? categoryNormalizedName);
    }
}
