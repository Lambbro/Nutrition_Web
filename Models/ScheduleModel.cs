using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nutrition.Models
{
    public class Schedule
    {
        [Key]
        public int schedule_id { get; set; }

        [ForeignKey("User")]
        public int? user_id { get; set; }

        public User? User { get; set; }
        [ForeignKey("Nutritionist")]
        public int? nutritionist_id { get; set; }
        public Nutritionist? Nutritionist { get; set; }
        public DateTime create_time { get; set; }
        public ScheduleType schedule_type { get; set; }
        public enum ScheduleType
        {
            Canceled,
            Pending,
            Confirmed
        }
    }
}
