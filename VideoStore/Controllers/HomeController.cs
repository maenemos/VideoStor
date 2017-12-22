using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoStore.Context;
using VideoStore.migration;

namespace VideoStore.Controllers
{
    public class HomeController : Controller
    {
        VideoStorContext db = new VideoStorContext();
        public ActionResult Index()
        {
            var movies = db.Movies.ToList();
            var movieRents = db.MovieRents.ToList();
            var customers = db.Customers.ToList();

            if (movies == null || movies.Count <= 0)
            {
                DataSeeder cs = new DataSeeder();
                cs.insertMovies(db);
               
            }
            if (customers == null || customers.Count <= 0)
            {
                DataSeeder cs = new DataSeeder();
                cs.insertCustomer(db);
               
            }

            if (movieRents == null || movieRents.Count <= 0)
            {
                DataSeeder cs = new DataSeeder();
                cs.insertMovieRents(db);
                              
            }

            



            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}