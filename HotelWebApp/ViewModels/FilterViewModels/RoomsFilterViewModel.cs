using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApp.ViewModels.FilterViewModels
{
    public class RoomsFilterViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
