using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM.Models;
using CRM.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

public class UsersController : Controller
{
    private readonly CRMContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public UsersController(CRMContext context, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // CRUD Operations

    public async Task<IActionResult> Index()
    {
        var users = await _context.User.ToListAsync();
        return View(users);
    }

    public async Task<IActionResult> Details(int id)
    {
        var user = await _context.User.FindAsync(id);
        return View(user);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        if (ModelState.IsValid)
        {
            await _userManager.CreateAsync(user, user.Password);
            return RedirectToAction("Index");
        }
        return View(user);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var user = await _context.User.FindAsync(id);
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, User user)
    {
        if (id != user.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
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
        return View(user);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.User.FindAsync(id);
        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _context.User.FindAsync(id);
        _context.User.Remove(user);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        return View(model);
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(User user)
    {
        if (ModelState.IsValid)
        {
            await _userManager.CreateAsync(user, user.Password);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index");
        }
        return View(user);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index");
    }

    private bool UserExists(int id)
    {
        return _context.User.Any(e => e.Id == id);
    }

}
