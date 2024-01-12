using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM.Models;
using CRM.Data;
using System.Linq;
using System.Threading.Tasks;

public class CompaniesController : Controller
{
    private readonly CRMContext _context;

    public CompaniesController(CRMContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var companies = await _context.Company.ToListAsync();
        return View(companies);
    }

    public async Task<IActionResult> Details(int id)
    {
        var company = await _context.Company.FindAsync(id);

        if (company == null)
        {
            return NotFound();
        }

        return View(company);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Company company)
    {
        if (ModelState.IsValid)
        {
            company.CreationDate = company.CreationDate.ToUniversalTime();

            _context.Add(company);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(company);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var company = await _context.Company.FindAsync(id);

        if (company == null)
        {
            return NotFound();
        }

        return View(company);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Company company)
    {
        if (id != company.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(company);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(company.Id))
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
        return View(company);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var company = await _context.Company.FindAsync(id);

        if (company == null)
        {
            return NotFound();
        }

        return View(company);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var company = await _context.Company.FindAsync(id);

        if (company == null)
        {
            return NotFound();
        }

        _context.Company.Remove(company);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    private bool CompanyExists(int id)
    {
        return _context.Company.Any(e => e.Id == id);
    }
}
