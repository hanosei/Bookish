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

        [HttpGet("MemberCheckouts")]
        public IActionResult MemberCheckouts(int MemberId)
        {
            var member = _context.Members.Find(MemberId); // to try and find a member in the db by their ID
            if (member == null) return NotFound(); //just added validation incase cant find member

            var activeCheckout = _context.ActiveCheckouts.Where(activecheckout => activecheckout.MemberId == MemberId)
                                .Include(activecheckout => activecheckout.Book)
                                .ToList();

            var viewModel = new MemberWithActiveCheckoutsViewModel
            {
                Member = member,
                ActiveCheckouts = activeCheckout,
                // Title = activeCheckout.Book.Title
            };
            // Console.Write(viewModel.ActiveCheckouts.Book.Title);

            return View(viewModel);
        }
        
        [HttpPost("Delete")]
        public IActionResult Delete(int ActiveCheckoutId)
        {
            var checkout = _context.ActiveCheckouts.SingleOrDefault(checkout => checkout.ActiveCheckoutId == ActiveCheckoutId);

            if (checkout == null) return BadRequest();

             var checkedBookInventory = _context.BookInventories.SingleOrDefault(book => book.BookId == checkout.BookId);
                if (checkedBookInventory != null)
                {
                    checkedBookInventory.AvailableCopies += 1;
                }

            _context.ActiveCheckouts.Remove(checkout);
            _context.SaveChanges();
            return RedirectToAction("MemberCheckouts", new { MemberId = checkout.MemberId });
        }

    }
}