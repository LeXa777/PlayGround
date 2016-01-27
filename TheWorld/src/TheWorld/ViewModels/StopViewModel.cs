namespace TheWorld.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class StopViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Name { get; set; }
        public double Longtitude { get; set; }
        public double Latitude { get; set; }

        [Required]
        public DateTime Arrival { get; set; }
    }
}
