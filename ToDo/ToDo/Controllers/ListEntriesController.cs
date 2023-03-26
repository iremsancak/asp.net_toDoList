using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using ToDo.Models;

namespace ToDo.Controllers
{
    public class ListEntriesController : Controller
    {
        private readonly ToDoContext _context;

        public ListEntriesController(ToDoContext context)
        {
            _context = context;
        }

        // GET: ListEntry
        public async Task<IActionResult> Index()
        {
              //return _context.ListEntry != null ? 
              //            View(await _context.ListEntry.ToListAsync()) :
              //            Problem("Entity set 'ToDoContext.ListEntry'  is null.");
              return View(await _context.ListEntry.ToListAsync());

        }
        
        // GET: ListEntry/Create
        public IActionResult Create()
        {
            return View("Create");
        }

        // POST: ListEntry/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,isDone")] ListEntry listEntry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(listEntry);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
			}
            return RedirectToAction(nameof(Index));
			//return View(listEntry);
		}

        // GET: ListEntry/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ListEntry == null)
            {
                return NotFound();
            }

            var listEntry = await _context.ListEntry.FindAsync(id);
            if (listEntry == null)
            {
                return NotFound();
            }
            return View(listEntry);
        }

        // POST: ListEntry/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,isDone")] ListEntry listEntry)
        {
            if (id != listEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(listEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListEntryExists(listEntry.Id))
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
            return View(listEntry);
        }

        // GET: ListEntry/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ListEntry == null)
            {
                return NotFound();
            }

            var listEntry = await _context.ListEntry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listEntry == null)
            {
                return NotFound();
            }

            return View(listEntry);
        }

        // POST: ListEntry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ListEntry == null)
            {
                return Problem("Entity set 'ToDoContext.ListEntry'  is null.");
            }
            var listEntry = await _context.ListEntry.FindAsync(id);
            if (listEntry != null)
            {
                _context.ListEntry.Remove(listEntry);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListEntryExists(int id)
        {
          return (_context.ListEntry?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
