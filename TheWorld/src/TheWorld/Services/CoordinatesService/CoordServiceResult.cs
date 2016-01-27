namespace TheWorld.Services.CoordinatesService
{
    public class CoordServiceResult
    {
        public bool Success { get; set; }
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
        public string Message { get; set; }
    }
}