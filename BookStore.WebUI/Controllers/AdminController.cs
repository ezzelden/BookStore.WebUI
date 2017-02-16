using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    //[Authorize]
    public class AdminController : Controller
    {
        private IBookRepository repository;

        public AdminController(IBookRepository repo )
        {
            repository = repo;
        }

        // GET: Admin
        public ViewResult Index()
        {

            return View(repository.Books);
        }

        public ViewResult Edit(int ISBN)
        {
            Book book = repository.Books.FirstOrDefault(b => b.ISBN == ISBN);
            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                repository.SaveBook(book);
                TempData["Message"] = book.Title + "Has Been Saved";

                return RedirectToAction("Index");
            }
            else
            {
                return View(book);
            }

        }

    }
}