using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStore.Domain.Abstract;
using Moq;
using BookStore.Domain.Entities;
using BookStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BookStore.WebUI.Models;
using BookStore.WebUI.HtmlHelper;

namespace BookStore.UnitTest
{
    [TestClass]
    public class ProductTests
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Book[]
            {
                new Book { ISBN=1 ,Title="book1" },
                new Book { ISBN=2 ,Title="book2" },
                new Book { ISBN=3 ,Title="book3" },
                new Book { ISBN=4 ,Title="book4" },
                new Book { ISBN=5 ,Title="book5" },
                new Book { ISBN=6 ,Title="book6" }
            });
            BookController controller = new BookController(mock.Object);
            controller.PageSize = 3;

            //Act
            BookListViewModel result = (BookListViewModel) controller.List(null,2).Model;

            //Assert
            Book[] bookArray =  result.Books.ToArray();
            Assert.IsTrue(bookArray.Length == 3);
            Assert.AreEqual(bookArray[0].Title, "book4");
            Assert.AreEqual(bookArray[1].Title, "book5");
            Assert.AreEqual(bookArray[2].Title, "book6");

        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //Arrange
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 14,
                ItemPerPage = 5
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            String expectedResult = "<a class=\"btn btn-default\" href=\"Page1\">1</a>"
                                   + "<a class=\"btn btn-default btn-primary selected\" href=\"Page2\">2</a>"
                                   + "<a class=\"btn btn-default\" href=\"Page3\">3</a>";
            //Act

            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            //Assert
            Assert.AreEqual(expectedResult, result.ToString());

        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            
            mock.Setup(b => b.Books).Returns(
                new Book[]
                {
                    new Book { ISBN = 1 , Title="Operating System"},
                    new Book { ISBN = 2, Title = "Database System" }
                });
            BookController controller = new BookController(mock.Object);
            controller.PageSize = 1;
            //Act
            BookListViewModel result = (BookListViewModel)controller.List(null,2).Model;

            //Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemPerPage, 1);
            Assert.AreEqual(pageInfo.TotalItems, 2);
            Assert.AreEqual(pageInfo.TotalPages, 2);




        }

        [TestMethod]
        public void Can_Filter_Books()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();

            mock.Setup(b => b.Books).Returns(
                new Book[]
                {
                    new Book { ISBN = 1 , Title="Operating System" ,Specialization="CS"},
                    new Book { ISBN = 2, Title = "Database System" , Specialization="IS"},
                    new Book { ISBN = 3, Title = "Database System" , Specialization="IS"},
                    new Book { ISBN = 4, Title = "System" , Specialization="IS"},
                    new Book { ISBN = 5, Title = "IS" , Specialization="IS"}

                });
            BookController controller = new BookController(mock.Object);
            controller.PageSize = 3;

            //Act
            Book[] result = ((BookListViewModel)
                            controller.List("IS", 2).Model).Books.ToArray();


            //Assert
            Assert.AreEqual(result.Length, 1);
            Assert.IsTrue(result[0].Title == "IS" && result[0].Specialization == "IS");

        }


        [TestMethod]
        public void Can_Create_specilizarion()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();

            mock.Setup(b => b.Books).Returns(
                new Book[]
                {
                    new Book { ISBN = 1 , Title="Operating System" ,Specialization="CS"},
                    new Book { ISBN = 2, Title = "Database System" , Specialization="IS"},
                    new Book { ISBN = 3, Title = "Database System" , Specialization="IS"},
                    new Book { ISBN = 4, Title = "System" , Specialization="IS"},
                    new Book { ISBN = 5, Title = "IS" , Specialization="IS"}

                });
            NavController controller = new NavController(mock.Object);
           

            //Act
            string[] result = ((IEnumerable<string>)
                            controller.Menu().Model).ToArray();


            //Assert
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0] == "CS" && result[1]  == "IS");

        }

        [TestMethod]
        public void Indicates_Selected_Spec()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();

            mock.Setup(b => b.Books).Returns(
                new Book[]
                {
                    new Book { ISBN = 1 , Title="Operating System" ,Specialization="CS"},
                    new Book { ISBN = 2, Title = "Database System" , Specialization="IS"},
                    new Book { ISBN = 3, Title = "Database System" , Specialization="IS"},
                    new Book { ISBN = 4, Title = "System" , Specialization="IS"},
                    new Book { ISBN = 5, Title = "IS" , Specialization="IS"}

                });
            NavController controller = new NavController(mock.Object);


            //Act
            string result = controller.Menu("IS").ViewBag.SelectedSpec;


            //Assert
            Assert.AreEqual("IS", result);
            

        }

        [TestMethod]
        public void Generate_Spec_Specific_Book_Count()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();

            mock.Setup(b => b.Books).Returns(
                new Book[]
                {
                    new Book { ISBN = 1 , Title="Operating System" ,Specialization="CS"},
                    new Book { ISBN = 2, Title = "Database System" , Specialization="IS"},
                    new Book { ISBN = 3, Title = "Database System" , Specialization="IS"},
                    new Book { ISBN = 4, Title = "System" , Specialization="CS"},
                    new Book { ISBN = 5, Title = "IS" , Specialization="IS"}

                });
             BookController controller = new BookController(mock.Object);


            //Act
            int result1 = ((BookListViewModel)controller.List("IS").Model).PagingInfo.TotalItems;
            int result2 = ((BookListViewModel)controller.List("CS").Model).PagingInfo.TotalItems;


            //Assert
            Assert.AreEqual(result1, 3);
            Assert.AreEqual(result2, 2);

        }
    }
}
