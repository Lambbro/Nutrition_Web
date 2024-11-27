using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nutrition.Models
{
    public class UserProgress
    {
        [Key]
        public int progress_id {  get; set; }

        [ForeignKey("User")]
        public int user_id { get; set; }

        public User? User { get; set; }
        public DateTime create_time { get; set; }
        public float height { get; set; }
        public float weight { get; set; }
        public string health_status { get; set; }
        public string commment {  get; set; }
    }
}
