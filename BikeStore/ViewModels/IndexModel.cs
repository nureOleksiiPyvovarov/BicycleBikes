using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BikeStore.ViewModels
{
    public class IndexModel
    {
        [BindProperty(SupportsGet = true)]
        public string SearchingString { get; set; }

        public IEnumerable<BikeCardViewModel> bikesList { get; set; }
    }
}
