using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nutrition.Models
{
    public class NutritionPlan
    {
        [Key]
        public int sMaKeHoach { get; set; }
        public int sMaChuyenGia { get; set; }
        public string sMaNguoiDung { get; set; }
        public DateTime dNgayTao { get; set; }
        public string sChiTietKeHoach { get; set; }
        public string sGioiYThucPham { get; set; }
        public int iSoCalo { get; set; }

    }
}
