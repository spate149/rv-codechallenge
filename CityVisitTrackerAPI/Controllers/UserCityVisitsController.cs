using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CityVisitTrackerAPI.Data;
using CityVisitTrackerAPI.Models;

namespace CityVisitTrackerAPI.Controllers
{
    public class UserCityVisitsController : Controller
    {
        private readonly CityVisitTrackerAPIContext _context;

        public UserCityVisitsController(CityVisitTrackerAPIContext context)
        {
            _context = context;
        }

        // GET: UserCityVisits
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserCityVisits.ToListAsync());
        }

        // GET: UserCityVisits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCityVisit = await _context.UserCityVisits
                .FirstOrDefaultAsync(m => m.CityId == id);
            if (userCityVisit == null)
            {
                return NotFound();
            }

            return View(userCityVisit);
        }

        // GET: UserCityVisits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserCityVisits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,CityId")] UserCityVisit userCityVisit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userCityVisit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userCityVisit);
        }

        // GET: UserCityVisits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCityVisit = await _context.UserCityVisits.FindAsync(id);
            if (userCityVisit == null)
            {
                return NotFound();
            }
            return View(userCityVisit);
        }

        // POST: UserCityVisits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,CityId")] UserCityVisit userCityVisit)
        {
            if (id != userCityVisit.CityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userCityVisit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserCityVisitExists(userCityVisit.CityId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userCityVisit);
        }

        // GET: UserCityVisits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCityVisit = await _context.UserCityVisits
                .FirstOrDefaultAsync(m => m.CityId == id);
            if (userCityVisit == null)
            {
                return NotFound();
            }

            return View(userCityVisit);
        }

        // POST: UserCityVisits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userCityVisit = await _context.UserCityVisits.FindAsync(id);
            _context.UserCityVisits.Remove(userCityVisit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserCityVisitExists(int id)
        {
            return _context.UserCityVisits.Any(e => e.CityId == id);
        }
    }
}
