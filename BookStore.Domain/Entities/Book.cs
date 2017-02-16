
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookStore.Domain.Entities
{
    public class Book
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int ISBN { get; set; }
        [Required(ErrorMessage = "Please Enter a Book Titel")]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please Enter a Book Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please Enter a Book Price")]
        [Range(1,9999.99,ErrorMessage ="Please Enter A Positive Price")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Please Enter a Book Specialization")]
        public string Specialization { get; set; }

    }
}
