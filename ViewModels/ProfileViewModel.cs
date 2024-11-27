﻿namespace Nutrition.ViewModels
{
    public class ProfileViewModel
    {
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

        public ICollection<ProgressListViewModel> progress_list { get; set; }
    }
}
