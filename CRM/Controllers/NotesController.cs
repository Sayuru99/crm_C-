using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM.Models;
using CRM.Data;
using System.Linq;
using System.Threading.Tasks;

public class NotesController : Controller
{
    private readonly CRMContext _context;

    public NotesController(CRMContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var notes = await _context.Note.ToListAsync();
        return View(notes);
    }

    public async Task<IActionResult> Details(int id)
    {
        var note = await _context.Note.FindAsync(id);

        if (note == null)
        {
            return NotFound();
        }

        return View(note);
    }

    public IActionResult Create()
    {
        ViewBag.Companies = _context.Company.ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Note note)
    {
        if (ModelState.IsValid)
        {
            _context.Add(note);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(note);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var note = await _context.Note.FindAsync(id);

        if (note == null)
        {
            return NotFound();
        }

        return View(note);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Note note)
    {
        if (id != note.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(note);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(note.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }
        return View(note);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var note = await _context.Note.FindAsync(id);

        if (note == null)
        {
            return NotFound();
        }

        return View(note);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var note = await _context.Note.FindAsync(id);

        if (note == null)
        {
            return NotFound();
        }

        _context.Note.Remove(note);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    private bool NoteExists(int id)
    {
        return _context.Note.Any(e => e.Id == id);
    }
}
