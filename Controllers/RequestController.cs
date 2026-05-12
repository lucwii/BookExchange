using System.Security.Claims;
using BookExchange.Models;
using BookExchange.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BookExchange.Controllers;

public class RequestController : Controller
{
    private readonly MongoDbService _db;

    public RequestController(MongoDbService db)
    {
        _db = db;
    }

    // Slanje zahteva za knjigu
    [HttpPost]
    public async Task<IActionResult> SendRequest(string bookId)
    {
        var posiljacId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var posiljacIme = User.FindFirst(ClaimTypes.Name)?.Value;

        var knjiga = await _db.Books.Find(b => b.Id == bookId).FirstOrDefaultAsync();
        if (knjiga == null) return NotFound();

        // Ne mozes poslati zahtev za svoju knjigu
        if (knjiga.VlasnikId == posiljacId)
        {
            TempData["Greska"] = "Ne možeš poslati zahtev za svoju knjigu.";
            return RedirectToAction("Details", "Book", new { id = bookId });
        }

        // Provera da li vec postoji zahtev
        var postojeci = await _db.ExchangeRequest
            .Find(r => r.KnjigaId == bookId && r.PosiljacId == posiljacId)
            .FirstOrDefaultAsync();

        if (postojeci != null)
        {
            TempData["Greska"] = "Već si poslao zahtev za ovu knjigu.";
            return RedirectToAction("Details", "Book", new { id = bookId });
        }

        var zahtev = new ExchangeRequest
        {
            KnjigaId = bookId,
            KnjigaNaslov = knjiga.Naslov,
            PosiljacId = posiljacId!,
            PosiljacIme = posiljacIme!,
            VlasnikId = knjiga.VlasnikId,
            DatumSlanja = DateTime.UtcNow,
            Status = "Na čekanju"
        };

        await _db.ExchangeRequest.InsertOneAsync(zahtev);
        TempData["Uspeh"] = "Zahtev je uspešno poslat!";
        return RedirectToAction("Details", "Book", new { id = bookId });
    }

    // Lista zahteva za knjige ulogovanog korisnika (vlasnik vidi zahteve)
    public async Task<IActionResult> Index()
    {
        var vlasnikId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var zahtevi = await _db.ExchangeRequest
            .Find(r => r.VlasnikId == vlasnikId)
            .SortByDescending(r => r.DatumSlanja)
            .ToListAsync();
        return View(zahtevi);
    }

    // Moji poslati zahtevi (posiljac vidi sta je poslao)
    public async Task<IActionResult> MyRequests()
    {
        var posiljacId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var zahtevi = await _db.ExchangeRequest
            .Find(r => r.PosiljacId == posiljacId)
            .SortByDescending(r => r.DatumSlanja)
            .ToListAsync();
        return View(zahtevi);
    }

    // Prihvatanje zahteva
    [HttpPost]
    public async Task<IActionResult> Accept(string id)
    {
        var vlasnikId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var zahtev = await _db.ExchangeRequest
            .Find(r => r.Id == id && r.VlasnikId == vlasnikId)
            .FirstOrDefaultAsync();

        if (zahtev == null) return NotFound();

        var update = Builders<ExchangeRequest>.Update
            .Set(r => r.Status, "Prihvaćen")
            .Set(r => r.PosiljacObavesten, true);

        await _db.ExchangeRequest.UpdateOneAsync(r => r.Id == id, update);
        TempData["Uspeh"] = "Zahtev je prihvaćen!";
        return RedirectToAction("Index");
    }

    // Odbijanje zahteva
    [HttpPost]
    public async Task<IActionResult> Reject(string id)
    {
        var vlasnikId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var zahtev = await _db.ExchangeRequest
            .Find(r => r.Id == id && r.VlasnikId == vlasnikId)
            .FirstOrDefaultAsync();

        if (zahtev == null) return NotFound();

        var update = Builders<ExchangeRequest>.Update
            .Set(r => r.Status, "Odbijen")
            .Set(r => r.PosiljacObavesten, true);

        await _db.ExchangeRequest.UpdateOneAsync(r => r.Id == id, update);
        TempData["Greska"] = "Zahtev je odbijen.";
        return RedirectToAction("Index");
    }
}