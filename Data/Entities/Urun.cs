using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using HepsiDeveloper.Data.Entities.Common;

namespace HepsiDeveloper.Data.Entities
{
    public class Urun:BaseClass
    {


        [Display(Name = "Ürün Adı")]
        [Required(ErrorMessage = "Adı alanı boş geçilemez.")]
        public string Ad { get; set; }

        [Display(Name = "Açıklama")]
        public string Aciklama { get; set; }

        [Display(Name = "Fiyat")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal Fiyat { get; set; }

        [NotMapped]
        public IFormFile[] Dosyalar { get; set; }

        public List<Resim> Resimler { get; set; } = new List<Resim>();
        public List<Urun>? UrunListesi { get; set; }


    }
}
