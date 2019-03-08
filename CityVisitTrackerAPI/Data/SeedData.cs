using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using CityVisitTrackerAPI.Helpers;
using CityVisitTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace CityVisitTrackerAPI.Data
{
    public class SeedData
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var context =
                new CityVisitTrackerAPIContext(serviceProvider
                    .GetRequiredService<DbContextOptions<CityVisitTrackerAPIContext>>()))
            {
                // If there is a state that assuming that we have already seeded some data and we will skip this on startup
                // Seed State, Cities, Users and UserVisits
                if (context.State.Any())
                    return;
                var alabama = new State
                {
                    Abbreviation = "AL",
                    DateAdded = DateTime.Today,
                    LastUpdated = DateTime.Today,
                    Name = "Alabama",
                    Cities = new List<City>()
                };
                var alaska = new State
                {
                    Abbreviation = "AK",
                    DateAdded = DateTime.Today,
                    LastUpdated = DateTime.Today,
                    Name = "Alaska",
                    Cities = new List<City>()
                };
                var arizona = new State
                {
                    Abbreviation = "AZ",
                    DateAdded = DateTime.Today,
                    LastUpdated = DateTime.Today,
                    Name = "Arizona",
                    Cities = new List<City>()
                };

                var seedStates = new[] { alabama, alaska, arizona };
                context.State.AddRange(seedStates);
                context.SaveChanges();
                var alabamaStateId = context.State.FirstOrDefaultAsync(x => x.Name == "Alabama").Id;
                var alaskaStateId = context.State.FirstOrDefaultAsync(x => x.Name == "Alaska").Id;
                var arizonaStateId = context.State.FirstOrDefaultAsync(x => x.Name == "Arizona").Id;
                var seedCities = new[] {
                    AddCity("Akron", alabamaStateId, 32.87802m, -87.743989m),
                    AddCity("Huntsville", alabamaStateId, 34.729135m , -86.584979m   ),
                    AddCity("Addison", alabamaStateId, 32.87802m, -87.181384m ),
                    AddCity("Montgomery", alabamaStateId, 34.202175m,    -86.300629m),
                    AddCity("Birmingham", alabamaStateId, 32.38012m, -86.811504m),

                    AddCity("Adak", alaskaStateId, 51.88001m, -176.657569m),
                    AddCity("Akhiok", alaskaStateId, 56.945599m, -154.169998m),
                    AddCity("Akiak", alaskaStateId, 60.909659m, -161.223451m),
                    AddCity("Kasigluk", alaskaStateId, 60.895273m,-162.517124m),
                    AddCity("Akutan", alaskaStateId, 54.134725m, -165.770554m),

                    AddCity("Mesa", arizonaStateId, 33.417045m,  -111.831459m),
                    AddCity("Phoenix", arizonaStateId, 33.44826m, -112.075774m),
                    AddCity("Avondale", arizonaStateId, 33.4405m, -112.349664m),
                    AddCity("Mohave Valley", arizonaStateId, 34.92384m, -114.597859m),
                    AddCity("Whiteriver", arizonaStateId, 33.834865m, -109.964934m),
                };
                context.City.AddRange(seedCities);
                context.SaveChanges();
                var salt = PasswordHashGenerator.GetSalt();
                var hashedPassword = PasswordHashGenerator.ComputeHash("TestPassword", new SHA256CryptoServiceProvider(), salt);
                var user = new User
                {
                    FirstName = "Sachin",
                    LastName = "Patel",
                    DateAdded = DateTime.Today,
                    LastUpdated = DateTime.Today,
                    UserName = "spatel",
                    PasswordSalt = PasswordHashGenerator.GetSaltString(salt),
                    PasswordHash = hashedPassword
                };
                context.User.Add(user);
                context.SaveChanges();
            }
        }

        private static City AddCity(string city, int stateId, decimal lat, decimal lon)
        {
            return new City
            {
                Name = city,
                LastUpdated = DateTime.Today,
                DateAdded = DateTime.Today,
                Latitude = lat,
                Longitude = lon,
                StateId = stateId
            };
        }
    }
}
