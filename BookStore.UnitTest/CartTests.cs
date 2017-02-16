using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStore.Domain.Entities;
using System.Linq;
using Moq;
using BookStore.Domain.Abstract;
using BookStore.WebUI.Controllers;
using System.Web.Mvc;
using BookStore.WebUI.Models;

namespace BookStore.UnitTest
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            //Arrange
            Book b1 = new Book { ISBN = 1, Title = "MYSQL" };
            Book b2 = new Book { ISBN = 2, Title = "Orical" };

            //Act
            Cart targat = new Cart();
            targat.AddItem(b1);
            targat.AddItem(b2, 3);

            CartLine[] resualt = targat.Lines.ToArray();

            //Assert
            Assert.AreEqual(resualt[0].Book, b1);
            Assert.AreEqual(resualt[1].Book, b2);


        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //Arrange
            Book b1 = new Book { ISBN = 1, Title = "MYSQL" };
            Book b2 = new Book { ISBN = 2, Title = "Orical" };

            //Act
            Cart targat = new Cart();
            targat.AddItem(b1);
            targat.AddItem(b2, 3);
            targat.AddItem(b1,2);
            CartLine[] resualt = targat.Lines
                                .OrderBy(cl => cl.Book.ISBN).ToArray();

            //Assert
            Assert.AreEqual(resualt.Length, 2);
            Assert.AreEqual(resualt[0].Quantity, 3);
            Assert.AreEqual(resualt[1].Quantity, 3);

        }


        [TestMethod]
        public void Can_Remove_Line()
        {
            //Arrange
            Book b1 = new Book { ISBN = 1, Title = "MYSQL" };
            Book b2 = new Book { ISBN = 2, Title = "Orical" };
            Book b3 = new Book { ISBN = 3, Title = "php" };


            //Act
            Cart targat = new Cart();
            targat.AddItem(b1);
            targat.AddItem(b2, 3);
            targat.AddItem(b3, 5);
            targat.AddItem(b2);
            targat.RemoveLine(b2);
            
            //Assert
            Assert.AreEqual(targat.Lines.Where(cl => cl.Book == b2).Count(), 0);
            Assert.AreEqual(targat.Lines.Count(), 2);

        }

        [TestMethod]
        public void Can_Calculate_Cart_Total()
        {
            //Arrange
            Book b1 = new Book { ISBN = 1, Title = "MYSQL", Price=100M };
            Book b2 = new Book { ISBN = 2, Title = "Orical",Price=50M };
            Book b3 = new Book { ISBN = 3, Title = "php", Price = 70m };


            //Act
            Cart targat = new Cart();
            targat.AddItem(b1);
            targat.AddItem(b2, 2);
            targat.AddItem(b3);
            decimal result = targat.ComputeTotalValue();

            //Assert
            Assert.AreEqual(result, 270M);
            

        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            //Arrange
            Book b1 = new Book { ISBN = 1, Title = "MYSQL", Price = 100M };
            Book b2 = new Book { ISBN = 2, Title = "Orical", Price = 50M };
            Book b3 = new Book { ISBN = 3, Title = "php", Price = 70m };


            //Act
            Cart targat = new Cart();
            targat.AddItem(b1);
            targat.AddItem(b2, 2);
            targat.AddItem(b3);
            targat.Clear();

            //Assert
            Assert.AreEqual(targat.Lines.Count(), 0);
        }


        [TestMethod]
        public void Can_Add_To_Cart()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(
                new Book[]
                {
                    new Book {ISBN = 1,Title ="ASP.NET",Specialization="Programming" }
                }.AsQueryable()
                );

            Cart cart = new Cart();
            CartController target = new CartController(mock.Object,null);


            //Act
            target.AddToCart(cart, 1, null);
           // RedirectToRouteResult result =  target.AddToCart(cart, 2, "myUrl");
            //Assert
            Assert.AreEqual(cart.Lines.Count(),1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Book.Title, "ASP.NET");

            // Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");

        }

        [TestMethod]
        public void Can_Adding_Book_To_Cart_Goes_To_Screen()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(
                new Book[]
                {
                    new Book {ISBN = 1,Title ="ASP.NET",Specialization="Programming" }
                }.AsQueryable()
                );

            Cart cart = new Cart();
            CartController target = new CartController(mock.Object,null);


            //Act
            target.AddToCart(cart, 1, null);
             RedirectToRouteResult result =  target.AddToCart(cart, 2, "myUrl");
            //Assert
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");


        }


        [TestMethod]
        public void Can_View_Cart_Content()
        {
            //Arrange
           
            Cart cart = new Cart();
            CartController target = new CartController(null,null);

            //Act
            CartIndexViewModel result =(CartIndexViewModel)  target.Index(cart, "myUrl").ViewData.Model ;
            //Assert
            Assert.AreEqual(result.Cart,cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");


        }


        [TestMethod]
        public void Can_Not_Checkout_Empty_Cart()
        {
            //Arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController target = new CartController(null, mock.Object);

            //Act
            ViewResult result = target.Checkout(cart, shippingDetails);

            //Assert

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }


        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            //Arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController target = new CartController(null, mock.Object);
            target.ModelState.AddModelError("error", "error");
            //Act
            ViewResult result = target.Checkout(cart, shippingDetails);

            //Assert
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),Times.Never());
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_and_submit_Order()
        {
            //Arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController target = new CartController(null, mock.Object);
          
            //Act
            ViewResult result = target.Checkout(cart, shippingDetails);

            //Assert
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());
            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
