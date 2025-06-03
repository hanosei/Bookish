namespace Bookish.Models
{
    public class ActiveCheckout
    {
        public int ActiveCheckoutId { get; set; }

        public int BookId { get; set; } 
        public Book Book { get; set; } 
        
        public int MemberId { get; set; } 
        public Member Member { get; set; } 
        
        public DateTime DueInDate {get; set;}
    }
} 