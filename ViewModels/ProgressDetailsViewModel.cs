using Nutrition.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nutrition.ViewModels
{
    public class ProgressDetailsViewModel
    {
        public int progress_id { get; set; }
        public int user_id { get; set; }
        public DateTime create_time { get; set; }
        public float height { get; set; }
        public float weight { get; set; }
        public string health_status { get; set; }
        public string commment { get; set; }
    }
}
