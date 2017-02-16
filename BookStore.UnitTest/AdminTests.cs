using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Controllers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BookStore.UnitTest
{
    [TestClass]
    public class AdminTests
    {

        [TestMethod]
        public void Index_Contains_All_Products()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[]
            {
                new Book {ISBN = 1, Title = "Book1" },
                new Book {ISBN = 2, Title = "Book2" },
                new Book {ISBN = 3, Title = "Book3" }

            });
            AdminController target = new AdminController(mock.Object);

            //Act
            Book[] result = ((IEnumerable<Book>) target.Index().ViewData.Model).ToArray();

            //Assert
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("Book1", result[0].Title);
            Assert.AreEqual("Book2", result[1].Title);
            Assert.AreEqual("Book3", result[2].Title);


        }

        [TestMethod]
        public void Cannot_Edit_Book()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[]
            {
                new Book {ISBN = 1, Title = "Book1" },
                new Book {ISBN = 2, Title = "Book2" },
                new Book {ISBN = 3, Title = "Book3" }

            });
            AdminController target = new AdminController(mock.Object);

            //Act
            Book result = target.Edit(4).ViewData.Model as Book;

            //Assert
            Assert.IsNull(result);



        }

        [TestMethod]
        public void Can_Edit_Book()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            
            AdminController target = new AdminController(mock.Object);
            Book book = new Book { Title = "testbook" };
            //Act

            ActionResult result = target.Edit(book);
            //Assert
            mock.Verify(b => b.SaveBook(book));
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));

        }
      

        [TestMethod]
        public void Cannot_Save_Invalid_Book()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();

            AdminController target = new AdminController(mock.Object);
            Book book = new Book { Title = "testbook" };
            target.ModelState.AddModelError("error", "error");
            //Act

            ActionResult result = target.Edit(book);
            //Assert
            mock.Verify(b => b.SaveBook(It.IsAny<Book>()),Times.Never());
            Assert.IsInstanceOfType(result, typeof(ViewResult));

        }

    }
}
