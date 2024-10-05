using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using WEB_253503_Yarmak.API.Data;
using WEB_253503_Yarmak.Domain.Entities;
using WEB_253503_Yarmak.Domain.Models;

namespace WEB_253503_Yarmak.API.Services
{
    public class ProductService : IProductService
    {
        private readonly int _maxPageSize = 20;
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseData<Phone>> CreateProductAsync(Phone phone)
        {
            _context.Phones.Add(phone);
            await _context.SaveChangesAsync();
            return ResponseData<Phone>.Success(phone);
        }

        public async Task DeleteproductAsync(int id)
        {
            var phone = await _context.Phones.FindAsync(id);
            if (phone == null)
            {
                throw new KeyNotFoundException("Phone not found!");
            }
            _context.Phones.Remove(phone);
            await _context.SaveChangesAsync();
        }

        public async Task<ResponseData<Phone>> GetProductByIdAsync(int id)
        {
            var phone = await _context.Phones.FindAsync(id);
            if (phone == null)
            {
                return ResponseData<Phone>.Error("Phone not found");
            }
            return ResponseData<Phone>.Success(phone);
        }

        public async Task<ResponseData<ProductListModel<Phone>>> GetProductListAsync(
            string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageNo > _maxPageSize)
                pageSize = _maxPageSize;

            var phoneQuery = _context.Phones.AsQueryable();
            var categoryQuery = _context.Categories.AsQueryable();
            var category = categoryQuery.First(c => categoryNormalizedName == null || categoryNormalizedName == c.NormalizedName);
            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                phoneQuery = phoneQuery.Where(d => d.CategoryId == category.Id);
            }
            var dataList = new ProductListModel<Phone>();

            var count = await phoneQuery.CountAsync();

            if (count == 0)
            {
                return ResponseData<ProductListModel<Phone>>.Success(dataList);
            }
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);

            if (pageNo > totalPages)
                return ResponseData<ProductListModel<Phone>>.Error("No such page");
            dataList.Items = phoneQuery.ToList();
            dataList.Items = await phoneQuery
                                .OrderBy(d => d.Id)
                                .Skip((pageNo - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;
            return ResponseData<ProductListModel<Phone>>.Success(dataList);

        }

        public async Task<ResponseData<ProductListModel<Phone>>> GetProductListWithoutPageAsync(string? categoryNormalizedName)
        {

            var phoneQuery = _context.Phones.AsQueryable();
            var categoryQuery = _context.Categories.AsQueryable();
            var category = categoryQuery.FirstOrDefault(c => categoryNormalizedName == null || categoryNormalizedName == c.NormalizedName);
            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                phoneQuery = phoneQuery.Where(d => d.CategoryId == category.Id);
            }
            var dataList = new ProductListModel<Phone>();
            
            var count = await phoneQuery.CountAsync();
            if (count == 0)
            {
                return ResponseData<ProductListModel<Phone>>.Success(dataList);
            }

            dataList.Items = phoneQuery.ToList();
            dataList.TotalPages = (int)Math.Ceiling(count /(double)dataList.PageSize);
            return ResponseData<ProductListModel<Phone>>.Success(dataList);
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return ResponseData<string>.Error("No file upload");
            }

            var imgPath = Path.Combine("wwwroot", "Images");
            if (!Directory.Exists(imgPath))
            {
                Directory.CreateDirectory(imgPath);
            }

            var fileName = $"{id}_{Path.GetFileName(formFile.FileName)}";

            var filePath = Path.Combine(imgPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            var url = $"{Path.Combine("Image", fileName)}";
            return ResponseData<string>.Success(url);
        }

        public async Task UpdateProductAsync(int id, Phone phone)
        {
            if (id != phone.Id)
            {
                throw new ArgumentException("Phone Id don't access");
            }
            _context.Entry(phone).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
