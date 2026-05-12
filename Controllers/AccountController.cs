using System.Security.Claims;
using BookExchange.Models;
using BookExchange.Services;
using BookExchange.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BookExchange.Controllers;

public class AccountController : Controller
{
    private readonly MongoDbService _db;

    public static readonly List<string> Zanrovi = new()
    {
        "Triler", "Romansa", "Sci-Fi", "Fantazija", "Drama",
        "Horor", "Biografija", "Istorija", "Poezija", "Detektivski"
    };

    public AccountController(MongoDbService db)
    {
        _db = db;
    }

    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Book");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _db.Users
            .Find(u => u.Email == model.Email)
            .FirstOrDefaultAsync();
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Lozinka, user.LozinkaHash))
        {
            ModelState.AddModelError("", "Pogrešan email ili lozinka.");
            return View(model);
        }

        await SignInUser(user);
        return RedirectToAction("Index", "Book");
    }

    public IActionResult Register()
    {
        ViewBag.Zanrovi = Zanrovi;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        ViewBag.Zanrovi = Zanrovi;

        if (!ModelState.IsValid) return View(model);

        if (model.OmiljeniZanrovi.Count > 3)
        {
            ModelState.AddModelError("", "Moras izabrati najvise 3 zanra");
            return View(model);
        }

        var postojeciUser = await _db.Users
            .Find(u => u.Email == model.Email)
            .FirstOrDefaultAsync();

        if (postojeciUser != null)
        {
            ModelState.AddModelError("", "Email je vec registrovan");
            return View(model);
        }

        var user = new User
        {
            Ime = model.Ime,
            Prezime = model.Prezime,
            Email = model.Email,
            LozinkaHash = BCrypt.Net.BCrypt.HashPassword(model.Lozinka),
            Grad = model.Grad,
            OmiljeniZanrovi = model.OmiljeniZanrovi
        };

        await _db.Users.InsertOneAsync(user);
        await SignInUser(user);
        return RedirectToAction("Index", "Book");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    private async Task SignInUser(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, $"{user.Ime} {user.Prezime}"),
            new Claim(ClaimTypes.Email, user.Email)
        };
        
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
}