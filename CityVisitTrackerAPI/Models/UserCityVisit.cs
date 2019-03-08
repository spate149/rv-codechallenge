using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityVisitTrackerAPI.Models
{
    public class UserCityVisit
    {

        [Key, Column(Order = 0)]
        public int UserId { get; set; }

        [Key, Column(Order = 1)]
        public int CityId { get; set; }

        public virtual City City { get; set; }

    }
}