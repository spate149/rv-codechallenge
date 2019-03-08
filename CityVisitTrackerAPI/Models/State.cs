using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityVisitTrackerAPI.Models
{
    public class State
    {
        public int StateId { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime LastUpdated { get; set; }

        public virtual List<City> Cities { get; set; }
    }
}
