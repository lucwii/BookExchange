using System.ComponentModel.DataAnnotations;

namespace BookExchange.ViewModels;

public class RegisterViewModel
{
    [Required] 
    public string Ime { get; set; } = string.Empty;

    [Required] 
    public string Prezime { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress] 
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MinLength(6)]
    public string Lozinka { get; set; } = string.Empty;
    
    [Required] 
    public string Grad { get; set; } = string.Empty;

    public List<string> OmiljeniZanrovi { get; set; } = new();
}

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required] 
    public string Lozinka { get; set; } = string.Empty;
}