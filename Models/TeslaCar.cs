namespace Tesla
{
    public class TeslaCar
    {
        public TeslaCar()
        {
            TeslaCarGuid = Guid.NewGuid();
        }

        public int Id { get; set; }
        public Guid TeslaCarGuid { get; set; }
        public string? Model { get; set; }
        public string? SerialNumber { get; set; }
        public string? Location { get; set; }

    }
}
