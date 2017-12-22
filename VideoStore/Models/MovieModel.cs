using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VideoStore.Models
{
    public class MovieModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Movie title is required")]
        public string Title { get; set; }

        [DisplayName("Quantity")]
        [Required(ErrorMessage = "Quantity in the stor is required")]
        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Quantity must be greater than 0")]
        public int NumberOfCopies { get; set; }
        public string Genre { get; set; }

        [DisplayName("Maximum rent days")]
        [Required(ErrorMessage = "Maximum rent days is required")]
        public int MaxRentDays { get; set; }

         [NotMapped]
        public int AvailableCopies { get; set; }

    }
}