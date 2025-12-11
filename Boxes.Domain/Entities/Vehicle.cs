namespace Boxes.Domain.Entities
{
    public class Vehicle
    {
        public string? Make { get; private set; }
        public string? Model { get; private set; }
        public int? Year { get; private set; }
        public string? LicensePlate { get; private set; }

        private Vehicle() { }

        public Vehicle(string? make, string? model, int? year, string? licensePlate)
        {
            Make = make;
            Model = model;
            Year = year;
            LicensePlate = licensePlate;
        }
    }
}
