# 📚 BookExchange — Platforma za pozajmljivanje i razmenu knjiga

Timski projekat u okviru predmeta Razvoj veb aplikacija i servisa.

**Tim:**
- Luka Milanović (IT8/2023) — Arhitektura, MongoDB, Auth, BookController
- Andrija Stojanović (IT7/2023) — 
- Stefan Tošić (IT6/2023) — 

## 🛠️ Tech Stack

- **Backend:** ASP.NET Core MVC (.NET 10)
- **Baza:** MongoDB Atlas (NoSQL)
- **Frontend:** Razor Views, Bootstrap 5
- **Auth:** Cookie Authentication + BCrypt

---

## ⚙️ Pokretanje projekta

### 1. Preduslovi

Pre nego što počneš, proveri da li imaš instalirano:

- [.NET 10 SDK](https://dotnet.microsoft.com/download) — proveri sa `dotnet --version`
- [Git](https://git-scm.com/downloads)
- [Visual Studio Code](https://code.visualstudio.com/) (preporučeno)

### 2. Kloniranje repozitorijuma

Otvori terminal i ukucaj:

```bash
git clone https://github.com/TVOJE_IME/BookExchange.git
cd BookExchange
```

### 3. Podešavanje konekcije sa bazom

Projekat koristi **MongoDB Atlas** (baza u cloudu). Da bi se konektovao na bazu, trebaš da kreiraš lokalni konfiguracioni fajl.

U folderu `BookExchange/` napravi novi fajl:

```
appsettings.Development.json
```

I stavi sledeći sadržaj (zameni `USERNAME` i `PASSWORD` sa kredencijalima koje ti da Luka):

```json
{
  "MongoDB": {
    "ConnectionString": "mongodb+srv://USERNAME:PASSWORD@bookexchange.jkdgxtp.mongodb.net/?appName=BookExchange",
    "DatabaseName": "BookExchange"
  }
}
```

> ⚠️ Ovaj fajl je u `.gitignore` — nikad ga ne commituj na GitHub jer sadrži lozinku!

### 4. Instalacija paketa

```bash
cd BookExchange
dotnet restore
```

### 5. Pokretanje aplikacije

```bash
dotnet run
```

Aplikacija će se pokrenuti na:

```
http://localhost:5000
```

Otvori taj link u browseru i trebalo bi da vidiš Login stranicu.

---

## 📁 Struktura projekta

```
BookExchange/
├── Controllers/
│   ├── AccountController.cs    # Registracija, Login, Logout
│   ├── BookController.cs       # CRUD za knjige, photo upload
│   └── RequestController.cs    # Zahtevi za razmenu
├── Models/
│   ├── User.cs                 # Model korisnika
│   ├── Book.cs                 # Model knjige
│   └── ExchangeRequest.cs      # Model zahteva
├── Services/
│   └── MongoDbService.cs       # Konekcija sa MongoDB
├── ViewModels/
│   └── AccountViewModels.cs    # Login/Register forme
├── Views/
│   ├── Account/                # Login, Register stranice
│   ├── Books/                  # Sve stranice za knjige
│   ├── Requests/               # Stranice za zahteve
│   └── Shared/                 # Layout, Navbar
├── wwwroot/
│   └── uploads/                # Uploadovane slike knjiga
├── appsettings.json            # Konfiguracija (bez lozinke)
└── Program.cs                  # Pokretanje i konfiguracija app
```

---

## 🔄 Git Workflow — kako radimo u timu

### Svaki put kada počneš da radiš:

```bash
git pull origin main
```

Uvek prvo povuci najnovije izmene pre nego što počneš!

### Kada završiš nešto:

```bash
git add .
git commit -m "Opis šta si uradio"
git push origin main
```

### Primeri dobrih commit poruka:

```
"Dodat BookController sa CRUD operacijama"
"Napravljen View za prikaz svih knjiga"
"Fiksiran bug u registraciji"
```

---

## 🌿 Podela posla po granama (preporučeno)

Da ne biste imali konflikte, svako radi na svojoj grani:

```bash
# Napravi svoju granu
git checkout -b ime-prezime/feature

# Npr:
git checkout -b andrija/book-views
git checkout -b stefan/request-controller
```

Kada završiš, merge-uješ na main:

```bash
git checkout main
git merge andrija/book-views
git push origin main
```

---

## ❓ Česti problemi

**Problem:** `dotnet: command not found`  
**Rešenje:** Instaliraj .NET 10 SDK sa https://dotnet.microsoft.com/download

---

**Problem:** Ne mogu da se konektujem na bazu  
**Rešenje:** Proveri da li imaš `appsettings.Development.json` sa ispravnim kredencijalima. Kontaktiraj Luku za kredencijale.

---

**Problem:** Slike se ne prikazuju  
**Rešenje:** Proveri da li postoji folder `wwwroot/uploads/` — ako ne postoji, napravi ga ručno.

---

**Problem:** `Port 5000 already in use`  
**Rešenje:**

```bash
# Pronađi i zaustavi proces na portu 5000
lsof -ti:5000 | xargs kill
```

---

## 📬 Kontakt

Za pristup bazi ili bilo kakva pitanja vezana za projekat, kontaktiraj:  
**Luka Milanović** — milanoviclukaa23@gmail.com
