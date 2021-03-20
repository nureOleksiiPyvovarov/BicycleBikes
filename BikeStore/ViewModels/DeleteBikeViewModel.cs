using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BikeStore.ViewModels
{
    public class DeleteBikeViewModel
    {
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Confirmed name")]
        [Compare("ProductName",
            ErrorMessage = "Bike name and confirmed name doesn't match")]
        public string ConfirmedName { get; set; }

    }
}
