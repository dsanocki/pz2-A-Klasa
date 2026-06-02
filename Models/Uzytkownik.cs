using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aklasa.Models
{
    public class Uzytkownik
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Login { get; set; }

        public string HasloHash { get; set; } = string.Empty;

        public bool CzyAdministrator { get; set; }

        [NotMapped]
        [Display(Name = "Hasło")]
        public string? Haslo { get; set; }
    }
}