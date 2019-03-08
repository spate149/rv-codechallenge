using System.Threading.Tasks;
using CityVisitTrackerAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CityVisitTrackerAPI.Controllers
{
    [Route("State")]
    public class StatesController : Controller
    {
        private readonly CityVisitTrackerAPIContext _context;

        public StatesController(CityVisitTrackerAPIContext context)
        {
            _context = context;
        }

        [Route("{stateId}/cities")]
        public async Task<IActionResult> Details([FromRoute] int stateId)
        {
            var stateInfo = await _context.State.Include(x => x.Cities).FirstOrDefaultAsync(x => x.StateId == stateId);
            if (stateInfo != null) return this.Ok(stateInfo.Cities); // if a state does exists we will return all the cities associated with this state.
            return this.NotFound(); // If a State does not exists we will return not found. 
        }
    }
}
