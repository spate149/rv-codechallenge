using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CityVisitTrackerAPI.Data;
using CityVisitTrackerAPI.Helpers;
using CityVisitTrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CityVisitTrackerAPI.Controllers
{
    [Authorize]
    [Route("User")]
    public class UsersController : Controller
    {
        private readonly CityVisitTrackerAPIContext _context;
        private readonly AppSettings _appSettings;
        public UsersController(CityVisitTrackerAPIContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public string Authenticate([FromBody] LoginModel model)
        {
            var username = model.UserName;
            var password = model.Password;
            var salt = _context.User.SingleOrDefault(x => x.UserName == username)?.PasswordSalt;
            var hashedPassword = PasswordHashGenerator.ComputeHash(password, new SHA256CryptoServiceProvider(), PasswordHashGenerator.StringToByteArray(salt));
            var user = _context.User.SingleOrDefault(x => x.UserName == username && x.PasswordHash == hashedPassword);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this._appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }

        [Route("{userId}/Visits")]
        [HttpPost]
        public async Task<IActionResult> AddUserVisitedCityAsync([FromRoute] int userId, [FromBody] CityModel model, CancellationToken cancel)
        {
            var stateVisited = await this._context.State.FirstOrDefaultAsync(x => x.Name == model.State, cancel);
            var cityVisited = stateVisited.Cities.FirstOrDefault(x => x.Name == model.City);
            if (stateVisited == null || cityVisited == null)
                return this.BadRequest("This an unknown state / city combination");
            var visit = new UserCityVisit
            {
                CityId = cityVisited.CityId,
                UserId = userId
            };
            this._context.UserCityVisits.Add(visit);
            await this._context.SaveChangesAsync(cancel);
            return this.Created("AddUserVisitedCityAsync", new { visit });
        }

        [Route("{userId}/Visits/{cityId}")]
        [HttpDelete]
        public async Task<IActionResult> RemoveVisitedCityAsync([FromRoute] int userId, [FromRoute] int cityId,
            CancellationToken cancel)
        {
            var visitedCity = await this._context.UserCityVisits.FirstOrDefaultAsync(x => x.CityId == cityId && x.UserId == userId, cancel);
            if (visitedCity == null)
                return this.BadRequest();
            this._context.UserCityVisits.Remove(visitedCity);
            this._context.SaveChanges();
            return this.Accepted();
        }

        [Route("{userId}/visits")]
        [HttpGet]
        public IActionResult GetAllVisitedCitiesByUser([FromRoute] int userId)
        {
            var userVisitedCities = this._context.UserCityVisits.Where(x => x.UserId == userId).Include(x => x.City).ToList();
            return this.Ok(userVisitedCities);
        }

        [Route("{userId}/visits/states")]
        [HttpGet]
        public IActionResult GetAllStatesVisitedByUser([FromRoute] int userId, CancellationToken cancel)
        {
            var userVisitedStateIds = this._context.UserCityVisits.Include(x => x.City).Select(x => x.City.StateId).ToList().Distinct();
            var states = userVisitedStateIds.Select(stateId => this._context.State.FirstOrDefault(x => x.StateId == stateId)).ToList().Select(x => x.Name);
            return this.Ok(states);
        }
    }
}
