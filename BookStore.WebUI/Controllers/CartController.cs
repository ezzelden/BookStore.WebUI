using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IBookRepository repository;

        private IOrderProcessor orderProcesser;

        public CartController(IBookRepository repo, IOrderProcessor pro)
        {
            repository = repo;
            orderProcesser = pro;
        }

        // GET: Cart
        public ViewResult Index(Cart cart, string returnUrl)
        {

            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }



        public RedirectToRouteResult AddToCart(Cart cart, int isbn, string returnUrl)
        {
            Book book = repository.Books
                .FirstOrDefault(b => b.ISBN == isbn);

            if (book != null)
            {
               cart.AddItem(book);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int isbn, string returnUrl)
        {
            Book book = repository.Books
               .FirstOrDefault(b => b.ISBN == isbn);
            if (book != null)
            {
                cart.RemoveLine(book);
            }
            return RedirectToAction("Index",new { returnUrl } );
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0) 
            {
                ModelState.AddModelError("", "Sorry Your Cart Is Empty");
            }

            if (ModelState.IsValid)
                {
                    orderProcesser.ProcessOrder(cart, shippingDetails);
                    cart.Clear();
                    return View("Completed");
                }
                else
                {
                    return View(shippingDetails);

                }
        
        }

    }
}