using Item1.Models;
using Microsoft.EntityFrameworkCore;

namespace Item1.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) :base(options){}
        public DbSet<Employees> employees {get; set;}
    }
}