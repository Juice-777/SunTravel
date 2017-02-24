using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunTravel.Models
{
    public class BookingForm
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter your name")]
        public string Name { get; set; }
        public string Country { get; set; }
        public string Hotel { get; set; }

        [Required(ErrorMessage = "Enter your email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Error email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your check in date")]
        [Display(Name = "Check-in")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CheckIn { get; set; }
        public int Comfort { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public int Rooms { get; set; }
        [StringLength(50, MinimumLength = 0, ErrorMessage = "String is max long 50 simvols")]
        public string Message { get; set; }
    }
}