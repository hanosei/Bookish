namespace Bookish.Models
{
    public class BookCatalogue
    {
        public int BookCatalogueId { get; set; }

        public int BookId { get; set; } 
        public Book Book { get; set; } 
        
        public int BookInventoryId { get; set; } 
        public BookInventory BookInventory { get; set; } 
        
    }
} 