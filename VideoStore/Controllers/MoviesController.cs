using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoStore.Context;
using VideoStore.migration;
using VideoStore.Models;

namespace VideoStore.Controllers
{
    public class MoviesController : Controller
    {
        VideoStorContext db = new VideoStorContext();
       
        // GET: /Movies/
        public ActionResult Index(string sortOrder = null)
        {
            ViewBag.titleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            IEnumerable<MovieModel> movies; 
            if(sortOrder=="title_desc"){
                movies = db.Movies.OrderByDescending(s => s.Title);
            }else{
                movies = db.Movies.OrderBy(s => s.Title);
            }
                     
           
            var movieRents = db.MovieRents.Where(x=>x.IsReturned==false).ToList();
            foreach(MovieModel movie in movies){
                var rented= movieRents.Where(x=>x.MovieId==movie.Id && !x.IsReturned);
                movie.AvailableCopies =movie.NumberOfCopies - rented.Count();
            }


            return View(movies.ToList());
        }

        //
        // GET: /Movies/Details/5
        public ActionResult Details(int id)
        {
            MovieModel movie = db.Movies.Single(x => x.Id == id);
            var movieRents = db.MovieRents.Where(x => x.MovieId == movie.Id && !x.IsReturned);
            movie.AvailableCopies = movie.NumberOfCopies - movieRents.Count();
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
            
        }

        //
        // GET: /Movies/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Movies/Create
        [HttpPost]
        public ActionResult Create(MovieModel movie)
        {
            try
            {

                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Movies/Edit/5
        public ActionResult Edit(int ? id)
        {
            MovieModel movie = db.Movies.Single(x => x.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
           
        }

        //
        // POST: /Movies/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, MovieModel movie)
        {
            try
            {
                db.Entry(movie).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Movies/Delete/5
        public ActionResult Delete(int id)
        {
            MovieModel movie = db.Movies.Single(x => x.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        //
        // POST: /Movies/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, MovieModel movie)
        {
            try
            {
                MovieModel mov = db.Movies.Single(x => x.Id == id);
                db.Movies.Remove(mov);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
