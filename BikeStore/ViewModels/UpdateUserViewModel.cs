using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStore.ViewModels
{
    public class UpdateUserViewModel
    {
        public UpdateUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public List<string> Claims { get; set; }

        public IList<string> Roles { get; set; }
    }
}
