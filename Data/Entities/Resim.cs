using HepsiDeveloper.Data.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace HepsiDeveloper.Data.Entities
{
    public class Resim:BaseClass
    {
        public string DosyaAdi { get; set; }


        public int UrunuId { get; set; }

        [Required]
        public Urun Urunu { get; set; }
    }
}
