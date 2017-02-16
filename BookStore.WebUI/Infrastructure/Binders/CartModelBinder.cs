using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BookStore.WebUI.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext ControllerContext,
                                ModelBindingContext bindingContext)
        {
            //get cart from session 
            Cart cart = null;
            if (ControllerContext.HttpContext.Session != null)
            {
                cart = (Cart) ControllerContext.HttpContext.Session[sessionKey];

            }
            if (cart == null)
            {
                cart = new Cart();
                if (ControllerContext.HttpContext.Session != null)
                {
                    ControllerContext.HttpContext.Session[sessionKey] = cart;
                }
            }

            return cart;

        }

        
    }
}