using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nutrition.Models
{
    public class Account
    {
        [Key]
        public int account_id { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public bool account_type { get; set; }   

        [ForeignKey("User")]
        public int? user_id { get; set; }
        public User? User { get; set; }

        [ForeignKey("Nutritionist")]
        public int? nutritionist_id { get; set; }
        public Nutritionist? Nutritionist { get; set; }
    }
}
