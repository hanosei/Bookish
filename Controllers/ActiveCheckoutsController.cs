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

        [HttpGet("CreateCheckout")]
        public IActionResult CreateCheckOut(int BookId)
        {
            var book = _context.Books.SingleOrDefault(book => book.BookId == BookId);
            if (book == null) return NotFound();

           var model = new ActiveCheckoutViewModel
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    Member = _context.Members.ToList(),
                    DueInDate = DateTime.UtcNow.AddMonths(1)
                };
                
            return View(model);
        }

        [HttpPost("CreateCheckout")]
        public IActionResult CreateCheckout(ActiveCheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                var checkout = new ActiveCheckout
                {
                    BookId = model.BookId,
                    MemberId = model.MemberId,
                    DueInDate = model.DueInDate.ToUniversalTime()
                };

                _context.ActiveCheckouts.Add(checkout);

                var checkedBookInventory = _context.BookInventories.SingleOrDefault(book => book.BookId == model.BookId);
                if (checkedBookInventory != null && checkedBookInventory.AvailableCopies > 0)
                {
                    checkedBookInventory.AvailableCopies -= 1;
                    _context.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "No available copies to check out");
                    model.Member = _context.Members.ToList();
                    return View(model);
                }
                ;

                _context.SaveChanges();
                return RedirectToAction("Index", "BookInventory");
            }

            model.Member = _context.Members.ToList();
            return View(model);
        }
    }
}