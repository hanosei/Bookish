using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bookish.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Bookish.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;

namespace Bookish.Controllers
{
    [Route("Members")]
    public class MembersController : Controller
    {
        private readonly BookishContext _context;

        public MembersController(BookishContext context)
        {
            _context = context;
        }


        [HttpGet("Index")]
        public IActionResult Index()
        {
            var members = _context.Members.Select(member => new MemberWithActiveCheckoutsViewModel //I used select to create an instance of ViewModel Class with 2 properties (ones below)
            {
                Member = member, //entire member object (id,name, etc)
                HasActiveCheckouts = _context.ActiveCheckouts.Any(checkout => checkout.MemberId == member.MemberId) //boolean of if they have checkouts
            }).ToList(); //put it back in a list (moved it from line 27)

            return View(members);
        }

        [HttpGet("Add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("Add")]
        public IActionResult Add(Member member)
        {
            if (ModelState.IsValid)
            {
                member.DateJoined = DateTime.UtcNow;
                _context.Members.Add(member);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ;

            return View(member);
        }

        [HttpGet("Edit")]
        public IActionResult Edit(int MemberId)
        {
            var member = _context.Members.SingleOrDefault(member => member.MemberId == MemberId);
            if (member == null) return BadRequest();
            return View(member);

        }

        [HttpPost("Edit")]
        public IActionResult Edit(Member member)
        {

            if (ModelState.IsValid)
            {
                member.DateJoined = DateTime.SpecifyKind(member.DateJoined, DateTimeKind.Utc);

                member.DateJoined = member.DateJoined;
                _context.Members.Update(member);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(int MemberId)
        {
            var member = _context.Members.SingleOrDefault(member => member.MemberId == MemberId);

            if (member == null) return BadRequest();

            _context.Members.Remove(member);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}