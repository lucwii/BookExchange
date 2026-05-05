using System.Security.Claims;
using BookExchange.Models;
using BookExchange.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BookExchange.Controllers;

public class BookController : Controller
{
    private readonly MongoDbService _db;
    private readonly IWebHostEnvironment _env;

    public static readonly List<string> Zanrovi = new()
    {
        "Triler", "Romansa", "Sci-Fi", "Fantazija", "Drama",
        "Horor", "Biografija", "Istorija", "Poezija", "Detektivski"
    };

    public BookController(MongoDbService db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var books = await _db.Books.Find(_ => true).ToListAsync();
        return View(books);
    }

    public async Task<IActionResult> Details(string id)
    {
        var book = await _db.Books.Find(b => b.Id == id).FirstOrDefaultAsync();
        if (book == null) return NotFound();
        return View(book);
    }

    public IActionResult Create()
    {
        ViewBag.Zanrovi = Zanrovi;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Book model, IFormFile? slika)
    {
        ViewBag.Zanrovi = Zanrovi;
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userIme = User.FindFirst(ClaimTypes.Name)?.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        var user = await _db.Users.Find(u => u.Id == userId).FirstOrDefaultAsync();

        model.VlasnikId = userId!;
        model.VlasnikIme = userIme;
        model.VlasnikEmail = userEmail;
        model.VlasnikGrad = user?.Grad ?? "";
        model.DatumObjave = DateTime.UtcNow;
        
        if (slika != null && slika.Length > 0)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(slika.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await slika.CopyToAsync(stream);
            model.SlikaPath = $"/uploads/{fileName}";
        }

        await _db.Books.InsertOneAsync(model);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> MyBooks()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var books = await _db.Books.Find(b => b.VlasnikId == userId).ToListAsync();
        return View(books);
    }

    public async Task<IActionResult> Edit(string id)
    {
        ViewBag.Zanrovi = Zanrovi;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var book = await _db.Books.Find(b => b.Id == id && b.VlasnikId == userId).FirstOrDefaultAsync();
        if (book == null) return NotFound();
        return View(book);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(string id, Book model, IFormFile? slika)
    {
        ViewBag.Zanrovi = Zanrovi;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var postojecaKnjiga = await _db.Books
            .Find(b => b.Id == id && b.VlasnikId == userId)
            .FirstOrDefaultAsync();

        if (postojecaKnjiga == null) return NotFound();

        postojecaKnjiga.Naslov = model.Naslov;
        postojecaKnjiga.Autor = model.Autor;
        postojecaKnjiga.Zanr = model.Zanr;
        postojecaKnjiga.IsNovo = model.IsNovo;
        postojecaKnjiga.Opis = model.Opis;
        postojecaKnjiga.DatumIzmene = DateTime.UtcNow;

        if (slika != null && slika.Length > 0)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(slika.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await slika.CopyToAsync(stream);
            postojecaKnjiga.SlikaPath = $"/uploads/{fileName}";
        }

        await _db.Books.ReplaceOneAsync(b => b.Id == id, postojecaKnjiga);
        return RedirectToAction("MyBooks");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _db.Books.DeleteOneAsync(b => b.Id == id && b.VlasnikId == userId);
        return RedirectToAction("MyBooks");
    }
}