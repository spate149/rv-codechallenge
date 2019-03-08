using System;
using System.Collections.Generic;

namespace CityVisitTrackerAPI.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime LastUpdated { get; set; }

        public virtual List<UserCityVisit> Cities { get; set; }
    }
}
