using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bookish.Models;

namespace Bookish.Controllers;

[Route("Books")]
public class BooksController : Controller
{
    private readonly ILogger<BooksController> _logger;

    // public BooksController(ILogger<BooksController> logger)
    // {
    //     _logger = logger;
    // }
    

    [HttpGet("Catalogue")]
    public IActionResult Catalogue() {
        var books = new List<Book>
            {
            new Book { BookId = 1, Title = "Clifford the Big Red Dog", Author = "Norman Bridwell"},
            new Book { BookId = 2, Title = "Of Mice and Men", Author = "John Steinbeck"},
            };
        
            return View(books);

    }
};
