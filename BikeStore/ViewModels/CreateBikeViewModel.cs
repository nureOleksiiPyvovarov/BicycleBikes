using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BikeStore.Models.EfModels;
using Microsoft.AspNetCore.Http;

namespace BikeStore.ViewModels
{
    public class CreateBikeViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Brand")]
        public int? BrandId { get; set; }
        [Required]
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        [Required]
        [Display(Name = "Model year")]
        [RegularExpression("\\d{4}", ErrorMessage = "Model year accepts only 4 digits")]
        public short? ModelYear { get; set; }
        [Required(ErrorMessage = "Price field is required")]
        [Display(Name = "Price")]
        [RegularExpression("^([-+] ?)?[0-9]+(,[0-9]+)?$", ErrorMessage = "Price accepts only digits")]
        public decimal? ListPrice { get; set; }
        public IFormFile ProductPhoto { get; set; }
    }
}
