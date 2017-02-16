using BookStore.Domain.Abstract;
using BookStore.Domain.Concrete;
using BookStore.Domain.Entities;
using Moq;
using Ninject;
using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Configuration;

namespace BookStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel Kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            Kernel = kernelParam;
            AddBindings();
        }


        public object GetService(Type serviceType)
        {
            return Kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Kernel.GetAll(serviceType);
        }


        private void AddBindings()
        {
            //we add binding here
            //Mock<IBookRepository> mock = new Mock<IBookRepository>();
            //mock.Setup(b => b.Books).Returns(
            //    new List<Book>
            //    {
            //        new Book { Title="SQL Server", Description="Education Book"},
            //        new Book { Title="PHP 7", Description="Education Book"},
            //        new Book { Title="MVC", Description="Education Book"}
            //    }
            //    );



            Kernel.Bind<IBookRepository>().To<EFBookRepository>();
          

            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };

            Kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("setting", emailSettings);

            //Kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}