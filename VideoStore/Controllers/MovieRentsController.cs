using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using VideoStore.Context;
using VideoStore.migration;
using VideoStore.Models;
namespace VideoStore.Controllers
{
    public class MovieRentsController : Controller
    {
        VideoStorContext db = new VideoStorContext();

       
        // GET: /MovieRents/
        //public ActionResult Index()
        public ActionResult Index(bool OnlyUnreturned=false, string sortOrder=null)
        
        {
            ViewBag.OnlyUnreturned = OnlyUnreturned;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.titleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            IEnumerable<MovieRentModel> movieRents; 
            if (OnlyUnreturned)
            {
                 movieRents = db.MovieRents.Where(x => x.IsReturned == !OnlyUnreturned);
            }
            else
            {
                 movieRents = db.MovieRents;
            }
           
           
            switch (sortOrder)
            {
                case "name_desc":
                    movieRents = movieRents.OrderByDescending(s => s.customer.Name);
                    break;
                case "Title":
                    movieRents = movieRents.OrderBy(s => s.movie.Title);
                    break;
                case "title_desc":
                    movieRents = movieRents.OrderByDescending(s => s.movie.Title);
                    break;
                default:
                    movieRents = movieRents.OrderBy(s => s.customer.Name);
                    break;
            }

            foreach (var itm in movieRents) itm.IsOverdue= itm.ReturnDateTime < DateTime.Today? true : false;
          
            return View(movieRents.ToList());
        }



        //
        // GET: /MovieRents/Details/5
        public ActionResult Details(int id)
        {

            MovieRentModel movieRent = db.MovieRents.Single(x => x.Id == id);

            if (movieRent == null)
            {
                return HttpNotFound();
            }
            return View(movieRent);
            

           
        }

        //
        // GET: /MovieRents/Create
        public ActionResult Create()
        {
           
            MovieRentModel movieRent = new MovieRentModel();
            movieRent.RentStartDateTime = DateTime.Today;
            movieRent.ReturnDateTime = DateTime.Today.AddDays(1);
            ViewData["Customers"] = new SelectList(db.Customers.OrderBy(s => s.Name), "id", "Name");
            ViewData["Movies"] = new SelectList(db.Movies.OrderBy(s=>s.Title), "id", "Title");
            return View(movieRent);
        }
       

        //
        // POST: /MovieRents/Create
        [HttpPost]
        public ActionResult Create(MovieRentModel movieRent)
        {

          
            try
            {
                movieRent.movie = db.Movies.SingleOrDefault(x => x.Id == movieRent.MovieId);
                if (!isValidCustom(movieRent))
                {

                    ViewData["Customers"] = new SelectList(db.Customers.OrderBy(s => s.Name), "id", "Name");
                    ViewData["Movies"] = new SelectList(db.Movies.OrderBy(s => s.Title), "id", "Title");
                    return View(movieRent);
                }
                
                db.MovieRents.Add(movieRent);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

      
        public ActionResult CreateByMovieId(int id)
        {

            MovieRentModel movieRent = new MovieRentModel();
            movieRent.MovieId = id;
            movieRent.RentStartDateTime =  DateTime.Today;
            movieRent.ReturnDateTime = DateTime.Today.AddDays(1);
            ViewData["Customers"] = new SelectList(db.Customers.OrderBy(s => s.Name), "id", "Name");
            ViewData["Movies"] = new SelectList(db.Movies.OrderBy(s => s.Title), "id", "Title");
            return View(movieRent);
        }

        // POST: /MovieRents/CreateByMovieId/id
        [HttpPost]
        public ActionResult CreateByMovieId(MovieRentModel movieRent)
        {            
           return Create(movieRent);
         }
                 
        public ActionResult CreateByCustomerId(int id)
        {

            MovieRentModel movieRent = new MovieRentModel();
            movieRent.CustomerId = id;
            movieRent.RentStartDateTime = DateTime.Today;
            movieRent.ReturnDateTime = DateTime.Today.AddDays(1);
            ViewData["Customers"] = new SelectList(db.Customers.OrderBy(s => s.Name), "id", "Name");
            ViewData["Movies"] = new SelectList(db.Movies.OrderBy(s => s.Title), "id", "Title");
            return View(movieRent);
        }

        // POST: /MovieRents/CreateByCustomerId/id
        [HttpPost]
        public ActionResult CreateByCustomerId(MovieRentModel movieRent)
        {
          return  Create(movieRent);
        }

        public ActionResult CustomerMovies(int id)
        {

            ViewBag.CustomerName = db.Customers.First(x => x.Id == id).Name;
            var movieRents = db.MovieRents.Where(x => x.CustomerId == id).OrderByDescending(x => !x.IsReturned);
           // movieRents.OrderByDescending(x => x.Id);
            return View(movieRents);
        }

        public ActionResult RentedMovie(int id)
        {
            MovieModel mov=db.Movies.First(x => x.Id == id);
            ViewBag.MovieTitle = mov.Title;
            var movieRents = db.MovieRents.Where(x => x.MovieId == id).OrderByDescending(x=>!x.IsReturned);
            ViewBag.AvailableCopies = mov.NumberOfCopies;
            foreach (var itm in movieRents)
            {
                itm.IsOverdue = itm.ReturnDateTime < DateTime.Today ? true : false;
               if(!itm.IsReturned) ViewBag.AvailableCopies -=1;

            }

            return View(movieRents);
        }
        //
        // GET: /MovieRents/Edit/5
        public ActionResult Edit(int ? id)
        {

            MovieRentModel movieRent = db.MovieRents.Single(x => x.Id == id);

            if (movieRent == null)
            {
                return HttpNotFound();
            }
            ViewData["Customers"] = new SelectList(db.Customers.OrderBy(s => s.Name), "id", "Name");
            ViewData["Movies"] = new SelectList(db.Movies.OrderBy(s => s.Title), "id", "Title");
            return View(movieRent);
            
        }

        //
        // POST: /MovieRents/Edit/5
        [HttpPost]
        public ActionResult Edit(MovieRentModel movieRent)
        {
            try
            {
                db.Entry(movieRent).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /MovieRents/Delete/5
        public ActionResult Delete(int id)
        {
            ViewData["Customers"] = new SelectList(db.Customers.OrderBy(s => s.Name), "id", "Name");
            ViewData["Movies"] = new SelectList(db.Movies.OrderBy(s => s.Title), "id", "Title");

            MovieRentModel movieRnt = db.MovieRents.Single(x => x.Id == id);

            if (movieRnt == null)
            {
                return HttpNotFound();
            }
            return View(movieRnt);


            
        }

        //
        // POST: /MovieRents/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, MovieRentModel movieRent)
        {
          
               

                try
                {
                    MovieRentModel movrnt = db.MovieRents.Single(x => x.Id == id);
                    db.MovieRents.Remove(movrnt);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }

                
        }



        private bool isValidCustom(MovieRentModel movieRent)
        {
            if (movieRent.movie == null)
            {
                ModelState.AddModelError("", "You should select a movie ...");
                return false;

            }
            if (movieRent.RentStartDateTime > DateTime.Today)
            {
                ModelState.AddModelError("", "Rent date should not be greater than today");
                return false;
            }
            if (movieRent.RentStartDateTime > movieRent.ReturnDateTime)
            {
                ModelState.AddModelError("", "Rent date should not be greater than return date");
                return false;

            }
            if (movieRent.ReturnDateTime > movieRent.RentStartDateTime.AddDays(movieRent.movie.MaxRentDays))
            {
                ModelState.AddModelError("", "You cannot rent the movie more than " + movieRent.movie.MaxRentDays +" days . .");
                return false;

            }

            var rented = db.MovieRents.Where(x => x.MovieId == movieRent.MovieId && !x.IsReturned);
           
            if ( movieRent.movie.NumberOfCopies - rented.Count() <= 0)
            {
                ModelState.AddModelError("", "All copies of this movie is rented out ...");
                return false;

            }
           

            return true;
        }

             
       

    }
}
