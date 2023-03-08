using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using ToDo.Models;
using ToDoProject.Models.ViewModels;

namespace ToDo.Controllers
{
    public class ToDoItemController : Controller
    {
        private readonly ToDoItemDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ToDoItemController(ToDoItemDbContext context, IWebHostEnvironment webHostEnvironment) { 
            _context = context;
            _webHostEnvironment = webHostEnvironment; 
        }
       
        // GET: ToDoItem
        public async Task<IActionResult> Index() => View(await _context.ToDoItems.ToListAsync());
    

        // GET: ToDoItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ToDoItems == null)
            {
                return NotFound();
            }

            var toDoItem = await _context.ToDoItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            return View(toDoItem);
        }

        // GET: ToDoItem/Create
        public IActionResult AddOrEdit()  => View();
      

        // POST: ToDoItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id,Tittle,isActive,Image")] ToDoItemViewModel toDoItem)
        {
            if (ModelState.IsValid)
            {
                if(toDoItem.Image!= null)
                {
                    using(var stream = new FileStream(Path.Combine(_webHostEnvironment.WebRootPath, "uploadfiles", toDoItem.Tittle), FileMode.Create))
                    {
                        await toDoItem.Image.CopyToAsync(stream);
                    }
              
                   using( var memStream = new MemoryStream())
                   {
                        await toDoItem.Image.CopyToAsync(memStream);
                        if(memStream.Length < 2097152)
                        {
                            var item = new ToDoItem()
                            {
                                Tittle = toDoItem.Tittle,
                                isActive = toDoItem.isActive,
                                Image = memStream.ToArray()

                            };
                            await _context.AddAsync(item);

                        }
                    }
                }
             
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toDoItem);
        }

        // GET: ToDoItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ToDoItems == null)
            {
                return NotFound();
            }

            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem == null)
            {
                return NotFound();
            }
            return View(toDoItem);
        }

        // POST: ToDoItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tittle,isActive,Image")] ToDoItemViewModel toDoItem)
        {
            if (id != toDoItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var item = _context.ToDoItems.Find(id);
                    using(var memStream = new MemoryStream())
                    {
                         await toDoItem.Image.CopyToAsync(memStream);
                        if(memStream.Length < 2097152)
                        {
                            item.Image = memStream.ToArray();
                        }
                    }
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoItemExists(toDoItem.Id))
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
            return View(toDoItem);
        }

        // GET: ToDoItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ToDoItems == null)
            {
                return NotFound();
            }

            var toDoItem = await _context.ToDoItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            return View(toDoItem);
        }

        // POST: ToDoItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ToDoItems == null)
            {
                return Problem("Entity set 'ToDoItemDbContext.ToDoItems'  is null.");
            }
            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem != null)
            {
                _context.ToDoItems.Remove(toDoItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoItemExists(int id) => _context.ToDoItems.Any(e => e.Id == id);
       
    }
}
