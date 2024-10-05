using WEB_253503_Yarmak.Domain.Entities;
using WEB_253503_Yarmak.Domain.Models;

namespace WEB_253503_Yarmak.API.Services
{
    public interface ICategoryService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <param name="categoryNormalizesNmae"></param>
        /// <returns></returns>
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
