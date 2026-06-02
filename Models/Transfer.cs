using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aklasa.Models
{
    public class Transfer
    {
        [Key]
        public int IdTransferu { get; set; }

        [Display(Name = "Zawodnik")]
        public int? ZawodnikId { get; set; }
        [ForeignKey("ZawodnikId")]
        public Zawodnik? Zawodnik { get; set; }

        [Display(Name = "Z drużyny")]
        public int? DruzynaOdId { get; set; }
        [ForeignKey("DruzynaOdId")]
        public Druzyna? DruzynaOd { get; set; }

        [Display(Name = "Do drużyny")]
        public int? DruzynaDoId { get; set; }
        [ForeignKey("DruzynaDoId")]
        public Druzyna? DruzynaDo { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data transferu")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataTransferu { get; set; }
    }
}