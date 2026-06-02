using System.ComponentModel.DataAnnotations;

namespace Aklasa.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Haslo { get; set; }
    }
}