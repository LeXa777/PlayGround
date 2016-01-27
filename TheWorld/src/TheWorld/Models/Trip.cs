namespace TheWorld.Models
{
    using System;
    using System.Collections.Generic;

    public class Trip
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public string UserName { get; set; }

        public ICollection<Stop> Stops { get; set; }
    }
}
