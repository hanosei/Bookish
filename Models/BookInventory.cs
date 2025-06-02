namespace Bookish.Models
{
    public class BookInventory
    {
        public int BookInventoryId { get; set; }

        public int BookId { get; set; } 
        public Book Book { get; set; } 
        
        public int AvailableCopies{ get; set; }
        public int TotalCopies { get; set; }
    }
}