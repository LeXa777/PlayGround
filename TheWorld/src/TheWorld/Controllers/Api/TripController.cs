namespace TheWorld.Controllers.Api
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using AutoMapper;
    using Microsoft.AspNet.Authorization;
    using Microsoft.AspNet.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;
    using ViewModels;

    [Authorize]
    [Route("api/trips")]
    public class TripController : Controller
    {
        private readonly IWorldRepository repository;
        private readonly ILogger<TripController> logger;

        public TripController(IWorldRepository repository, ILogger<TripController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                var trips = this.repository.GetUserTripsWithStops(User.Identity.Name);
                var results = Mapper.Map<IEnumerable<TripViewModel>>(trips);
                return Json(results);
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error in Get", ex);
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult Post([FromBody]TripViewModel vm)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var trip = Mapper.Map<Trip>(vm);
                    trip.UserName = this.User.Identity.Name;

                    // Save to db
                    this.logger.LogInformation("Attempting to save to db");
                    this.repository.AddTrip(trip);

                    if (this.repository.SaveAll())
                    {
                        this.Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<TripViewModel>(trip));
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("Exception in Post", ex);
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = "Failed", ModelState = ex.Message });
            }

            this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new {Message = "Failed", ModelState = this.ModelState});
        }
    }
}
