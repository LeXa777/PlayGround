namespace TheWorld.Services.CoordinatesService
{
    using Microsoft.Extensions.Logging;

    public class CoordService
    {
        private readonly ILogger<CoordService> logger;

        public CoordService(ILogger<CoordService> logger)
        {
            this.logger = logger;
        }

        public CoordServiceResult Lookup(string location)
        {
            var result = new CoordServiceResult
            {
                Success = false,
                Message = "Failed"
            };

            // Lookup

            return result;
        }
    }
}
