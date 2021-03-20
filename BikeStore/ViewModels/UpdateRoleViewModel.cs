using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStore.ViewModels
{
    public class UpdateRoleViewModel
    {
        public int Id { get; set; }
        [Required]
        public string RoleName { get; set; }
        public List<string> Users { get; set; } = new List<string>();
    }
}
