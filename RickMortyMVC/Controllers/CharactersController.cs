using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using RickMorty.Data;
using RickMorty.Domain.Models;
using RickMortyMVC.Filters;

namespace RickMortyMVC.Controllers
{
    public class CharactersController : Controller
    {
        private readonly RickMortyDbContext _context;
        private readonly IOutputCacheStore _outputCacheStore;

        public CharactersController(RickMortyDbContext context, IOutputCacheStore outputCacheStore)
        {
            _context = context;
            _outputCacheStore = outputCacheStore;
        }

        // GET: Characters
        // Marco added caching
        // [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)] 
        // I think outputcaching is more appropriate for this.
        [OutputCache(PolicyName = "Expire300")]
        //[TypeFilter(typeof(IndexResourceFilterAttribute))] // this filter is ran before the action filters and after the response filters
        //[IndexPreActionFilter("from-database", "false")] // this is ran just around the action itself.
        [HttpHeaderResponseFilter("from-database", "true")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Characters.OrderByDescending(x => x.Id).ToListAsync());
        }

        // Marco GET: Characters/Location/Earth
        //[ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client, NoStore = false)]
        [OutputCache(PolicyName = "Expire300ByQuery")]
        [HttpHeaderResponseFilter("from-database", "true")]
        public async Task<IActionResult> Planet(string? id)
        {
            // here I would probably verify with someone if this is safe url wise
            // I see that the query is using parameters, so it's probably safe
            return View(await _context.Characters.Where(c => c.Origin == id).ToListAsync());
        }

        // GET: Characters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (character == null)
            {
                return NotFound();
            }

            return View(character);
        }

        // GET: Characters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Characters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(InvalidateCacheFilterAttribute))]
        public async Task<IActionResult> Create([Bind("Id,Created,Name,Status,Species,Origin,Location,ExternalId,externalCreated")] Character character)
        {
            if (ModelState.IsValid)
            {
                _context.Add(character);
                await _context.SaveChangesAsync();

                //await _outputCacheStore.EvictByTagAsync("TagHandleForExpire300Policy", new CancellationToken()); // I left this here, but moved it to a filter. to stay in line with what I think is MS' directive on using MVC

                return RedirectToAction(nameof(Index));
            }
            return View(character);
        }

        // GET: Characters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }
            return View(character);
        }

        // POST: Characters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        // I should possibly also added some guard rails here, like remove the externalid or only allow manually created characters to be edited
        // I should also invalidate the caches after this action, but haven't for now.
        public async Task<IActionResult> Edit(int id, [Bind("Id,Created,Name,Status,Species,Origin,Location,ExternalId,externalCreated")] Character character)
        {
            if (id != character.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(character);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacterExists(character.Id))
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
            return View(character);
        }

        // GET: Characters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (character == null)
            {
                return NotFound();
            }

            return View(character);
        }

        // POST: Characters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(InvalidateCacheFilterAttribute))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var character = await _context.Characters.FindAsync(id);
            if (character != null)
            {
                _context.Characters.Remove(character);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CharacterExists(int id)
        {
            return _context.Characters.Any(e => e.Id == id);
        }
    }
}
