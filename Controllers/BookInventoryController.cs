using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bookish.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Bookish.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Bookish.Controllers {

    [Route("BookInventory")]
    public class BookInventoryController : Controller
    {
        private readonly BookishContext _context;

        public BookInventoryController(BookishContext context)
        {
            _context = context;
        }


        [HttpGet("Index")]
        public IActionResult Index(string search)
        {
            var books = from BookInventory in _context.BookInventories
                        join Book in _context.Books
                        on BookInventory.BookId equals Book.BookId
                        select new BookInventoryViewModel
                        {
                            BookId = Book.BookId,
                            Title = Book.Title,
                            Author = Book.Author,
                            AvailableCopies = BookInventory.AvailableCopies,
                            TotalCopies = BookInventory.TotalCopies,
                        };

            if (!string.IsNullOrEmpty(search))
            {
                books = books.Where(books => books.Title.Contains(search) || books.Author.Contains(search));
            }
            return View(books.ToList());
        }

        [HttpGet("Add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("Add")]
        public IActionResult Add(BookInventoryViewModel bookInventoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var book = new Book
                {
                    Title = bookInventoryViewModel.Title,
                    Author = bookInventoryViewModel.Author
                };

                _context.Books.Add(book);
                _context.SaveChanges();

                // var book = _context.Books.Find(BookId);

                var inventory = new BookInventory
                {
                    BookId = book.BookId,
                    AvailableCopies = bookInventoryViewModel.TotalCopies,
                    TotalCopies = bookInventoryViewModel.TotalCopies
                };

                _context.BookInventories.Add(inventory);
                _context.SaveChanges();
            }

            return ModelState.IsValid ? RedirectToAction("Index") : View(bookInventoryViewModel);
        }

        [HttpGet("Edit")]
        public IActionResult Edit(int BookId)
        {
            var book = _context.Books.SingleOrDefault(book => book.BookId == BookId);
            var inventory = _context.BookInventories.SingleOrDefault(book => book.BookId == BookId);

            if (book == null || inventory == null)
            {
                return BadRequest();
            }

            var model = new BookInventoryViewModel
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                AvailableCopies = inventory.AvailableCopies,
                TotalCopies = inventory.TotalCopies
            };

            return View(model);
        }

        [HttpPost("Edit")]
        public IActionResult Edit(BookInventoryViewModel model)
        {
            if (ModelState.IsValid)
            {

                var book = _context.Books.SingleOrDefault(book => book.BookId == model.BookId);
                var inventory = _context.BookInventories.SingleOrDefault(book => book.BookId == model.BookId);

                if (book == null || inventory == null)
                {
                    return NotFound();
                }

                book.Title = model.Title;
                book.Author = model.Author;
                inventory.AvailableCopies = model.AvailableCopies;
                inventory.TotalCopies = model.TotalCopies;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(int BookId)
        {
            var book = _context.Books.SingleOrDefault(book => book.BookId == BookId);
            var inventory = _context.BookInventories.SingleOrDefault(book => book.BookId == BookId);

            if (book == null || inventory == null)
            {
                return BadRequest();
            }

            _context.BookInventories.Remove(inventory);
            _context.Books.Remove(book);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    };
};
