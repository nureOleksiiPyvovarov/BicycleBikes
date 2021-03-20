using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStore.ViewModels
{
    public class UpdateBikeViewModel:CreateBikeViewModel
    {
        public int ProductId { get; set; }
        public string ExistingPhoto { get; set; }
    }
}
