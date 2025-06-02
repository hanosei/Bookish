namespace Bookish.Models
{
    public class ActiveCheckouts
    {
        public int ActiveCheckoutsId { get; set; }

        public int BookId { get; set; } 
        public Book Book { get; set; } 
        
        public int MemberId { get; set; } 
        public Member Member { get; set; } 
        
        public DateTime DueInDate {get; set;}
    }
} 