namespace TheWorld.Controllers.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNet.Authorization;
    using Microsoft.AspNet.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;
    using Services.CoordinatesService;
    using ViewModels;

    [Authorize]
    [Route("api/trips/{tripName}")]
    public class StopController : Controller
    {
        private readonly IWorldRepository repository;
        private readonly ILogger<StopController> logger;
        private readonly CoordService cordService;

        public StopController(IWorldRepository repository, ILogger<StopController> logger, CoordService cordService)
        {
            this.repository = repository;
            this.logger = logger;
            this.cordService = cordService;
        }

        [HttpGet]
        public JsonResult Get(string tripName)
        {
            if (string.IsNullOrWhiteSpace(tripName))
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new {Message = "Bad request"});
            }

            try
            {
                var trip = this.repository.GetTripByName(tripName, this.User.Identity.Name);

                if (trip == null)
                {
                    return Json(null);
                }

                var vm = Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Order));
                return Json(vm);
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error", ex);
            }

            return Json(null);
        }

        [HttpPost]
        public async Task<JsonResult> Post(string tripName, [FromBody] StopViewModel vm)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    // Map entity
                    var stop = Mapper.Map<Stop>(vm);

                    // Lookup Geocordinates
                    var cordResult = await this.cordService.Lookup(stop.Name);

                    stop.Latitude = cordResult.Latitude;
                    stop.Longtitude = cordResult.Longtitude;

                    if (!cordResult.Success)
                    {
                        this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(cordResult.Message);
                    }

                    // Save to db
                    this.repository.AddStop(tripName, this.User.Identity.Name, stop);

                    if (this.repository.SaveAll())
                    {
                        this.Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(stop);
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error", ex);
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Json(new {Error = ex.Message});
            }

            this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Error = "Validation Failed" });
        }
    }
}
