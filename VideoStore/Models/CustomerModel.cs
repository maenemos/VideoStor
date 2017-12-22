using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VideoStore.Models
{
    
    public class CustomerModel
    {
       
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Email { get; set; }
        [DisplayName("Phone number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Personal Number is required")]
        [StringLength(12)]
        [DisplayName("Personal number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Personal number must be numeric")]
        public string SwedishPersonalNumber { get; set; }

        [NotMapped]
        [DisplayName("# Rented movies")]
        public int NumberOfRentedMovies { get; set; }
    }

        

}