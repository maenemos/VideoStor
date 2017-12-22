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
    public class CustomersController : Controller
    {

        VideoStorContext db = new VideoStorContext();
        //
        // GET: /Customers/
        public ActionResult Index(string sortOrder = null)
        {

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            IEnumerable<CustomerModel> customers; 
            
            if (sortOrder == "name_desc") { 
                customers = db.Customers.OrderByDescending(s => s.Name);
            }
            else
            {
                customers = db.Customers.OrderBy(s => s.Name);
            }
                     
          

            var movieRents = db.MovieRents.Where(x=>x.IsReturned==false).ToList();
            foreach (CustomerModel customer in customers)
            {
                var rented = movieRents.Where(x => x.CustomerId == customer.Id);
                customer.NumberOfRentedMovies =  rented.Count();
            }


            return View(customers.ToList());
        }

        //
        // GET: /Customers/Details/5
        public ActionResult Details(int id)
        {
            CustomerModel customer = db.Customers.Single(x => x.Id == id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            var movieRents = db.MovieRents.ToList();
            var rented = movieRents.Where(x => x.CustomerId == customer.Id && !x.IsReturned);
            customer.NumberOfRentedMovies = rented.Count();
            


            return View(customer);
           
        }

        //
        // GET: /Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Customers/Create
        [HttpPost]
        public ActionResult Create(CustomerModel customer )
        {
            try
            {
               
                if (!isUniquePersonalNumber(customer))
                {
                    ModelState.AddModelError("", "Personal number is already registered . . .");
                    return View(customer);

                }
                if (!isValidPersonNumber(customer))
                {
                  
                    return View(customer);

                }
               


                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Customers/Edit/5
        public ActionResult Edit(int ? id)
        {
            CustomerModel customer = db.Customers.Single(x => x.Id == id);
           
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
           
        }

        //
        // POST: /Customers/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CustomerModel customer)
        {
            try
            {
                if (!isUniquePersonalNumber(customer))
                {
                   
                    ModelState.AddModelError("", "Personal number is already registered . . .");
                    return View(customer);

                }

                if (!isValidPersonNumber(customer))
                {
                  
                    return View(customer);

                }

                db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
          
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Customers/Delete/5
        public ActionResult Delete(int id)
        {
            CustomerModel customer = db.Customers.Single(x => x.Id == id);

            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //
        // POST: /Customers/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, CustomerModel customer)
        {
            try
            {
                CustomerModel cus = db.Customers.Single(x => x.Id == id);
                db.Customers.Remove(cus);
                db.SaveChanges();
               
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public bool isUniquePersonalNumber(CustomerModel customer)
        {

            var obj = db.Customers.FirstOrDefault(x => x.SwedishPersonalNumber == customer.SwedishPersonalNumber && x.Id != customer.Id);
            if (obj == null) return true;
            return false;
        }

        public bool isValidPersonNumber(CustomerModel customer)
        {
            if (customer.SwedishPersonalNumber.Length != 12)
            {
                ModelState.AddModelError("", "Personal number should be 12 digits !");
                return false;
            }

            var birthYear = Convert.ToInt32(customer.SwedishPersonalNumber.Substring(0, 4));
            var birthMonth = Convert.ToInt32(customer.SwedishPersonalNumber.Substring(4, 2));
            var birthDay = Convert.ToInt32(customer.SwedishPersonalNumber.Substring(6, 2));
            DateTime birthDate = new DateTime();

            try
            {
                birthDate = new DateTime(birthYear, birthMonth, birthDay);
            }
            catch
            {
                ModelState.AddModelError("", "Wrong Personal number format, YYYY MM DD XXXX!");
                return false;
            }

            int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            int dob = int.Parse(birthDate.ToString("yyyyMMdd"));
            if(((now - dob) / 10000)<18)
            {
                ModelState.AddModelError("", "The customer is under 18 years old ...");
                return false;
            }
           
            return true;
        }

       

    }
}
