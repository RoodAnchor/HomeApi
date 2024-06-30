using HomeApi.Configuration.Enums;

namespace HomeApi.Configuration
{
    public class HomeOptions
    {
        public int FloorCount { get; set; }
        public string Telephone { get; set; }
        public HeatingEnum Heating { get; set; }
        public int CurrentVolts { get; set; }
        public bool GasConnected { get; set; }
        public int Area { get; set; }
        public MaterialEnum Material { get; set; }
        public Address Address { get; set; }

    }
}
