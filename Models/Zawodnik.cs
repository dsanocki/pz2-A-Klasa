using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aklasa.Models
{
    public class Zawodnik
    {
        [Key]
        public int IdZawodnika { get; set; }
        
        [Display(Name = "Imię")]
        public String Imie { get; set; }
        
        [Display(Name = "Nazwisko")]
        public String Nazwisko { get; set; }
        
        [Display(Name = "Ilość bramek")]
        public int IloscBramek { get; set; }
        
        [Display(Name = "Ilość asyst")]
        public int IloscAsyst { get; set; }

        [Display(Name = "Klub")]
        public int? DruzynaId { get; set; } 
        
        [ForeignKey("DruzynaId")]
        public Druzyna? Druzyna { get; set; }
    }
}