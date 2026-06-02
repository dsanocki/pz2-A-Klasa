using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aklasa.Models
{
    public class Mecz
    {
        [Key]
        public int IdMeczu { get; set; }
        
        [Display(Name = "Gospodarz")]
        public int? GospodarzId { get; set; }
        [ForeignKey("GospodarzId")]
        public Druzyna? Gospodarz { get; set; }

        [Display(Name = "Gość")]
        public int? GoscId { get; set; }
        [ForeignKey("GoscId")]
        public Druzyna? Gosc { get; set; }

        [Display(Name = "Bramki gospodarzy")]
        public int BramkiGospodarzy { get; set; }
        
        [Display(Name = "Bramki gości")]
        public int BramkiGosci { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data meczu")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataMeczu { get; set; }
    }
}