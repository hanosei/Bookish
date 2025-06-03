using Bookish.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookish
{
    public class BookishContext : DbContext
    {
        // Put all the tables you want in your database here
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCatalogue> BookCatalogues { get; set; }
        public DbSet<ActiveCheckout> ActiveCheckouts { get; set; }
        public DbSet<BookInventory> BookInventories { get; set; }
        public DbSet<Member> Members { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            // This is the configuration used for connecting to the database
            optionsBuilder.UseNpgsql(@"Server=localhost;Port=5432;Database=bookish;User Id=bookish;Password=bookish;");
        }
    }
}