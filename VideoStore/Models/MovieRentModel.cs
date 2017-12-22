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
    public class MovieRentModel
    {
        public int Id { get; set; }

        [DisplayName("Customer name")]
        [Required(ErrorMessage = "Customer is required")]
        public int CustomerId { get; set; }

        [DisplayName("Movie title")]
        [Required(ErrorMessage = "Movie is required")]
        public int MovieId { get; set; }

        [DisplayName("Rent date")]
        [Required(ErrorMessage = "Rent date is required")]
        [DataType(DataType.Date)]
        public DateTime RentStartDateTime { get; set; }
            

        [DisplayName("Return date")]
        [Required(ErrorMessage = "Return date is required")]
        [DataType(DataType.Date)]
        public DateTime ReturnDateTime { get; set; }

        [DisplayName("Is returned")]
        [DefaultValue(false)]
                
        public bool IsReturned { get; set; }

        [NotMapped]
        public bool IsOverdue { get; set; }
        public virtual MovieModel movie { get; set; }
        public virtual CustomerModel customer { get; set; }


    }
}