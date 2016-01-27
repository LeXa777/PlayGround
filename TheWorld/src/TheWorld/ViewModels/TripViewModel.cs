namespace TheWorld.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TripViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Name { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
