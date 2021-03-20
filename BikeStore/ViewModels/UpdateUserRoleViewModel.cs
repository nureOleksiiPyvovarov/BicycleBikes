using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStore.ViewModels
{
    public class UpdateUserRoleViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; }
    }
}
