using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253503_Yarmak.Domain.Models
{
    /// <summary>
    /// Класс описывает данные используемые при получении списка объектов
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProductListModel<T>
    {
        //Запрошенный список объектов
        public List<T> Items { get; set; } = new();
        //номер текущей страницы
        public int CurrentPage { get; set; } = 1;
        //общее количество страниц
        public int TotalPages { get; set; } = 1;
        public int PageSize { get; set; } = 3;
        public ProductListModel(int count, int currentPage, int PageSize)
        {
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(count/(double)PageSize);
        }
        public ProductListModel()
        {
            
        }
        public bool HasPreviousPage
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }
    }
}
