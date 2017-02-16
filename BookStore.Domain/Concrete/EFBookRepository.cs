using BookStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Entities;
using System.Data.Entity;

namespace BookStore.Domain.Concrete
{
    public class EFBookRepository : IBookRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<Book> Books
        {
            get
            {
                
                return context.Books;
            }
        }
        


        public void SaveBook(Book book)
        {
          
            Book dbEntity = context.Books.Find(book.ISBN);
            if (dbEntity==null)
            {
                context.Books.Add (book);
            }
            else
            {
                dbEntity.Title = book.Title;
                dbEntity.Specialization = book.Specialization;
                dbEntity.Price = book.Price;
                dbEntity.Description = book.Description;

            }
            context.SaveChanges();
        }
    }
}
