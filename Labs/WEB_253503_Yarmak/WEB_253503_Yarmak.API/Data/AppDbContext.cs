using Microsoft.EntityFrameworkCore;
using WEB_253503_Yarmak.Domain.Entities;

namespace WEB_253503_Yarmak.API.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Phone> Phones { get; set; }
    }
}
