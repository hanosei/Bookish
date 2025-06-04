using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bookish.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Bookish.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Bookish.Controllers
{
    [Route("ActiveCheckouts")]
    public class ActiveCheckoutsController : Controller
    {
        private readonly BookishContext _context;

        public ActiveCheckoutsController(BookishContext context)
        {
            _context = context;
        }

        [HttpGet("CreateCheckOut")]
        public IActionResult CreateCheckOut(int BookId)
        {
            var book = _context.Books.SingleOrDefault(book => book.BookId == book.BookId); //To Fix (error:sequence contains more than one element)
            if (book == null) return NotFound();
            //if(book.BookInventory.AvailableCopies <=0)

           var model = new ActiveCheckoutViewModel
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    Member = _context.Members.ToList(),
                    DueInDate = DateTime.UtcNow.AddMonths(1)
                };
                
            return View();
        }

        [HttpPost("CreateCheckOut")]
        public IActionResult CreateCheckout(ActiveCheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Needs member to be entered and verified with database 
                // Active check: Book Id, Member Id, Due In Date
                var checkout = new ActiveCheckout
                {
                    BookId = model.BookId,
                    MemberId = model.MemberId,
                    DueInDate = model.DueInDate
                };

                _context.ActiveCheckouts.Add(checkout);
                _context.SaveChanges();
            };
            

            return View();
        }
    }
}