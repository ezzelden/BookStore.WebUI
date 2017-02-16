using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BookStore.WebUI.Infrastructure.Abstract;
using BookStore.WebUI.Models;
using BookStore.WebUI.Controllers;
using System.Web.Mvc;

namespace BookStore.UnitTest
{
    [TestClass]
    public class AdminSecurityTest
    {
        [TestMethod]
        public void Can_Login_With_valid_credentials()
        {
            //Arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "admin")).Returns(true);
            LoginViewModel model = new LoginViewModel {
                UserName = "admin",
                Password = "admin"
            };
            AccountController target = new AccountController(mock.Object);

            //Act 
            ActionResult result = target.Login(model, "/MyUrl");


            //assert
            Assert.IsInstanceOfType(result,typeof(RedirectResult));
            Assert.AreEqual("/MyUrl",((RedirectResult) result).Url);

        }

        [TestMethod]
        public void Can_Login_With_Invalid_credentials()
        {
            //Arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admins", "admins")).Returns(false);
            LoginViewModel model = new LoginViewModel
            {
                UserName = "admins",
                Password = "admins"
            };
            AccountController target = new AccountController(mock.Object);

            //Act 
            ActionResult result = target.Login(model, "/MyUrl");


            //assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);

        }
    }
}
