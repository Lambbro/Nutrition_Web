using System.ComponentModel.DataAnnotations;
namespace Nutrition.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; } // user_id
        public string user_name { get; set; } // user_name
        public bool gender { get; set; } // gender
        public int age { get; set; } // age
        public string number { get; set; } // number
        public string address { get; set; } // address
        public float height { get; set; } // height
        public float weight { get; set; } // weight
        public string health_info { get; set; } // health_info
        public string eating_habits { get; set; } // eating_habits
        public string meals_per_day { get; set; } //meals per day

        public ICollection<UserProgress>? user_progresses { get; set; }
        public ICollection<NutritionPlan>? plans { get; set; }
        public ICollection<Schedule>? schedules { get; set; }

    }
}
