using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStore.ViewModels
{
    public class HomeBikesListViewModel
    {
        public IEnumerable<BikeCardViewModel> Bikes;
        public IEnumerable<BikeCardViewModel> PreferedBikes;
    }
}
