using System.ComponentModel.DataAnnotations;

namespace Nutrition.Models
{
    public class Nutritionist
    {
        [Key]
        public int sMaChuyenGia { get; set; }
        public string sHoTenChuyenGia { get; set; }
        public string sSDT { get; set; }
        public string sDiaChi { get; set; }
        public int iTuoi { get; set; }
        public bool bGioiTinh { get; set; }
        public string sChuyenMon { get; set; }

    }
}
