using System.ComponentModel.DataAnnotations;

namespace Aklasa.Models
{
    public class Druzyna
    {
        [Key]
        public int IdDruzyny { get; set; }
        
        [Display(Name = "Nazwa klubu")]
        public String NazwaKlubu { get; set; }
        
        [Display(Name = "Ilość meczy")]
        public int IloscMeczy { get; set; }
        
        [Display(Name = "Ilość zwycięstw")]
        public int IloscZwyciestw { get; set; }
        
        [Display(Name = "Ilość remisów")]
        public int IloscRemisow { get; set; }
        
        [Display(Name = "Ilość porażek")]
        public int IloscPorazek { get; set; }
        
        [Display(Name = "Bramki zdobyte")]
        public int BramkiZdobyte { get; set; }
        
        [Display(Name = "Bramki stracone")]
        public int BramkiStracone { get; set; }
        
        [Display(Name = "Bilans bramkowy")]
        public int BilansBramkowy { get; set; }
        
        [Display(Name = "Punkty")]
        public int Punkty { get; set; }

        public ICollection<Zawodnik>? Zawodnicy { get; set; }
    }
}