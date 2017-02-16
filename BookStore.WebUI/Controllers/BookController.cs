using BookStore.Domain.Abstract;
using BookStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class BookController : Controller
    {

        private IBookRepository repository;
        public int PageSize = 4;


        public BookController(IBookRepository bookRep)
        {
            repository = bookRep;
        }

        public ViewResult List(string specilization, int page=1)
        {
            BookListViewModel model = new BookListViewModel
            {
                Books = repository.Books
                .Where(b => specilization == null || b.Specialization == specilization)
                .OrderBy(b => b.ISBN)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),

                PagingInfo = new Models.PagingInfo
                {
                    CurrentPage = page,
                    ItemPerPage = PageSize,
                    TotalItems =
                    specilization == null ?
                    repository.Books.Count() :
                    repository.Books.Where(b => b.Specialization == specilization).Count()
                },
                CurrentSpecilization = specilization
        };

            return View(model);
        }

        public ViewResult ListAll()
        {
            return View(repository.Books);
        }

    }

}