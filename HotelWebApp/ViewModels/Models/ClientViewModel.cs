using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelWebApp.Models;
using HotelWebApp.ViewModels.FilterViewModels;

namespace HotelWebApp.ViewModels.Models
{
    public class ClientViewModel
    {
        public IEnumerable<Client> Clients { get; set; }

        public Client Client { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public ClientsFilterViewModel ClientsFilterViewModel { get; set; }
    }
}
