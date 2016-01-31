namespace TheWorld.Models
{
    using System;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class WorldUser : IdentityUser
    {
        public DateTime FirstTrip { get; set; }
    }
}
