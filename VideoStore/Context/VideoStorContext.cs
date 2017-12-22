using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VideoStore.Models;

namespace VideoStore.Context
{
    public class VideoStorContext : DbContext
    {
        
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<MovieModel> Movies { get; set; }
        public DbSet<MovieRentModel> MovieRents { get; set; }
    }


   
}