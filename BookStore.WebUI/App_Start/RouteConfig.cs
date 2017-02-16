using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute( // url/
                name: null,
                url: "",
                defaults: new { controller = "Book", action = "List" ,specilization =(string) null,page = 1}
            );

            routes.MapRoute( // url/BookListPage2
              name: null,
              url: "BookListPage{page}",
              defaults: new { controller = "Book", action = "List", specilization = (string) null}
          );

            routes.MapRoute( // url/Computer Seince
                name: null,
                url: "{specilization}",
                defaults: new { controller = "Book", action = "List", page = 1}
            );

            routes.MapRoute( // url/IS/Page2
                name: null,
                url: "{specilization}/Page",
                defaults: new { controller = "Book", action = "List" , page= 1 }
            //  constraints: new { page =@"\d+"}
            );

            routes.MapRoute( // url/IS/Page2
                name: null,
                url: "{specilization}/Page{page}",
                defaults: new { controller = "Book", action = "List"}
              //  constraints: new { page =@"\d+"}
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Book", action = "List", id = UrlParameter.Optional }
            );
        }
    }
}
