using Bookish.Models;
using Microsoft.AspNetCore.SignalR;

namespace Bookish.ViewModels
{
    public class MemberWithActiveCheckoutsViewModel
    {
        public Member Member { get; set; }
        public bool HasActiveCheckouts { get; set; }

        public List<ActiveCheckout> ActiveCheckouts { get; set; } = new List<ActiveCheckout>();
    }
}
