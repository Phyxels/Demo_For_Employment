using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo_Web.Data;
using Demo_Web.Models;

namespace Demo_Web.Controllers
{
    public class recipesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public recipesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: recipes
        public async Task<IActionResult> Index()
        {
            return View(await _context.recipe.ToListAsync());
        }

        // GET: SearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // GET: SearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.recipe.Where(j => j.ulamname.Contains(SearchPhrase)).ToListAsync());
        }

        // GET: recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.recipe
                .FirstOrDefaultAsync(m => m.id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: recipes/Create
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: recipes/Create
        [Microsoft.AspNetCore.Authorization.Authorize]
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,ulamname,ulamrecipe,ulamauthor")] recipe recipe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }

        // GET: recipes/Edit/5
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.recipe.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        // POST: recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,ulamname,ulamrecipe, ulamauthor")] recipe recipe)
        {
            if (id != recipe.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!recipeExists(recipe.id))
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
            return View(recipe);
        }

        // GET: recipes/Delete/5
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.recipe
                .FirstOrDefaultAsync(m => m.id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: recipes/Delete/5
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _context.recipe.FindAsync(id);
            _context.recipe.Remove(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool recipeExists(int id)
        {
            return _context.recipe.Any(e => e.id == id);
        }
    }
}
