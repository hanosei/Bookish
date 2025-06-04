namespace Bookish.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string Name {get; set; }
        public string Email {get; set; }
        public int Mobile {get; set; }
        public DateTime DateJoined { get; set; } = DateTime.UtcNow;
    }
}