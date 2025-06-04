using Bookish.Models;

namespace Bookish.ViewModels
{
    public class ActiveCheckoutViewModel
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int MemberId { get; set; }
        public List<Member> Member { get; set; }= new List<Member>();
        public DateTime DueInDate { get; set; }
    }
};
