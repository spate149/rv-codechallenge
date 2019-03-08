using System;
using CityVisitTrackerAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace CityVisitTrackerAPI.Models
{
    public class City
    {
        public int CityId { get; set; }

        public string Name { get; set; }

        public int StateId { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime LastUpdated { get; set; }

    }
}